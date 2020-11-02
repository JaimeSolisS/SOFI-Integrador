using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.SG.Models.ViewModels.SecurityGuard
{
    public class SuppliersViewModel
    {
        public IEnumerable<SelectListItem> VendorsList;
        public List<VendorUser> VendorUserList;

        public SuppliersViewModel()
        {
            VendorsList = new SelectList(new List<SelectListItem>());
            VendorUserList = new List<VendorUser>();
        }
    }
}