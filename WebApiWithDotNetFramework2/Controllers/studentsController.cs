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
    public class studentsController : ApiController
    {
        private StudentEntities db = new StudentEntities();

        // GET: api/students
        public IQueryable<student> Getstudents()
        {
            return db.students;
        }

        // GET: api/students/5
        [ResponseType(typeof(student))]
        public async Task<IHttpActionResult> Getstudent(string id)
        {
            student student = await db.students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            return Ok(student);
        }

        // PUT: api/students/5
        [ResponseType(typeof(void))]

        public string Putstudent(string id, [FromBody] student student)
        {
            using(StudentEntities entities = new StudentEntities())
            {
                var entity = entities.students.FirstOrDefault(s => s.sid == id);
                entity.sname = student.sname;
                entity.sdob = student.sdob;
                entity.scity = student.scity;
                entity.squal = student.squal;
                entity.semail = student.semail;
                entity.sphone = student.sphone;

                entities.SaveChanges();
                return "Update successfully";
            }
        }
        //public async Task<IHttpActionResult> Putstudent(string id, student student)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != student.sid)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(student).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!studentExists(id))
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

        // POST: api/students
        [ResponseType(typeof(student))]
        public async Task<IHttpActionResult> Poststudent(student student)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.students.Add(student);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (studentExists(student.sid))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return Ok("Added successfully");
            //return CreatedAtRoute("DefaultApi", new { id = student.sid }, student);
        }

        // DELETE: api/students/5
        [ResponseType(typeof(student))]
        public async Task<IHttpActionResult> Deletestudent(string id)
        {
            student student = await db.students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            db.students.Remove(student);
            await db.SaveChangesAsync();

            return Ok(student);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool studentExists(string id)
        {
            return db.students.Count(e => e.sid == id) > 0;
        }
    }
}