using System.Web.Mvc;

namespace WebSite.Areas.SG
{
    public class SGAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SG";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SG_default",
                "SG/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}