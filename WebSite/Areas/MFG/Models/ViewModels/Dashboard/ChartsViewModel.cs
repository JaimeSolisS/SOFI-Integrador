using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.Dashboard
{
    public class ChartsViewModel
    {
        public IEnumerable<SelectListItem> ShiftList { get; set; }
        public IEnumerable<SelectListItem> MachinesList { get; set; }
        public IEnumerable<SelectListItem> ProcessList { get; set; }
        public IEnumerable<SelectListItem> MaterialsList { get; set; }
        public string DateFormat { get; set; }
        public string HoursArray { get; set; }
        public ChartsViewModel()
        {
            ShiftList = new SelectList(Enumerable.Empty<SelectListItem>());
            MachinesList = new SelectList(Enumerable.Empty<SelectListItem>());
            ProcessList = new SelectList(Enumerable.Empty<SelectListItem>());
            MaterialsList = new SelectList(Enumerable.Empty<SelectListItem>());
            DateFormat = string.Format("{0:yyyy-MM-dd}", DateTime.Now);

        }
    }
}