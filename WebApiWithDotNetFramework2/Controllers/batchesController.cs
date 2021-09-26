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
    public class batchesController : ApiController
    {
        private StudentEntities db = new StudentEntities();

        // GET: api/batches
        public IQueryable<batch> Getbatches()
        {
            return db.batches;
        }

        // GET: api/batches/5
        [ResponseType(typeof(batch))]
        public async Task<IHttpActionResult> Getbatch(string id)
        {
            batch batch = await db.batches.FindAsync(id);
            if (batch == null)
            {
                return NotFound();
            }

            return Ok(batch);
        }

        // PUT: api/batches/5
        [ResponseType(typeof(void))]
        public string Putstudent(string id, [FromBody] batch batch)
        {
            using (StudentEntities entities = new StudentEntities())
            {
                var entity = entities.batches.FirstOrDefault(b => b.batchid== id);
                entity.bsdate= batch.bsdate;
                entity.bstrength= batch.bstrength;
                entity.courseid= batch.courseid;
                entities.SaveChanges();
                return "Update successfully";
            }
        }
        //public async Task<IHttpActionResult> Putbatch(string id, batch batch)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != batch.batchid)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(batch).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!batchExists(id))
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

        // POST: api/batches
        [ResponseType(typeof(batch))]
        public async Task<IHttpActionResult> Postbatch(batch batch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.batches.Add(batch);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (batchExists(batch.batchid))
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

        // DELETE: api/batches/5
        [ResponseType(typeof(batch))]
        public async Task<IHttpActionResult> Deletebatch(string id)
        {
            batch batch = await db.batches.FindAsync(id);
            if (batch == null)
            {
                return NotFound();
            }

            db.batches.Remove(batch);
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

        private bool batchExists(string id)
        {
            return db.batches.Count(e => e.batchid == id) > 0;
        }
    }
}