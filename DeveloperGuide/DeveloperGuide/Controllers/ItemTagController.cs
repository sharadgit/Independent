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
    public class ItemTagController : Controller
    {
        private DGuideContext db = new DGuideContext();

        // GET: /ItemTag/
        public async Task<ActionResult> Index()
        {
            return View(await db.ItemTags.ToListAsync());
        }

        // GET: /ItemTag/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemTag itemtag = await db.ItemTags.FindAsync(id);
            if (itemtag == null)
            {
                return HttpNotFound();
            }
            return View(itemtag);
        }

        // GET: /ItemTag/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /ItemTag/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Id,Tag")] ItemTag itemtag)
        {
            if (ModelState.IsValid)
            {
                db.ItemTags.Add(itemtag);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(itemtag);
        }

        // GET: /ItemTag/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemTag itemtag = await db.ItemTags.FindAsync(id);
            if (itemtag == null)
            {
                return HttpNotFound();
            }
            return View(itemtag);
        }

        // POST: /ItemTag/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,Tag")] ItemTag itemtag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(itemtag).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(itemtag);
        }

        // GET: /ItemTag/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ItemTag itemtag = await db.ItemTags.FindAsync(id);
            if (itemtag == null)
            {
                return HttpNotFound();
            }
            return View(itemtag);
        }

        // POST: /ItemTag/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ItemTag itemtag = await db.ItemTags.FindAsync(id);
            db.ItemTags.Remove(itemtag);
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
