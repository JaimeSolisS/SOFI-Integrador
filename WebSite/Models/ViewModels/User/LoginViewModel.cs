using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Models.ViewModels.User
{
    public class LoginViewModel
    {
        public IEnumerable<SelectListItem> Domains { get; set; }

        public LoginViewModel()
        {

        }
    }
}