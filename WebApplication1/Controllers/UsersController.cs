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
using System.Data.Entity.Core.Metadata.Edm;

namespace WebApplication1.Controllers
{
    [SessionTimeout]
    public class UsersController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Users
        public async Task<ActionResult> Index()
        {
            return View(await db.tblUsers.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser tblUser = await db.tblUsers.FindAsync(id);
            if (tblUser == null)
            {
                return HttpNotFound();
            }
            return View(tblUser);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,FullName,Password,Email,Phone,Role")] tblUser tblUser, string RoleType)
        {
            if (ModelState.IsValid)
            {
                tblUser.Id = Guid.NewGuid();
                tblUser.Role = RoleType;
                tblUser.Password = HelperClass.Encrypt(tblUser.Password);
                db.tblUsers.Add(tblUser);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tblUser);
        }

        // GET: Users/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser tblUser = await db.tblUsers.FindAsync(id);
            if (tblUser == null)
            {
                return HttpNotFound();
            }
            tblUser.Password = HelperClass.Decrypt(tblUser.Password);
            return View(tblUser);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FullName,Password,Email,Phone,Role")] tblUser tblUser, string RoleType)
        {
            if (ModelState.IsValid)
            {
                tblUser.Role = RoleType;
                //string pwd = tblUser.Password.ToString();
                tblUser.Password = HelperClass.Encrypt(tblUser.Password);
                db.Entry(tblUser).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tblUser);
        }

        // GET: Users/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tblUser tblUser = await db.tblUsers.FindAsync(id);
            if (tblUser == null)
            {
                return HttpNotFound();
            }
            return View(tblUser);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            tblUser tblUser = await db.tblUsers.FindAsync(id);
            db.tblUsers.Remove(tblUser);
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
