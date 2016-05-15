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
using ExterniUredjajServis.Uposlenici.Models;

namespace ExterniUredjajServis.Uposlenici.Controllers
{
    public class UposlenikServiceController : ApiController
    {
        private UposlenikDbContext db = new UposlenikDbContext();

        // GET: api/UposlenikService
        public IQueryable<Uposlenik> GetUposlenici()
        {
            return db.Uposlenici;
        }

        // GET: api/UposlenikService/5
        [ResponseType(typeof(Uposlenik))]
        public async Task<IHttpActionResult> GetUposlenik(int id)
        {
            Uposlenik uposlenik = await db.Uposlenici.FindAsync(id);
            if (uposlenik == null)
            {
                return NotFound();
            }

            return Ok(uposlenik);
        }

        // PUT: api/UposlenikService/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUposlenik(int id, Uposlenik uposlenik)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != uposlenik.Id)
            {
                return BadRequest();
            }

            db.Entry(uposlenik).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UposlenikExists(id))
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

        // POST: api/UposlenikService
        [ResponseType(typeof(Uposlenik))]
        public async Task<IHttpActionResult> PostUposlenik(Uposlenik uposlenik)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Uposlenici.Add(uposlenik);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = uposlenik.Id }, uposlenik);
        }

        // DELETE: api/UposlenikService/5
        [ResponseType(typeof(Uposlenik))]
        public async Task<IHttpActionResult> DeleteUposlenik(int id)
        {
            Uposlenik uposlenik = await db.Uposlenici.FindAsync(id);
            if (uposlenik == null)
            {
                return NotFound();
            }

            db.Uposlenici.Remove(uposlenik);
            await db.SaveChangesAsync();

            return Ok(uposlenik);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UposlenikExists(int id)
        {
            return db.Uposlenici.Count(e => e.Id == id) > 0;
        }
    }
}