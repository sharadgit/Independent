using DGuide.Infrastructure;
using DGuide.Infrastructure.Core;
using DGuide.Infrastructure.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DGuide.Controllers
{
    [Authorize(Roles=DGuideAuthorize.Administrators)]
    public class SectionController : Controller
    {
        private DGuideContext _db = new DGuideContext();


        [HttpGet]
        [AllowAnonymous]
        public async Task<FileContentResult> LoadDocument(int dbDocumentId)
        {
            DbDocument document = await _db.DbDocuments.FirstOrDefaultAsync(p => p.Id == dbDocumentId);
            if (document != null)
            {
                return File(document.FileContent, document.Type);
            }
            else
            {
                return null;
            }
        }

        // GET: /Section/Create
        public ActionResult Create(int? articleId)
        {
            return View(new Section { ArticleId = (articleId ?? 0) });
        }

        // POST: /Section/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] // TODO: Replace with View Model AllowHtml property
        public async Task<ActionResult> Create([Bind(Include = "Sequence,Header,ContentFormat,Content,ArticleId")] Section section,
            HttpPostedFileBase uploadedDocument)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (uploadedDocument != null)
                    {
                        DbDocument document = new DbDocument
                        {
                            Name = uploadedDocument.FileName,
                            Type = uploadedDocument.ContentType,
                            Size = uploadedDocument.ContentLength,
                            CreatedOn = DateTime.Now
                        };
                        document.FileContent = new byte[uploadedDocument.ContentLength];
                        uploadedDocument.InputStream.Read(document.FileContent, 0, uploadedDocument.ContentLength);

                        _db.DbDocuments.Add(document);
                        await _db.SaveChangesAsync();

                        section.DbDocumentId = document.Id;
                    }
                    _db.Sections.Add(section);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Details", "Article", new { id = section.ArticleId });
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View(section);
        }

        // GET: /Section/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Section section = await _db.Sections.FindAsync(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // POST: /Section/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] // TODO: Replace with View Model AllowHtml property
        public async Task<ActionResult> Edit([Bind(Include = "Id,Sequence,Header,ContentFormat,Content,DbDocumentId,ArticleId")] Section section,
            HttpPostedFileBase uploadedDocument)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (uploadedDocument != null)
                    {
                        DbDocument document = new DbDocument
                        {
                            Name = uploadedDocument.FileName,
                            Type = uploadedDocument.ContentType,
                            Size = uploadedDocument.ContentLength,
                            CreatedOn = DateTime.Now
                        };
                        document.FileContent = new byte[uploadedDocument.ContentLength];
                        uploadedDocument.InputStream.Read(document.FileContent, 0, uploadedDocument.ContentLength);

                        _db.DbDocuments.Add(document);
                        await _db.SaveChangesAsync();

                        section.DbDocumentId = document.Id;
                    }
                    
                    if (section.Id > 0)
                    {
                        _db.Entry(section).State = EntityState.Modified;
                    }
                    else
                    {
                        _db.Sections.Add(section);
                    }
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Details", "Article", new { Id = section.ArticleId });
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(section);
        }

        // GET: /Section/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }

            Section section = await _db.Sections.FindAsync(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // GET: /Section/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Section section = await _db.Sections.FindAsync(id);
        //    if (section == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(section);
        //}

        // POST: /Section/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Section section = await _db.Sections.FindAsync(id);
            _db.Sections.Remove(section);
            await _db.SaveChangesAsync();
            return RedirectToAction("Details", "Article", new { Id = section.ArticleId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
