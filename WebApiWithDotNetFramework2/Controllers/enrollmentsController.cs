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
    public class enrollmentsController : ApiController
    {
        private StudentEntities db = new StudentEntities();

        // GET: api/enrollments
        public IQueryable<enrollment> Getenrollments()
        {
            return db.enrollments;
        }

        // GET: api/enrollments/5

        //public IHttpActionResult Edit(int? bid, int? sid)
        //{
        //    enrollment enrollment = db.enrollments.Find(bid, sid);
        //    if (enrollment == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(enrollment);
        //}
        //if (record == null)
        //{
        //    return HttpNotFound();
        //}
        //return View(record);
        //}
        [HttpGet]
        [Route("api/enrollments/{id1}/{id2}")]
        [ResponseType(typeof(enrollment))]
        public async Task<IHttpActionResult> Getenrollment(string id1, string id2)
        {

            var enrollment = db.enrollments.Where(up => up.batchid == id1 && up.sid == id2).FirstOrDefaultAsync();

            if (enrollment == null)
            {
                return NotFound();
            }

            return Ok(enrollment);
            
        }
        //[HttpGet]
        //[Route("api/enrollments/{id1}/{id2}")]
        //[ResponseType(typeof(enrollment))]
        //public async Task<IHttpActionResult> Getenrollment(string id1, string id2)
        //{
        //    enrollment enrollment = await db.enrollments.FindAsync(id1 && id2);
        //    if (enrollment == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(enrollment);
        //}

        // PUT: api/enrollments/5/4 -->update only enrollment date
        [HttpPut]
        [Route("api/enrollments/{id1}/{id2}")]
        [ResponseType(typeof(void))]
        public string Putstudent(string id1, string id2, [FromBody] enrollment enrollment)
        {
            using (StudentEntities entities = new StudentEntities())
            {
                var entity = entities.enrollments.FirstOrDefault(e => e.batchid == id1 && e.sid ==id2);
                entity.edate = enrollment.edate;


                entities.SaveChanges();
                return "Update successfully";
            }
        }
        //public async Task<IHttpActionResult> Putenrollment(string id, enrollment enrollment)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != enrollment.batchid)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(enrollment).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!enrollmentExists(id))
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

        // POST: api/enrollments
        [ResponseType(typeof(enrollment))]
        public async Task<IHttpActionResult> Postenrollment(enrollment enrollment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.enrollments.Add(enrollment);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (enrollmentExists(enrollment.batchid))
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

        // DELETE: api/enrollments/5
        [HttpDelete]
        [Route("api/enrollments/{id1}/{id2}")]
        [ResponseType(typeof(enrollment))]
        public async Task<IHttpActionResult> Deleteenrollment(string id1, string id2)
        {
            var enrollment = db.enrollments.Where(up => up.batchid == id1 && up.sid == id2).FirstOrDefaultAsync();

            //enrollment enrollment = await db.enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return NotFound();
            }

            db.enrollments.Remove(await enrollment);
            await db.SaveChangesAsync();

            return Ok("Delete successfully!!");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool enrollmentExists(string id)
        {
            return db.enrollments.Count(e => e.batchid == id) > 0;
        }
    }
}