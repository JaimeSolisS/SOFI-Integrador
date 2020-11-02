using System.Web.Mvc;

namespace WebSite.Areas.LGX
{
    public class LGXAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "LGX";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "LGX_default",
                "LGX/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}