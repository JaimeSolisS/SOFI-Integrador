using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.CI.Models.ViewModels.Administration
{
    public class IndexViewModel
    {
        public IEnumerable<SelectListItem> ListAreas;
        public List<DashboardArea> DashboardAreaList;
        public IEnumerable<SelectListItem> FileTypeList;
        public IEnumerable<SelectListItem> ViewsList;
        public int DFT_View;
        public IndexViewModel()
        {
            ListAreas = new SelectList(new List<DashboardArea>());
            FileTypeList = new SelectList(new List<Catalog>());
            DashboardAreaList = new List<DashboardArea>();
            ViewsList = new SelectList(new List<Catalog>());
        }
    }
}