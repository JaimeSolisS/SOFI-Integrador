using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.OperationRecords
{
    public class NewViewModel
    {
        public IEnumerable<SelectListItem> MachinesList { get; set; }
        public IEnumerable<SelectListItem> ShiftList { get; set; }
        public IEnumerable<SelectListItem> SetupsList { get; set; }
        public string DateFormat;
        public IEnumerable<SelectListItem> MaterialsList { get; set; }
        public NewViewModel()
        {
            MachinesList = new SelectList(new List<SelectListItem>());
            ShiftList = new SelectList(new List<SelectListItem>());
            SetupsList = new SelectList(new List<SelectListItem>());
            MaterialsList = new SelectList(new List<Catalog>());
        }
    }
}