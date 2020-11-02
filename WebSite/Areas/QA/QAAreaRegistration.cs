using System.Web.Mvc;

namespace WebSite.Areas.QA
{
    public class QAAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "QA";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "QA_default",
                "QA/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}