using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.ActionPlan
{
    public class IndexViewModel
    {
        public IEnumerable<SelectListItem> MachinesList { get; set; }
        public IEnumerable<SelectListItem> ShiftsList { get; set; }
        public IEnumerable<SelectListItem> StatusList { get; set; }
        public IEnumerable<SelectListItem> TypeOfDateList { get; set; }
        public List<OperationTask> OperationTasksList { get; set; }
        public string DateFormat { get; set; }
        public string CultureID { get; set; }


        public IndexViewModel()
        {
            MachinesList = new SelectList(new List<SelectListItem>());
            ShiftsList = new SelectList(new List<SelectListItem>());
            StatusList = new SelectList(new List<SelectListItem>());
            TypeOfDateList = new SelectList(new List<SelectListItem>());

            OperationTasksList = new List<OperationTask>();
        }
    }
}