using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DGuide.Startup))]
namespace DGuide
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
