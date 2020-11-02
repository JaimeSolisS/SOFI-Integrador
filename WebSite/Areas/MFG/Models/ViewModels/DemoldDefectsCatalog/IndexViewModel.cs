using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.DemoldDefectsCatalog
{
    public class IndexViewModel
    {
        public IEnumerable<SelectListItem> ShiftsList { get; set; }
        public IEnumerable<SelectListItem> ProductionProcessList { get; set; }
        public IEnumerable<SelectListItem> LinesList { get; set; }
        public IEnumerable<SelectListItem> VATList { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<DemoldDefectDetail> DemoldDefectDetailList { get; set; }
        public IEnumerable<SelectListItem> DefectsList { get; set; }
        public IEnumerable<SelectListItem> DefectsTypesList { get; set; }
        public IEnumerable<SelectListItem> DesignList { get; set; }
        public IndexViewModel()
        {
            ShiftsList = new SelectList(new List<SelectListItem>());
            LinesList = new SelectList(new List<SelectListItem>());
            ProductionProcessList = new SelectList(new List<SelectListItem>());
            VATList = new SelectList(new List<SelectListItem>());
            DemoldDefectDetailList = new List<DemoldDefectDetail>();
            DefectsList = new SelectList(new List<SelectListItem>());
            DefectsTypesList = new SelectList(new List<SelectListItem>());
            DesignList= new SelectList(new List<SelectListItem>());
        }
    }
}