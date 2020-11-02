using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.DemoldDefects
{
    public class IndexViewModel
    {
        public string DayCode { get; set; }
        public IEnumerable<SelectListItem> ShiftsList { get; set; }
        public IEnumerable<SelectListItem> ProductionProcessList { get; set; }
        public IEnumerable<SelectListItem> LinesList { get; set; }
        public IEnumerable<SelectListItem> VATList { get; set; }
        public int CurrentShiftID { get; set; }

        public int? ProductionProcessID { get; set; }
        public int? ProductionLineID { get; set; }
        public int? VATID { get; set; }
        public string InspectorName { get; set; }

        public IndexViewModel()
        {
            DayCode = "";
            ShiftsList = new SelectList(new List<SelectListItem>());
            ProductionProcessList = new SelectList(new List<SelectListItem>());
            LinesList = new SelectList(new List<SelectListItem>());
            VATList = new SelectList(new List<SelectListItem>());
            CurrentShiftID = 0;

            ProductionProcessID = 0;
            ProductionLineID = 0;
            VATID = 0;
            InspectorName = "";
        }
    }
}