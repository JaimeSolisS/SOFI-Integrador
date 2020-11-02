using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.Reports
{
    public class ACTParamsViewModel
    {
        public IEnumerable<SelectListItem> MachinesList { get; set; }
        public IEnumerable<SelectListItem> SetupsList { get; set; }
        public IEnumerable<SelectListItem> MaterialsList { get; set; }
        public IEnumerable<SelectListItem> ProcessList { get; set; }
        public IEnumerable<SelectListItem> ShiftsList { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public IEnumerable<SelectListItem> ResponsiblesList { get; set; }
        public IEnumerable<SelectListItem> TypeOfDateList { get; set; }
        public ACTParamsViewModel()
        {
            MachinesList = new SelectList(new List<SelectListItem>());
            ShiftsList = new SelectList(new List<SelectListItem>());
            StatusList = new SelectList(new List<SelectListItem>());
            TypeOfDateList = new SelectList(new List<SelectListItem>());
            ResponsiblesList = new SelectList(new List<SelectListItem>());
        }
    }
}