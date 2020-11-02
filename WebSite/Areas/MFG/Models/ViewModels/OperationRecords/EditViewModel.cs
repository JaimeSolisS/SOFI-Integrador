using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.OperationRecords
{
    public class EditViewModel
    {
        public OperationRecord Operation { get; set; }
        //public OperationSetup Setup { get; set; }
        public TabSetupViewModel TabSetup { get; set; }
        public OperationProduction Production { get; set; }
        public List<DownTime> DownTimesList { get; set; }
        public IEnumerable<SelectListItem> DepartmentsList { get; set; }
        public IEnumerable<SelectListItem> ReasonsList { get; set; }
        public IEnumerable<SelectListItem> RejectsTypesList { get; set; }
        public IEnumerable<SelectListItem> HoursList { get; set; }
        public string OperatorNumber { get; set; }
        public string DateFormat;
        public int CavitiesNumber;
        public string ProductionProcess { get; set; }
        public EditViewModel()
        {
            Operation = new OperationRecord();
            // Setup = new OperationSetup();
            TabSetup = new TabSetupViewModel();
            Production = new OperationProduction();
            DownTimesList = new List<DownTime>();
            DepartmentsList = new SelectList(new List<SelectListItem>());
            ReasonsList = new SelectList(new List<SelectListItem>());
            RejectsTypesList = new SelectList(new List<SelectListItem>());
            HoursList = new SelectList(new List<SelectListItem>());
            DateFormat = string.Format("{0:HH:mm}", DateTime.Now);
        }
    }
}