using System;
using System.Configuration;
using System.Data.Entity;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Crims.Data;

namespace Crims.UI.Web.Enroll
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutofacConfig.ConfigureContainer();
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<CrimsDbContext, Data.Migrations.Configuration>("crimsDbEntities"));

        }
    }
}
