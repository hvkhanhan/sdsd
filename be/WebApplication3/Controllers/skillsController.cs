﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class skillsController : ApiController
    {
        private test1Entities db = new test1Entities();

        // GET: api/skills
        //[AllowAnonymous]
        //[HttpGet]
        //[Route("api/skills/Getskills")]
        public Object Getskills()
        {
            var result = (from a in db.skills
                          select new
                          {
                              a.idskill,
                              a.skillname
                          }).ToList();
            return result;
        }
        //public IQueryable<skill> Getskills()
        //{
        //    return db.skills;
        //}

        // GET: api/skills/5
        [ResponseType(typeof(skill))]
        public Object Getskill(int id)
        {
            skill skill = db.skills.Find(id);
            if (skill == null)
            {
                return NotFound();
            }

            return Ok(skill);
        }

        // PUT: api/skills/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putskill(int id, skill skill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != skill.idskill)
            {
                return BadRequest();
            }

            db.Entry(skill).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!skillExists(id))
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

        // POST: api/skills
        [ResponseType(typeof(skill))]
        public IHttpActionResult Postskill(skill skill)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.skills.Add(skill);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = skill.idskill }, skill);
        }

        // DELETE: api/skills/5
        [ResponseType(typeof(skill))]
        public IHttpActionResult Deleteskill(int id)
        {
            skill skill = db.skills.Find(id);
            if (skill == null)
            {
                return NotFound();
            }

            db.skills.Remove(skill);
            db.SaveChanges();

            return Ok(skill);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool skillExists(int id)
        {
            return db.skills.Count(e => e.idskill == id) > 0;
        }
    }
}