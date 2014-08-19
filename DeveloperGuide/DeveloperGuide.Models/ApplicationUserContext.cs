using DGuide.Infrastructure.Core;
using DGuide.Infrastructure.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace DGuide.Infrastructure
{
    public class ApplicationUserContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationUserContext()
            : base("ApplicationUserContext")
        {
        }
    }

    public class ApplicationUserInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<ApplicationUserContext>
    //public class ApplicationUserInitializer : System.Data.Entity.DropCreateDatabaseAlways<ApplicationUserContext>
    {
        protected override void Seed(ApplicationUserContext context)
        {
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var rmResult = rm.Create(new IdentityRole(DGuideAuthorize.Administrators));
            rmResult = rm.Create(new IdentityRole(DGuideAuthorize.Users));
            
            var um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            
            var user = new ApplicationUser()
            {
                UserName = "administrator"
            };
            var umResult = um.Create(user, "Administrator");
            
            if (umResult.Succeeded && rmResult.Succeeded)
            {
                um.AddToRole(user.Id, DGuideAuthorize.Administrators);
            }

            user = new ApplicationUser()
            {
                UserName = "rbobby"
            };
            umResult = um.Create(user, "RickyBobby");

            if (umResult.Succeeded && rmResult.Succeeded)
            {
                um.AddToRole(user.Id, DGuideAuthorize.Users);
            }

            user = new ApplicationUser()
            {
                UserName = "mbolton"
            };
            umResult = um.Create(user, "MichaelBolton");

            if (umResult.Succeeded && rmResult.Succeeded)
            {
                um.AddToRole(user.Id, DGuideAuthorize.Users);
            }

            base.Seed(context);
        }
    }
}