using System.Web.Mvc;

namespace WebSite.Areas.MNT
{
    public class MNTAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MNT";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "MNT_default",
                "MNT/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}