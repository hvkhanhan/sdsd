﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class mentorsController : ApiController
    {
        private test1Entities db = new test1Entities();

        #region Get tat ca giang vien
        [AllowAnonymous]
        [HttpGet]
        [Route("api/mentors/Getmentors")]
        public Object Getmentors()
        {
            var result = (from a in db.mentors
                          join b in db.skills on a.idskill equals b.idskill

                          select new
                          {
                              a.mentorid,
                              a.name,
                              a.email,
                              a.phone,
                              a.address,
                              a.isActive,
                              a.password,
                              b.idskill,
                              skill = b.skillname,
                              a.activationcode,
                              a.resetpasswordcode,
                          }).ToList();
            return result;
        }

        public static string GetMD5(string str)
        {

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

            StringBuilder sbHash = new StringBuilder();

            foreach (byte b in bHash)
            {

                sbHash.Append(String.Format("{0:x2}", b));

            }

            return sbHash.ToString();

        }

        #endregion

        #region Get khoa hoc theo loai skill
        [AllowAnonymous]
        [HttpGet]
        [Route("api/mentors/Getcoursefollowskillid/{idskill}")]
        [ResponseType(typeof(course))]
        public Object Getcoursefollowskillid(int idskill)
        {
            var result = (from a in db.courses
                          join b in db.skills on a.idskill equals b.idskill
                          join e in db.trainees on a.trainee_course equals e.traineeid
                          where b.idskill == idskill
                          select new
                          {

                              a.courseid,
                              a.title,
                              a.description,
                              a.city,
                              a.address,
                              a.time,
                              a.createddate,
                              b.idskill,
                              skill = b.skillname,
                              trainee = e.name,
                              a.image_course
                          }).ToList();
            return result;
        }
        #endregion


        #region Get 1 Giang vien
        // GET: api/mentors/5
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpGet]
        [Route("api/mentors/Getmentor/{id}")]
        [ResponseType(typeof(mentor))]
        public IHttpActionResult Getmentor(int id)
        {
            mentor mentor = db.mentors.Find(id);
            if (mentor == null)
            {
                return NotFound();
            }

            return Ok(mentor);
        }

        #endregion


        #region Update thong tin Giang vien
        // PUT: api/mentors/5
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.AcceptVerbs("GET", "PUT")]
        [System.Web.Http.HttpPut]
        [Route("api/mentors/Putmentor/{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Putmentor(int id, mentor mentor)
        {


            if (id != mentor.mentorid)
            {
                return BadRequest();
            }

            db.Entry(mentor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                if (!mentorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        #endregion


        #region Post giang vien

        [AllowAnonymous]
        [Route("api/mentors/Postmentor")]
        [HttpPost]
        [ResponseType(typeof(mentor))]
        public HttpResponseMessage Postmentor(mentor mentor)
        {
            var resp = Request.CreateResponse(HttpStatusCode.BadRequest, "Mail is registered. Please try again!");
            var response = Request.CreateResponse(HttpStatusCode.Created, "Register Successfully!");
            var result = from a in db.mentors
                         join b in db.skills on a.idskill equals b.idskill
                         select a;
            Random random = new Random();
            string activationCode = random.Next(100000, 999999).ToString();
            foreach (var item in result)
            {
                if (item.email == mentor.email)
                {
                    return resp;
                }
            }
            db.mentors.Add(new mentor
            {
                name = mentor.name,
                email = mentor.email,
                phone = mentor.phone,
                address = mentor.address,
                isActive = 0,
                password = GetMD5(mentor.password),
                idskill = mentor.idskill,
                isreceivenotification = 1,
                activationcode = activationCode.ToString()

            });

            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            // setup Smtp authentication
            NetworkCredential credentials = new NetworkCredential("luffboa@gmail.com", "meomeo123456789");
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;
            //can be obtained from your model
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("luffboa@gmail.com");
            msg.To.Add(new MailAddress(mentor.email)); // Truyền người nhận

            msg.Subject = "[khanhan123]_[" + mentor.email + "]_Cảm ơn bạn đã quan tâm!"; // Tiêu đề
            msg.IsBodyHtml = true;

            msg.Body += string.Format("<html><head></head><body><b>Click Link to CONFIRM EMAIL: </b></body>");// Nội dung mail
            msg.Body += "<br /><a href = '" + string.Format("http://localhost:59786/api/mentors/ActiveEmail/?activationcode={0}", activationCode) + "'>Click here to activate your account.</a>";

            client.Send(msg);


            db.SaveChanges();

            return response;
        }
        #endregion



        #region Method Gui email xac nhan
        [AcceptVerbs("GET", "POST")]
        //[HttpPost]
        [ResponseType(typeof(mentor))]
        public IHttpActionResult ActiveEmail(string ActivationCode)
        {

            var v = db.mentors.Where(e => e.activationcode == ActivationCode).FirstOrDefault<mentor>();
            Guid originalGuid = Guid.NewGuid();
            string guild = originalGuid.ToString("D");

            if (v.activationcode.Length > 6)
            {
                return BadRequest();
            }

            Guid newGuid = Guid.Parse(guild);
            v.isActive = 1;
            v.activationcode = newGuid.ToString();
            db.SaveChanges();
            return Ok();
        }

        #endregion


        #region Method Dang nhap

        [AllowAnonymous]
        [HttpGet]
        public IHttpActionResult Login(string Email, string Password)
        {
            var v = from t in db.mentors select t;
            foreach (var item in v)
            {
                if (item.email == Email && item.password == GetMD5(Password) && item.isActive == 1)
                    return Ok(item);
            }
            return NotFound();
        }
        #endregion


        #region Method Quen mat khau

        [AllowAnonymous]
        [HttpPost]
        [Route("api/mentors/ForgotPassword")]
        [ResponseType(typeof(mentor))]
        public IHttpActionResult ForgotPassword(string Email)
        {
            var v = from t in db.mentors
                    where t.email == Email
                    select t;


            if (v.Count() < 1)
            {
                return NotFound();
            }
            Random random = new Random();
            string resetPasswordCode = random.Next(100000, 999999).ToString();

            SmtpClient client = new SmtpClient();
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            // setup Smtp authentication
            NetworkCredential credentials = new NetworkCredential("luffboa@gmail.com", "meomeo123456789");
            client.UseDefaultCredentials = false;
            client.Credentials = credentials;
            //can be obtained from your model
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("luffboa@gmail.com");
            msg.To.Add(new MailAddress(Email)); // Truyền người nhận

            msg.Subject = "[khanhan123]_[" + Email + "]_Cảm ơn bạn đã quan tâm!"; // Tiêu đề
            msg.IsBodyHtml = true;

            msg.Body += string.Format("<html><head></head><body><b>Copy <mark>CODE</mark> to Reset Password: </b></body>");// Nội dung mail
            msg.Body += "<br /><h1><b>" + resetPasswordCode + "</b></h1>";

            client.Send(msg);

            var a = db.mentors.Where(e => e.email == Email).FirstOrDefault<mentor>();
            a.resetpasswordcode = resetPasswordCode;
            db.SaveChanges();
            return Ok();
        }

        #endregion


        #region Method Khoi phuc mat khau
        [AcceptVerbs("GET", "POST")]
        [HttpPost]
        [ResponseType(typeof(mentor))]
        public IHttpActionResult ResetPassword(string resetPasswordCode, string Password)
        {
            Guid originalGuid = Guid.NewGuid();
            string guild = originalGuid.ToString("D");
            var v = from t in db.mentors select t;
            foreach (var item in v)
            {
                if (item.resetpasswordcode == resetPasswordCode)
                {
                    Guid newGuid = Guid.Parse(guild);
                    item.password = GetMD5(Password);
                    item.resetpasswordcode = newGuid.ToString();
                }
            }
            db.SaveChanges();
            return StatusCode(HttpStatusCode.NoContent);
        }


        #endregion


        // DELETE: api/mentors/5
        [ResponseType(typeof(mentor))]
        public IHttpActionResult Deletementor(int id)
        {
            mentor mentor = db.mentors.Find(id);
            if (mentor == null)
            {
                return NotFound();
            }

            db.mentors.Remove(mentor);
            db.SaveChanges();

            return Ok(mentor);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool mentorExists(int id)
        {
            return db.mentors.Count(e => e.mentorid == id) > 0;
        }
    }
}