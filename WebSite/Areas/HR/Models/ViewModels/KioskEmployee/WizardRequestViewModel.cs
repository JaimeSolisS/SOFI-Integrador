using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.HR.Models.ViewModels.KioskEmployee
{
    public class WizardRequestViewModel
    {
        public IEnumerable<SelectListItem> DepartmentsList { get; set; }
        public IEnumerable<SelectListItem> ShiftsList { get; set; }
        public List<Catalog> RequestTypesList { get; set; }


        public WizardRequestViewModel()
        {
            DepartmentsList = new SelectList(new List<SelectListItem>());
            ShiftsList = new SelectList(new List<SelectListItem>());
            RequestTypesList = new List<Catalog>();
        }

    }
}