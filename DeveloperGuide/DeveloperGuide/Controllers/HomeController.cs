using DGuide.Infrastructure;
using DGuide.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DGuide.Controllers
{
    public class HomeController : Controller
    {
        private DGuideContext db = new DGuideContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
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