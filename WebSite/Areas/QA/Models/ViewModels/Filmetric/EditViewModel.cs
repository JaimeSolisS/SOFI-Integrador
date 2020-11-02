using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.QA.Models.ViewModels.Filmetric
{
    public class EditViewModel
    {
        public OperationRecord Operation { get; set; }
        public OperationSetup Setup { get; set; }
        public OperationProduction Production { get; set; }
        public List<DownTime> DownTimesList { get; set; }
        public IEnumerable<SelectListItem> DepartmentsList { get; set; }
        public IEnumerable<SelectListItem> ReasonsList { get; set; }
        public IEnumerable<SelectListItem> DefectsList { get; set; }
        public IEnumerable<SelectListItem> RejectsList { get; set; }
        public string OperatorNumber { get; set; }
        public string DateFormat;
        public int CavitiesNumber;
        public EditViewModel()
        {
            Operation = new OperationRecord();
            Setup = new OperationSetup();
            Production = new OperationProduction();
            DownTimesList = new List<DownTime>();
            DepartmentsList = new SelectList(new List<SelectListItem>());
            ReasonsList = new SelectList(new List<SelectListItem>());
            DefectsList = new SelectList(new List<SelectListItem>());
            RejectsList = new SelectList(new List<SelectListItem>());
            DateFormat = string.Format("{0:HH:mm}", DateTime.Now);
        }
    }
}