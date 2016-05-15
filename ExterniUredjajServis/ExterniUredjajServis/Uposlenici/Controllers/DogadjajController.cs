using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExterniUredjajServis.Uposlenici.Models;

namespace ExterniUredjajServis.Uposlenici.Controllers
{
    public class DogadjajController : Controller
    {
        private UposlenikDbContext db = new UposlenikDbContext();

        // GET: Dogadjaj
        public async Task<ActionResult> Index()
        {
            var dogadjaji = db.Dogadjaji.Include(d => d.Uposlenik);
            return View(await dogadjaji.ToListAsync());
        }

        // GET: Dogadjaj/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dogadjaj dogadjaj = await db.Dogadjaji.FindAsync(id);
            if (dogadjaj == null)
            {
                return HttpNotFound();
            }
            return View(dogadjaj);
        }

        // GET: Dogadjaj/Create
        public ActionResult Create()
        {
            ViewBag.UposlenikId = new SelectList(db.Uposlenici, "Id", "Ime");
            return View();
        }

        // POST: Dogadjaj/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,DatumVrijemeDogadjaja,UposlenikId")] Dogadjaj dogadjaj)
        {
            if (ModelState.IsValid)
            {
                db.Dogadjaji.Add(dogadjaj);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.UposlenikId = new SelectList(db.Uposlenici, "Id", "Ime", dogadjaj.UposlenikId);
            return View(dogadjaj);
        }

        // GET: Dogadjaj/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dogadjaj dogadjaj = await db.Dogadjaji.FindAsync(id);
            if (dogadjaj == null)
            {
                return HttpNotFound();
            }
            ViewBag.UposlenikId = new SelectList(db.Uposlenici, "Id", "Ime", dogadjaj.UposlenikId);
            return View(dogadjaj);
        }

        // POST: Dogadjaj/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,DatumVrijemeDogadjaja,UposlenikId")] Dogadjaj dogadjaj)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dogadjaj).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.UposlenikId = new SelectList(db.Uposlenici, "Id", "Ime", dogadjaj.UposlenikId);
            return View(dogadjaj);
        }

        // GET: Dogadjaj/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dogadjaj dogadjaj = await db.Dogadjaji.FindAsync(id);
            if (dogadjaj == null)
            {
                return HttpNotFound();
            }
            return View(dogadjaj);
        }

        // POST: Dogadjaj/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Dogadjaj dogadjaj = await db.Dogadjaji.FindAsync(id);
            db.Dogadjaji.Remove(dogadjaj);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
