using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Crims.UI.Web.Startup))]
namespace Crims.UI.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
