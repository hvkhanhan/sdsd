using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Threading.Tasks;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class coursesController : ApiController
    {
        private test1Entities db = new test1Entities();

        // GET: api/courses
        #region Get tat ca khoa hoc
        [AllowAnonymous]
        [HttpGet]
        [Route("api/courses/Getcourses")]
        public Object Getcourses()
        {
            var result = (from a in db.courses
                          join b in db.skills on a.idskill equals b.idskill
                          join c in db.trainees on a.trainee_course equals c.traineeid

                          select new
                          {
                              a.courseid,
                              a.title,
                              a.description,
                              a.city,
                              a.address,
                              a.time,
                              a.createddate,
                              //a.status,
                              a.isnotification,
                              a.idskill,
                              a.trainee_course,
                              skill = b.skillname,
                              trainee = c.name,
                              a.image_course,
                          }).ToList();
            return result;
        }
        #endregion


        #region Get khoa hoc theo hoc viên
        [AllowAnonymous]
        [HttpGet]
        [Route("api/courses/Getcoursefollowtrainee/{traineeid}")]
        [ResponseType(typeof(course))]
        public Object Getcoursefollowtrainee(int traineeid)
        {
            var result = (from a in db.courses
                          join b in db.skills on a.idskill equals b.idskill
                          join c in db.trainees on a.trainee_course equals c.traineeid
                          where a.trainee_course == traineeid
                          select new
                          {
                              a.courseid,
                              a.title,
                              a.description,
                              a.city,
                              a.address,
                              a.time,
                              a.createddate,
                              //a.status,
                              a.isnotification,
                              skill = b.skillname,
                              trainee = c.name,
                              a.image_course,
                          }).ToList();
            return result;
        }
        #endregion


        #region Get khoa hoc theo loai
        [AllowAnonymous]
        [HttpGet]
        [Route("api/courses/Getcoursefollowcategory/{categoryid}")]
        [ResponseType(typeof(course))]
        public Object Getcoursefollowcategory(int categoryid)
        {
            var result = (from a in db.courses
                          join b in db.skills on a.idskill equals b.idskill
                          join c in db.trainees on a.trainee_course equals c.traineeid
                          join d in db.categories on b.categoryid equals d.categoryid
                          where b.categoryid == categoryid && b.categoryid == d.categoryid
                          select new
                          {
                              a.courseid,
                              a.title,
                              a.description,
                              a.city,
                              a.address,
                              a.time,
                              a.createddate,
                              //a.status,
                              skill = b.skillname,
                              trainee = c.name,
                              a.image_course,
                          }).ToList();
            return result;
        }
        #endregion
        // GET: api/courses/5

        #region Get khoa hoc theo ma khoa hoc
        [System.Web.Http.AllowAnonymous]
        [System.Web.Http.HttpGet]
        [Route("api/courses/Getcourse/{id}")]
        [ResponseType(typeof(course))]
        public IHttpActionResult Getcourse(string id)
        {
            course course = db.courses.Find(id);
            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        #endregion

        #region Put khoa hoc
        // PUT: api/courses/5
        [Route("api/courses/Putcourse/{courseid}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Putcourse(string courseid, course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Entry(course).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!courseExists(courseid))
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

        
        [ResponseType(typeof(void))]
        public IHttpActionResult PutFile(string id, course course)
        {
            db.Entry(course).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!courseExists(id))
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

        #region Post khoa hoc
        // POST: api/courses
        //[AllowAnonymous]
        //[Route("api/courses/Postcourse", Name = "Postcourse")]
        //[HttpPost]
        //[ResponseType(typeof(course))]
        //public IHttpActionResult Postcourse([FromBody]course course)
        //{
        //    string imageName = course.image_course;
        //    var httpRequest = HttpContext.Current.Request;
        //    var postFile = httpRequest.Files[0];

        //    //
        //    //var mediaTypeFormatter = new FormMultipartEncodedMediaTypeFormatter(new MultipartFormatterSettings()
        //    //{
        //    //    SerializeByteArrayAsHttpFile = true,
        //    //    CultureInfo = CultureInfo.CurrentCulture,
        //    //    ValidateNonNullableMissedProperty = true
        //    //});
        //    //using (new WebApiHttpServer(BaseApiAddress, mediaTypeFormatter))
        //    //using (var client = CreateHttpClient(BaseApiAddress))
        //    //using (HttpResponseMessage response = client.PostAsync(url, model, mediaTypeFormatter).Result)
        //    //{
        //    //    if (response.StatusCode != HttpStatusCode.OK)
        //    //    {
        //    //        var err = response.Content.ReadAsStringAsync().Result;
        //    //        Assert.Fail(err);
        //    //    }

        //    //    var resultModel = response.Content.ReadAsAsync<ApiResult<T>>(new[] { mediaTypeFormatter }).Result;
        //    //    return resultModel;
        //    //}
        //    //

        //    imageName = new String(Path.GetFileNameWithoutExtension(postFile.FileName).Take(20).ToArray()).Replace(" ", "-");
        //    imageName = imageName + Path.GetExtension(postFile.FileName);

        //    var filePath = HttpContext.Current.Server.MapPath("~/Images/" + imageName);
        //    //var filePath = "~/assets/image/" + imageName;
        //    postFile.SaveAs(filePath);

        //    //string imageName = null ;
        //    //var httpRequest = HttpContext.Current.Request;
        //    //var postFile = httpRequest.Files[0];

        //    //imageName = new String(Path.GetFileNameWithoutExtension(postFile.FileName).Take(10).ToArray()).Replace(" ", "-");
        //    //imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postFile.FileName);

        //    //var filePath = HttpContext.Current.Server.MapPath("~/Images/" + imageName);
        //    //postFile.SaveAs(filePath);


        //    Guid originalGuid = Guid.NewGuid();
        //    string guild = originalGuid.ToString("D");
        //    Guid newGuid = Guid.Parse(guild);


        //    var result = from a in db.courses
        //                 join b in db.skills on a.idskill equals b.idskill
        //                 join c in db.trainees on a.trainee_course equals c.traineeid
        //                 join d in db.categories on b.categoryid equals d.categoryid
        //                 where b.idskill == course.idskill && c.traineeid == a.trainee_course
        //                 select a;
        //    foreach (var item in result)
        //    {
        //        if (item.idskill == course.idskill && item.trainee_course == course.trainee_course)
        //        {
        //            return BadRequest();
        //        }
        //    }
        //    db.courses.Add(new course
        //    {
        //        courseid = newGuid.ToString(),
        //        title = course.title,
        //        description = course.description,
        //        city = course.city,
        //        address = course.address,
        //        time = course.time,
        //        createddate = DateTime.UtcNow.Date,
        //        status = 0,
        //        idskill = course.idskill,
        //        trainee_course = course.trainee_course,
        //        isnotification = 1,
        //        image_course = imageName,
        //    });


        //    db.SaveChanges();

        //    var v = (from a in db.courses
        //             join b in db.skills on a.idskill equals b.idskill
        //             join c in db.categories on b.categoryid equals c.categoryid
        //             join d in db.mentors on b.idskill equals d.idskill
        //             join e in db.trainees on a.trainee_course equals e.traineeid
        //             where b.idskill == d.idskill && a.idskill == a.idskill
        //             select new
        //             {
        //                 a.isnotification,
        //                 d.isreceivenotification,
        //                 d.email,
        //                 titlecourse = a.title,
        //                 addresscourse = a.address,
        //                 citycourse = a.city,
        //                 timecourse = a.time,
        //                 b.idskill,
        //                 d.name,
        //                 b.skillname,
        //                 nametrainee = e.name,
        //                 emailtrainee = e.email,
        //                 phonetrainee = e.phone
        //             }).ToList();

        //    foreach (var item in v)
        //    {
        //        if (item.idskill == course.idskill && item.isnotification == item.isreceivenotification)
        //        {
        //            SmtpClient client = new SmtpClient();
        //            client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //            client.EnableSsl = true;
        //            client.Host = "smtp.gmail.com";
        //            client.Port = 587;
        //            // setup Smtp authentication
        //            System.Net.NetworkCredential credentials =
        //                new System.Net.NetworkCredential("luffboa@gmail.com", "meomeo123456789");
        //            client.UseDefaultCredentials = false;
        //            client.Credentials = credentials;
        //            //can be obtained from your model
        //            MailMessage msg = new MailMessage();
        //            msg.From = new MailAddress("luffboa@gmail.com");
        //            msg.To.Add(new MailAddress(item.email)); // Truyền người nhận

        //            msg.Subject = "[" + item.nametrainee + "] - Tôi đang quan tâm đến khóa học - " + item.skillname; // Tiêu đề
        //            msg.IsBodyHtml = true;

        //            msg.Body = string.Format("<html><head></head><body><b>Xin Chào " + item.name + ".<br/>" + "Đây là thông tin khóa học:<br/> - Tên khóa học: " + "<h2 style=" + "color:red" + ">" + item.titlecourse + "</h2>" + "<br/>- Địa chỉ: " + "<b style=" + "color:blue" + ">" + item.addresscourse + "," + item.citycourse + "</b>" + "<br/>- Thời gian học: " + "<mark>" + item.timecourse + "</mark>" + "<br/>Nếu thích hợp với khóa học thì bạn vui lòng liên hệ với tôi qua email: " + "<mark>" + item.emailtrainee + "</mark>" + "<br /> Hoặc Số điện thoại: " + "<a href=" + "tel:" + item.phonetrainee + " >" + "<mark>" + item.phonetrainee + "</mark>" + "</a>" + "(Nhấn để gọi trực tiếp)" + "</b></body>");// Nội dung mail
        //            client.Send(msg);
        //        }

        //    }

        //    return CreatedAtRoute("Postcourse", new { id = course.courseid }, course);
        //}
        #endregion

        [AllowAnonymous]
        [Route("api/courses/Postcourse", Name = "Postcourse")]
        [HttpPost]
        [ResponseType(typeof(course))]
        public IHttpActionResult Postcourse([FromBody]course course)
        {
            var result = from a in db.courses
                         join b in db.skills on a.idskill equals b.idskill
                         join c in db.trainees on a.trainee_course equals c.traineeid
                         join d in db.categories on b.categoryid equals d.categoryid
                         where b.idskill == course.idskill && c.traineeid == a.trainee_course
                         select a;
            foreach (var item in result)
            {
                if (item.idskill == course.idskill && item.trainee_course == course.trainee_course)
                {
                    return BadRequest();
                }
            }

            db.courses.Add(new course
            {
                title = course.title,
                description = course.description,
                city = course.city,
                address = course.address,
                time = course.time,
                createddate = DateTime.UtcNow.Date,
                idskill = course.idskill,
                trainee_course = course.trainee_course,
                isnotification = 1,
                image_course = "sdf",
            });

            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = course.courseid }, course);
        }

        [AllowAnonymous]
        [Route("api/courses/TestPost", Name = "TestPost")]
        [HttpPost]
        [ResponseType(typeof(course))]
        public IHttpActionResult TestPost(course course)
        {

            //string imageName = null;
            //var httpRequest = HttpContext.Current.Request;
            //var postFile = httpRequest.Files[0];

            //imageName = new String(Path.GetFileNameWithoutExtension(postFile.FileName).Take(10).ToArray()).Replace(" ", "-");
            //imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postFile.FileName);

            //var filePath = HttpContext.Current.Server.MapPath("~/Images/" + imageName);
            //postFile.SaveAs(filePath);

            string imageName = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {

                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filePath = HttpContext.Current.Server.MapPath("~/" + postedFile.FileName);
                    imageName = postedFile.FileName;
                    postedFile.SaveAs(filePath);
                }
            }



            Guid originalGuid = Guid.NewGuid();
            string guild = originalGuid.ToString("D");
            Guid newGuid = Guid.Parse(guild);


            var result = from a in db.courses
                         join b in db.skills on a.idskill equals b.idskill
                         join c in db.trainees on a.trainee_course equals c.traineeid
                         join d in db.categories on b.categoryid equals d.categoryid
                         where b.idskill == a.idskill && c.traineeid == a.trainee_course
                         select a;
            foreach (var item in result)
            {
                if (item.idskill == course.idskill)
                {
                    return BadRequest();
                }
            }
            db.courses.Add(new course
            {
                courseid = newGuid.ToString(),
                title = course.title,
                description = course.description,
                city = course.city,
                address = course.address,
                time = course.time,
                createddate = DateTime.UtcNow.Date,
                //status = 0,
                idskill = course.idskill,
                trainee_course = course.trainee_course,
                isnotification = 1,
                image_course = imageName,
            });


            db.SaveChanges();


            return CreatedAtRoute("TestPost", new { id = course.courseid }, course);
        }

        #region Delete khoa hoc
        // DELETE: api/courses/5
        [Route("api/courses/Deletecourse/{courseid}")]
        [HttpDelete]
        [ResponseType(typeof(course))]
        public IHttpActionResult Deletecourse(string courseid)
        {
            course course = db.courses.Find(courseid);
            if (course == null)
            {
                return NotFound();
            }

            db.courses.Remove(course);
            db.SaveChanges();

            return Ok(course);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private bool courseExists(string id)
        {
            return db.courses.Count(e => e.courseid == id) > 0;
        }
    }
}