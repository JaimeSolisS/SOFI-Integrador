using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.Dashboard
{
    public class ExportToExcelViewModel
    {
        public IEnumerable<SelectListItem> ShiftsList { get; set; }
        public IEnumerable<SelectListItem> MachinesList { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string CultureID { get; set; }

        public ExportToExcelViewModel()
        {
            ShiftsList = new SelectList(new List<SelectListItem>());
            MachinesList = new SelectList(new List<SelectListItem>());
            StartDate = new DateTime();
            EndDate = new DateTime();
            StartTime = new DateTime();
            EndTime = new DateTime();
            CultureID = "";
        }
    }
}