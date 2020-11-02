using System.Web.Mvc;

namespace WebSite.Areas.PUR
{
    public class PURAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PUR";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PUR_default",
                "PUR/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}