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
    public class ArticleController : Controller
    {
        private const int PAGE_SIZE = 10;

        private DGuideContext _db = new DGuideContext();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult AutocompleteArticle(string term)
        {
            var articles =
                _db.Articles
                    .Where(a => a.Title.StartsWith(term))
                    .Take(10)
                    .Select(a => new
                    {
                        label = a.Title
                    });

            return Json(articles, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [Analytic]
        // TODO: Need to make it async; couldn't do it because of paging list view
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

            ViewBag.CurrentFilter = searchString;

            PopulateDbVersionsDropDownList();
            PopulateItemTagsDropDownList();

            var articles = from a in _db.Articles
                           select a;

            if (!User.IsInRole(DGuideAuthorize.Administrators))
            {
                articles = articles.Where(a => a.DisplayStatus != DisplayStatus.Hidden);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                articles = articles.Where(a => a.Title.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case SortKey.Date_asc:
                    articles = articles.OrderBy(a => a.TimeStamp);
                    break;
                case SortKey.Date_desc:
                    articles = articles.OrderByDescending(a => a.TimeStamp);
                    break;
                case SortKey.Title_desc:
                    articles = articles.OrderByDescending(a => a.Title);
                    break;
                case SortKey.Title_asc:
                    articles = articles.OrderBy(a => a.Title);
                    break;
                case SortKey.Votes_asc:
                    articles = articles.OrderBy(a => a.Votes);
                    break;
                default:
                    articles = articles.OrderByDescending(a => a.Votes);
                    break;
            }

            int pageNumber = (page ?? 1);

            if(Request.IsAjaxRequest())
            {
                return PartialView("_ArticleList", articles.ToPagedList(pageNumber, PAGE_SIZE));
            }
            return View(articles.ToPagedList(pageNumber, PAGE_SIZE));
        }

        // GET: /Article/Details/5
        [AllowAnonymous]
        [Analytic]
        public async Task<ActionResult> Details(int? id, string searchString)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = await _db.Articles.FindAsync(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: /Article/Create
        [Authorize(Roles = DGuideAuthorize.Administrators)]
        public ActionResult Create()
        {
            PopulateDbVersionsDropDownList();
            return View();
        }

        // POST: /Article/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = DGuideAuthorize.Administrators)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Title,Description,Tags,DisplayStatus,OrgVersionId")] Article article)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    article.Author = User.Identity.Name;
                    article.TimeStamp = DateTime.Now;

                    _db.Articles.Add(article);
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Details", "Article", new { Id = article.Id });
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            PopulateDbVersionsDropDownList(article.DbVersionId);
            return View(article);
        }

        // GET: /Article/Edit/5
        [Authorize(Roles = DGuideAuthorize.Administrators)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = await _db.Articles.FindAsync(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            PopulateDbVersionsDropDownList(article.DbVersionId);
            return View(article);
        }

        // POST: /Article/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = DGuideAuthorize.Administrators)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,Tags,DisplayStatus,Votes,OrgVersionId")] Article article)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //TODO: Implement history; right now it is - you touch it, you own it model
                    article.Author = this.User.Identity.Name;
                    article.TimeStamp = DateTime.Now;
                    _db.Entry(article).State = EntityState.Modified;
                    _db.Entry(article).Property("Votes").EntityEntry.State = EntityState.Unchanged;
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Details", "Article", new { Id = article.Id });
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            PopulateDbVersionsDropDownList(article.DbVersionId);
            return View(article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = DGuideAuthorize.UsersAndAdministrators)]
        public async Task<ActionResult> UpVote(int id)
        {
            Article article = await _db.Articles.FindAsync(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            try
            {
                article.Votes++;
                _db.Entry(article).Property("Votes").EntityEntry.State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View("Details", article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = DGuideAuthorize.UsersAndAdministrators)]
        public async Task<ActionResult> DownVote(int id)
        {
            Article article = await _db.Articles.FindAsync(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            try
            {
                article.Votes--;
                _db.Entry(article).Property("Votes").EntityEntry.State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.)
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View("Details", article);
        }

        private void PopulateItemTagsDropDownList(object selectedItemTag = null)
        {
            ViewBag.ItemTags = _db.ItemTags.ToList();
        }

        private void PopulateDbVersionsDropDownList(object selectedDbVersion = null)
        {
            var dbVersions = from v in _db.DbVersions
                                  orderby v.Id descending
                                  select v;
            ViewBag.DbVersionId = new SelectList(dbVersions, "Id", "Version", selectedDbVersion);
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
