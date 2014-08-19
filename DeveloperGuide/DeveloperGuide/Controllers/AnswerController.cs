using DGuide.Infrastructure;
using DGuide.Infrastructure.Core;
using DGuide.Infrastructure.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DGuide.Controllers
{
    [Authorize(Roles = DGuideAuthorize.UsersAndAdministrators)]
    public class AnswerController : Controller
    {
        private DGuideContext _db = new DGuideContext();

        // GET: /Answer/Create
        public ActionResult Create(int? questionId)
        {
            return View(new Answer { QuestionId = (questionId ?? 0) });
        }

        // POST: /Answer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] // TODO: Replace with View Model AllowHtml property
        public async Task<ActionResult> Create([Bind(Include = "ContentFormat,Content,QuestionId")] Answer answer)
        {
            try
            {
                answer.Author = this.User.Identity.Name;
                answer.TimeStamp = DateTime.Now;

                if (ModelState.IsValid)
                {
                    _db.Answers.Add(answer);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Details", "Question", new { id = answer.QuestionId });
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View(answer);
        }

        // GET: /Answer/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = await _db.Answers.FindAsync(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // POST: /Answer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] // TODO: Replace with View Model AllowHtml property
        public async Task<ActionResult> Edit([Bind(Include = "Id,ContentFormat,Content,QuestionId")] Answer answer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //TODO: Implement history; right now it is - you touch it, you own it model
                    answer.Author = this.User.Identity.Name;
                    answer.TimeStamp = DateTime.Now;

                    _db.Entry(answer).State = EntityState.Modified;
                    _db.Entry(answer).Property("Votes").IsModified = false;
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Details", "Question", new { Id = answer.QuestionId });
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(answer);
        }

        // GET: /Answer/Delete/5
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

            Answer answer = await _db.Answers.FindAsync(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }

        // GET: /Answer/Delete/5
        //[Authorize(Roles = DGuideAuthorize.Administrators)]
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Answer answer = await _db.Answers.FindAsync(id);
        //    if (answer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(answer);
        //}

        // POST: /Answer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = DGuideAuthorize.Administrators)]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Answer answer = await _db.Answers.FindAsync(id);
            _db.Answers.Remove(answer);
            await _db.SaveChangesAsync();
            return RedirectToAction("Details", "Question", new { Id = answer.QuestionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpVote(int id)
        {
            Answer answer = await _db.Answers.FindAsync(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            try
            {
                answer.Votes++;
                _db.Entry(answer).Property("Votes").EntityEntry.State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            if(Request.IsAjaxRequest())
            {
                return PartialView("_Answer", answer);
            }
            return RedirectToAction("Details", "Question", new { Id = answer.QuestionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DownVote(int id)
        {
            Answer answer = await _db.Answers.FindAsync(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            try
            {
                answer.Votes--;
                _db.Entry(answer).Property("Votes").EntityEntry.State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Answer", answer);
            }
            return RedirectToAction("Details", "Question", new { Id = answer.QuestionId });
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
