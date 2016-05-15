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
    public class DogadjajServiceController : ApiController
    {
        private UposlenikDbContext db = new UposlenikDbContext();

        // GET: api/DogadjajService
        public IQueryable<Dogadjaj> GetDogadjaji()
        {
            return db.Dogadjaji;
        }

        // GET: api/DogadjajService/5
        [ResponseType(typeof(Dogadjaj))]
        public async Task<IHttpActionResult> GetDogadjaj(int id)
        {
            Dogadjaj dogadjaj = await db.Dogadjaji.FindAsync(id);
            if (dogadjaj == null)
            {
                return NotFound();
            }

            return Ok(dogadjaj);
        }

        // PUT: api/DogadjajService/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDogadjaj(int id, Dogadjaj dogadjaj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dogadjaj.Id)
            {
                return BadRequest();
            }

            db.Entry(dogadjaj).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DogadjajExists(id))
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

        // POST: api/DogadjajService
        [ResponseType(typeof(Dogadjaj))]
        public async Task<IHttpActionResult> PostDogadjaj(Dogadjaj dogadjaj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Dogadjaji.Add(dogadjaj);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = dogadjaj.Id }, dogadjaj);
        }

        // DELETE: api/DogadjajService/5
        [ResponseType(typeof(Dogadjaj))]
        public async Task<IHttpActionResult> DeleteDogadjaj(int id)
        {
            Dogadjaj dogadjaj = await db.Dogadjaji.FindAsync(id);
            if (dogadjaj == null)
            {
                return NotFound();
            }

            db.Dogadjaji.Remove(dogadjaj);
            await db.SaveChangesAsync();

            return Ok(dogadjaj);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DogadjajExists(int id)
        {
            return db.Dogadjaji.Count(e => e.Id == id) > 0;
        }
    }
}