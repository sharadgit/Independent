using DGuide.Infrastructure;
using DGuide.Infrastructure.Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace DGuide.Filters
{
    public class AnalyticAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;

            Analytic analytic = new Analytic 
            { 
                UserName = (request.IsAuthenticated) ? filterContext.HttpContext.User.Identity.Name : "Anonymous",
                IPAddress = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress,
                AreaAccessed = request.RawUrl,
                Timestamp = DateTime.UtcNow
            };

            // Welcome to the dark side!!!
            SaveAnalyticAttributeAsync(analytic);

            base.OnActionExecuting(filterContext);
        }

        private async Task SaveAnalyticAttributeAsync(Analytic analytic)
        {
            using (DGuideContext context = new DGuideContext())
            {
                context.Analytics.Add(analytic);
                await context.SaveChangesAsync();
            }
        }
    }
}