using System.Web.Mvc;

namespace WebSite.Areas.WHS
{
    public class WHSAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "WHS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "WHS_default",
                "WHS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}