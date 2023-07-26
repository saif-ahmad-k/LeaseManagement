using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using System.Globalization;

namespace WebApplication1.Controllers
{
    [SessionTimeout]
    public class LendersController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Lenders
        public async Task<ActionResult> Index()
        {
            return View(await db.tblLenders.ToListAsync());
        }

        // GET: Lenders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLender tblLender = await db.tblLenders.FindAsync(id);
            if (tblLender == null)
            {
                return HttpNotFound();
            }
            return View(tblLender);
        }

        // GET: Lenders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Lenders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name")] tblLender tblLender)
        {
            if (ModelState.IsValid)
            {
                db.tblLenders.Add(tblLender);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tblLender);
        }

        // GET: Lenders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLender tblLender = await db.tblLenders.FindAsync(id);
            if (tblLender == null)
            {
                return HttpNotFound();
            }
            return View(tblLender);
        }

        // POST: Lenders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name")] tblLender tblLender)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tblLender).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tblLender);
        }

        // GET: Lenders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblLender tblLender = await db.tblLenders.FindAsync(id);
            if (tblLender == null)
            {
                return HttpNotFound();
            }
            return View(tblLender);
        }

        // POST: Lenders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            tblLender tblLender = await db.tblLenders.FindAsync(id);
            db.tblLenders.Remove(tblLender);
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
