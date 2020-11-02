using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.Dashboard
{
    public class HeaderViewModel
    {
        public string PlantName { get; set; }
        public string JulianDay { get; set; }
        public string Shift { get; set; }
        public IEnumerable<SelectListItem> ShiftList { get; set; }
        public string DateFormat { get; set; }
        public HeaderViewModel()
        {
            ShiftList = new SelectList(new List<SelectListItem>());
            DateFormat = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
        }
    }
}