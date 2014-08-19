using DGuide.Filters;
using DGuide.Infrastructure;
using DGuide.Infrastructure.Core;
using DGuide.Infrastructure.Models;
using PagedList;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DGuide.Controllers
{
    [Authorize(Roles = DGuideAuthorize.UsersAndAdministrators)]
    public class QuestionController : Controller
    {
        private const int PAGE_SIZE = 10;

        private DGuideContext _db = new DGuideContext();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult AutocompleteQuestion(string term)
        {
            var questions =
                _db.Questions
                    .Where(a => a.Header.StartsWith(term))
                    .Take(10)
                    .Select(a => new
                    {
                        label = a.Header
                    });

            return Json(questions, JsonRequestBehavior.AllowGet);
        }

        // GET: /Question/
        [AllowAnonymous]
        [Analytic]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.VotesSortParm = String.IsNullOrEmpty(sortOrder) ? SortKey.Votes_desc : String.Empty;
            ViewBag.DateSortParm = sortOrder == SortKey.Date_desc ? SortKey.Date_asc : SortKey.Date_desc;
            ViewBag.TitleSortParm = sortOrder == SortKey.Title_asc ? SortKey.Title_desc : SortKey.Title_asc;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            PopulateItemTagsDropDownList();

            ViewBag.CurrentFilter = searchString;

            var questions = from q in _db.Questions
                           select q;

            if (!String.IsNullOrEmpty(searchString))
            {
                questions = questions.Where(q => q.Header.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case SortKey.Date_asc:
                    questions = questions.OrderBy(q => q.TimeStamp);
                    break;
                case SortKey.Date_desc:
                    questions = questions.OrderByDescending(q => q.TimeStamp);
                    break;
                case SortKey.Title_desc:
                    questions = questions.OrderByDescending(q => q.Header);
                    break;
                case SortKey.Title_asc:
                    questions = questions.OrderBy(q => q.Header);
                    break;
                case SortKey.Votes_asc:
                    questions = questions.OrderBy(q => q.Votes);
                    break;
                default:
                    questions = questions.OrderByDescending(q => q.Votes);
                    break;
            }

            int pageNumber = (page ?? 1);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_QuestionList", questions.ToPagedList(pageNumber, PAGE_SIZE));
            }
            return View(questions.ToPagedList(pageNumber, PAGE_SIZE));
        }

        // GET: /Question/Details/5
        [AllowAnonymous]
        [Analytic]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = await _db.Questions.FindAsync(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // GET: /Question/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Question/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] // TODO: Replace with View Model AllowHtml property
        public async Task<ActionResult> Create([Bind(Include = "Header,ContentFormat,Content,Tags,DisplayStatus")] Question question)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    question.Author = this.User.Identity.Name;
                    question.TimeStamp = DateTime.Now;

                    _db.Questions.Add(question);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(question);
        }

        // GET: /Question/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = await _db.Questions.FindAsync(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: /Question/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)] // TODO: Replace with View Model AllowHtml property
        public async Task<ActionResult> Edit([Bind(Include = "Id,Header,ContentFormat,Content,Tags,DisplayStatus")] Question question)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //TODO: Implement history; right now it is - you touch it, you own it model
                    question.Author = this.User.Identity.Name;
                    question.TimeStamp = DateTime.Now;

                    _db.Entry(question).State = EntityState.Modified;
                    _db.Entry(question).Property("Votes").IsModified = false;
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Details", new { id = question.Id });
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpVote(int id)
        {
            Question question = await _db.Questions.FindAsync(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            try
            {
                question.Votes++;
                _db.Entry(question).Property("Votes").EntityEntry.State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View("Details", question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DownVote(int id)
        {
            Question question = await _db.Questions.FindAsync(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            try
            {
                question.Votes--;
                _db.Entry(question).Property("Votes").EntityEntry.State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View("Details", question);
        }

        private void PopulateItemTagsDropDownList(object selectedItemTag = null)
        {
            ViewBag.ItemTags = _db.ItemTags.ToList();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        internal class SortKey
        {
            public const string Title_asc = "Title_asc";
            public const string Title_desc = "Title_desc";
            public const string Date_asc = "Date_asc";
            public const string Date_desc = "Date_desc";
            public const string Votes_asc = "Votes_asc";
            public const string Votes_desc = "Votes_desc";
        }
    }
}
