using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.Administration.Models.ViewModels.Profiles
{
    public class EditViewModel
    {
        public EditViewModel() {
            EditProfile = new Profile();
            OrganizationsList = new SelectList(Enumerable.Empty<SelectListItem>());
        }

        public Profile EditProfile { get; set; }
        public IEnumerable<SelectListItem> OrganizationsList { get; set; }
    }
}