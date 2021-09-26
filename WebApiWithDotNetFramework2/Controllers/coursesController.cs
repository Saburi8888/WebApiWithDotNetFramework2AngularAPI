using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApiWithDotNetFramework2.Models;

namespace WebApiWithDotNetFramework2.Controllers
{
    public class coursesController : ApiController
    {
        private StudentEntities db = new StudentEntities();

        // GET: api/courses
        public IQueryable<course> Getcourses()
        {
            return db.courses;
        }

        // GET: api/courses/5
        [ResponseType(typeof(course))]
        public async Task<IHttpActionResult> Getcourse(string id)
        {
            course course = await db.courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            return Ok(course);
        }

        // PUT: api/courses/5
        [ResponseType(typeof(void))]
        public string Putstudent(string id, [FromBody] course course)
        {
            using (StudentEntities entities = new StudentEntities())
            {
                var entity = entities.courses.FirstOrDefault(s => s.courseid == id);
                entity.coursename = course.coursename;
                entity.coursecategory = course.coursecategory;
                entity.coursefees = course.coursefees;
                entity.courseduration = course.courseduration;
                
                entities.SaveChanges();
                return "Update successfully";
            }
        }
        //public async Task<IHttpActionResult> Putcourse(string id, course course)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != course.courseid)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(course).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!courseExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/courses
        [ResponseType(typeof(course))]
        public async Task<IHttpActionResult> Postcourse(course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.courses.Add(course);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (courseExists(course.courseid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return Ok("Added successfully");
        }

        // DELETE: api/courses/5
        [ResponseType(typeof(course))]
        public async Task<IHttpActionResult> Deletecourse(string id)
        {
            course course = await db.courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            db.courses.Remove(course);
            await db.SaveChangesAsync();

            return Ok("Delete Successfully!!");
        }

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