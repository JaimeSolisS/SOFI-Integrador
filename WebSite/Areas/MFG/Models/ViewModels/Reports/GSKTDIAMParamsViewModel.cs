using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.Reports
{
    public class GSKTDIAMParamsViewModel
    {
        public IEnumerable<SelectListItem> GasketsList { get; set; }
        public IEnumerable<SelectListItem> MachinesList { get; set; }
        public IEnumerable<SelectListItem> CavitiesList { get; set; }
        public IEnumerable<SelectListItem> ProcessList { get; set; }
        public IEnumerable<SelectListItem> ShiftsList { get; set; }
        public IEnumerable<SelectListItem> MaterialsList { get; set; }
        public GSKTDIAMParamsViewModel()
        {
            GasketsList = new SelectList(new List<SelectListItem>());
            MachinesList = new SelectList(new List<SelectListItem>());
            CavitiesList = new SelectList(new List<SelectListItem>());
            ProcessList = new SelectList(new List<SelectListItem>());
            ShiftsList = new SelectList(new List<SelectListItem>());
            MaterialsList = new SelectList(new List<SelectListItem>());
        }
    }
}