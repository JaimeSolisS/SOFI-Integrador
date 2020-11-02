using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.LGX.Controllers
{
    public class DashboardController : Controller
    {
        // GET: LGX/Dashboard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LogisticLinks()
        {
            return View();
        }
    }
}