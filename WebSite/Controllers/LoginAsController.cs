using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Models.ViewModels.LoginAs;

namespace WebSite.Controllers
{
    public class LoginAsController : BaseController
    {
        // GET: LoginAs
        public ActionResult Index()
        {
            var model = new IndexViewModel()
            {
                Domains = new SelectList(new List<Catalog>())
            };
            //try
            //{
             

            //}
            //catch (Exception ex)
            //{

            //}
            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }

            return View(model);
        }
    }
}