using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.Administration.Models.ViewModels.Profiles
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            Profiles = new List<Profile>();
            ProfileList = new SelectList(new List<Profile>());

        }

        public List<Profile> Profiles { get; set; }
        public IEnumerable<SelectListItem> ProfileList;
    }
}