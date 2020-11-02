using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.VATs
{
    public class EditViewModel
    {
        public string Title { get; set; }
        public bool IsEdit { get; set; }
        public IEnumerable<SelectListItem> ShiftsList { get; set; }
        public IEnumerable<SelectListItem> ProductionLinesList { get; set; }
        public ProductionVAT ProductionVATObj { get; set; }

        public EditViewModel()
        {
            Title = "";
            IsEdit = false;
            ShiftsList = new SelectList(new List<SelectListItem>());
            ProductionLinesList = new SelectList(new List<SelectListItem>());
            ProductionVATObj = new ProductionVAT();
        }
    }
}