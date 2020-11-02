using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.VATs
{
    public class IndexViewModel
    {
        public IEnumerable<SelectListItem> ShiftsList { get; set; }
        public IEnumerable<SelectListItem> LinesList { get; set; }
        public IEnumerable<SelectListItem> ProductionProcessList { get; set; }
        public IEnumerable<SelectListItem> ProductionLinesList { get; set; }
        public List<ProductionVAT> ProductionVATsList { get; set; }

        public IndexViewModel()
        {
            ShiftsList = new SelectList(new List<SelectListItem>());
            LinesList = new SelectList(new List<SelectListItem>());
            ProductionProcessList = new SelectList(new List<SelectListItem>());
            ProductionLinesList = new SelectList(new List<SelectListItem>());
            ProductionVATsList = new List<ProductionVAT>();
        }
    }
}