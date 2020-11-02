using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.SG.Models.ViewModels.SecurityGuardConfigurations
{
    public class BadgesViewModel
    {
        public List<Badge> BadgesList;
        public IEnumerable<SelectListItem> BadgesTypesList;


        public BadgesViewModel()
        {
            BadgesList = new List<Badge>();
            BadgesTypesList = new SelectList(new List<SelectListItem>());
        }
    }
}
