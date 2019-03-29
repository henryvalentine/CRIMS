using System.Web.Mvc;

namespace Crims.UI.Web.Areas.Enroll
{
    public class EnrollAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Enroll";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Enroll_default",
                "Enroll/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}