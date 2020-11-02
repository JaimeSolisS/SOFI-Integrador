using Core.Entities;
using Core.Entities.MFG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.DemoldDefectsCatalog
{
    public class AlertsViewModel
    {
        public IEnumerable<SelectListItem> ShiftsList { get; set; }
        public IEnumerable<SelectListItem> DefectsCategoriesList { get; set; }
        public IEnumerable<SelectListItem> LinesList { get; set; }
        public IEnumerable<SelectListItem> FamiliesList { get; set; }
        public List<DemoldDefectAlert> AlertsList { get; set; }

        public IEnumerable<SelectListItem> LensTypesList { get; set; }
        public AlertsViewModel()
        {
            ShiftsList = new SelectList(new List<SelectListItem>());
            LinesList = new SelectList(new List<SelectListItem>());
            DefectsCategoriesList = new SelectList(new List<SelectListItem>());
            FamiliesList = new SelectList(new List<SelectListItem>());
            AlertsList = new List<DemoldDefectAlert>();
            LensTypesList = new SelectList(new List<SelectListItem>());
        }
    }
}