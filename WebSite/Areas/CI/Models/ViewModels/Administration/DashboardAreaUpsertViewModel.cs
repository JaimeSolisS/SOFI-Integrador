using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.CI.Models.ViewModels.Administration
{
    public class DashboardAreaUpsertViewModel
    {
        public int Sequence;
        public List<Catalog> CulturesList;
        public IEnumerable<SelectListItem> SizeList;

        public DashboardAreaUpsertViewModel()
        {
            SizeList = new SelectList(new List<Catalog>());
            CulturesList = new List<Catalog>();
        }

    }
}