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
    public class UposlenikController : Controller
    {
        private UposlenikDbContext db = new UposlenikDbContext();

        // GET: Uposlenik
        public async Task<ActionResult> Index()
        {
            return View(await db.Uposlenici.ToListAsync());
        }

        // GET: Uposlenik/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uposlenik uposlenik = await db.Uposlenici.FindAsync(id);
            if (uposlenik == null)
            {
                return HttpNotFound();
            }
            return View(uposlenik);
        }

        // GET: Uposlenik/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Uposlenik/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Ime,Prezime,Pozicija,RfidKartica,KreditnaKartica,Slika")] Uposlenik uposlenik)
        {
            if (ModelState.IsValid)
            {
                db.Uposlenici.Add(uposlenik);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(uposlenik);
        }

        // GET: Uposlenik/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uposlenik uposlenik = await db.Uposlenici.FindAsync(id);
            if (uposlenik == null)
            {
                return HttpNotFound();
            }
            return View(uposlenik);
        }

        // POST: Uposlenik/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Ime,Prezime,Pozicija,RfidKartica,KreditnaKartica,Slika")] Uposlenik uposlenik)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uposlenik).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(uposlenik);
        }

        // GET: Uposlenik/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uposlenik uposlenik = await db.Uposlenici.FindAsync(id);
            if (uposlenik == null)
            {
                return HttpNotFound();
            }
            return View(uposlenik);
        }

        // POST: Uposlenik/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Uposlenik uposlenik = await db.Uposlenici.FindAsync(id);
            db.Uposlenici.Remove(uposlenik);
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
