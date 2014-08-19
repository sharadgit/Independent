using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DGuide.Infrastructure.Models;
using DGuide.Infrastructure;
using DGuide.Infrastructure.Core;

namespace DGuide.Controllers
{
    [Authorize(Roles = DGuideAuthorize.Administrators)]
    public class DbVersionController : Controller
    {
        private DGuideContext db = new DGuideContext();

        // GET: /DbVersion/
        public async Task<ActionResult> Index()
        {
            return View(await db.DbVersions.ToListAsync());
        }

        // GET: /DbVersion/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbVersion dbversion = await db.DbVersions.FindAsync(id);
            if (dbversion == null)
            {
                return HttpNotFound();
            }
            return View(dbversion);
        }

        // GET: /DbVersion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /DbVersion/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Id,Version")] DbVersion dbversion)
        {
            if (ModelState.IsValid)
            {
                db.DbVersions.Add(dbversion);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(dbversion);
        }

        // GET: /DbVersion/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbVersion dbversion = await db.DbVersions.FindAsync(id);
            if (dbversion == null)
            {
                return HttpNotFound();
            }
            return View(dbversion);
        }

        // POST: /DbVersion/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,Version")] DbVersion dbversion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dbversion).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(dbversion);
        }

        // GET: /DbVersion/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DbVersion dbversion = await db.DbVersions.FindAsync(id);
            if (dbversion == null)
            {
                return HttpNotFound();
            }
            return View(dbversion);
        }

        // POST: /DbVersion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DbVersion dbversion = await db.DbVersions.FindAsync(id);
            db.DbVersions.Remove(dbversion);
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
