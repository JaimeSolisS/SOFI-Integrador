using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.Dashboard
{
    public class ModalExportToExcelViewModel
    {
        public IEnumerable<SelectListItem> MachinesList { get; set; }
        public IEnumerable<SelectListItem> ProcessList { get; set; }
        public IEnumerable<SelectListItem> MaterialsList { get; set; }
        public string ActionName { get; set; }
        public bool IsProduction { get; set; }
        public string WidthClass { get; set; }
        public int ShiftID { get; set; }

        public ModalExportToExcelViewModel()
        {
            MachinesList = new SelectList(new List<SelectListItem>());
            ProcessList = new SelectList(new List<SelectListItem>());
            MaterialsList = new SelectList(new List<SelectListItem>());
            ActionName = "";
            IsProduction = false;
            WidthClass = "col-sm-12";
            ShiftID = 0;
        }
    }
}