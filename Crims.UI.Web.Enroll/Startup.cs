using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Crims.Data;
using Microsoft.Owin;
using Owin;

//[assembly: OwinStartup(typeof(Crims.UI.Web.Enroll.Startup))]
namespace Crims.UI.Web.Enroll
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
