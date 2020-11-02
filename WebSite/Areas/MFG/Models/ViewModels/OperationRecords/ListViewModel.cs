using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.OperationRecords
{
    public class ListViewModel
    {
        public IEnumerable<SelectListItem> MachinesList { get; set; }
        public IEnumerable<SelectListItem> ShiftList { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public List<OperationRecord> Operations { get; set; }
        public string DateFormat { get; set; }
        public string JulianDay { get; set; }
        public string AllowFilters { get; set; }

        public string Shift { get; set; }

        public ListViewModel()
        {
            MachinesList = new SelectList(new List<SelectListItem>());
            ShiftList = new SelectList(new List<SelectListItem>());
            Operations = new List<OperationRecord>();
            StatusList = new SelectList(new List<SelectListItem>());
            AllowFilters = "";
        }
    }
}