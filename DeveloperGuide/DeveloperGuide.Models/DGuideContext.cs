using DGuide.Infrastructure.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace DGuide.Infrastructure
{
    public class DGuideContext : IdentityDbContext<ApplicationUser>
    {
        public DGuideContext()
            : base("name=DGuideContext")
        {}

        public DbSet<Analytic> Analytics { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Article> Articles { get; set; } // does select * from Articles from sql server
        public DbSet<DbDocument> DbDocuments { get; set; }
        public DbSet<DbVersion> DbVersions { get; set; }
        public DbSet<ItemTag> ItemTags { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Question> Questions { get; set; }
    }
}
