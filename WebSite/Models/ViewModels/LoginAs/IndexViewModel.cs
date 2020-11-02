using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Models.ViewModels.LoginAs
{
    public class IndexViewModel
    {
        public IEnumerable<SelectListItem> Domains { get; set; }

        public IndexViewModel()
        {

        }
    }
}