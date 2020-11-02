using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.DemoldDefectsCharts
{
    public class IndexViewModel
    {
        public IEnumerable<SelectListItem> DemoldDefectsCategoriesList { get; set; }
        public IEnumerable<SelectListItem> ProductionLinesList { get; set; }
        public IEnumerable<SelectListItem> FamiliesList { get; set; }
        public IEnumerable<SelectListItem> ShiftsList { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<Catalog> DemoldCharts { get; set; }
        public IEnumerable<SelectListItem> DesignList { get; set; }

        public IndexViewModel()
        {
            DemoldDefectsCategoriesList = new SelectList(new List<SelectListItem>());
            ProductionLinesList = new SelectList(new List<SelectListItem>());
            FamiliesList = new SelectList(new List<SelectListItem>());
            ShiftsList = new SelectList(new List<SelectListItem>());
            DemoldCharts = new List<Catalog>();
            DesignList = new SelectList(new List<SelectListItem>());
        }
    }
}