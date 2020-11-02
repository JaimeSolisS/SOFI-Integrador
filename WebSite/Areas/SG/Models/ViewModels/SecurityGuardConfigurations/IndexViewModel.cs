using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.SG.Models.ViewModels.SecurityGuardConfigurations
{
    public class IndexViewModel
    {
        public IEnumerable<SelectListItem> CompaniesSelectList;
        public List<Vendor> CompaniesList;

        public IndexViewModel()
        {
            CompaniesSelectList = new SelectList(new List<SelectListItem>());
            CompaniesList = new List<Vendor>();
        }
    }
}