using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.Production.Controllers
{
    public class ChartsController : Controller
    {
        // GET: Production/Charts
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult Slider()
        {

            return View();
        }
    }
}