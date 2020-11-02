using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Models.ViewModels.Home
{
    public class ChartsViewModel
    {
        public ChartsViewModel()
        {
            DefectsList = new SelectList(Enumerable.Empty<SelectListItem>());
            TopDefectList = new SelectList(Enumerable.Empty<SelectListItem>());
            VAList = new SelectList(Enumerable.Empty<SelectListItem>());
            DesignList = new SelectList(Enumerable.Empty<SelectListItem>());
            LinesList = new SelectList(Enumerable.Empty<SelectListItem>());
            ShiftList = new SelectList(Enumerable.Empty<SelectListItem>());
        }
        public string ProcessName { get; set; }
        public bool UseLines { get; set; }
        public string HoursArray { get; set; }
        public IEnumerable<SelectListItem> DefectsList { get; set; }
        public IEnumerable<SelectListItem> TopDefectList { get; set; }
        public IEnumerable<SelectListItem> LinesList { get; set; }
        public IEnumerable<SelectListItem> VAList { get; set; }
        public IEnumerable<SelectListItem> DesignList { get; set; }
        public IEnumerable<SelectListItem> ShiftList { get; set; }
        public List<int> SelectedDefects { get; set; }
        public string ProcessGroup { get; set; }
        public int ProductionProcessID { get; set; }
        public string DashboardSelectedDefects { get; set; }
    }
}