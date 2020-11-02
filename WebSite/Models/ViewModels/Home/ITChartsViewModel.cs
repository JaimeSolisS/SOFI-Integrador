using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Models.ViewModels.Home
{
    public class ITChartsViewModel
    {
        public ITChartsViewModel()
        {
            MonthsArray = "";
            YearList = new SelectList(Enumerable.Empty<SelectListItem>());
            TopTicketsList = new SelectList(Enumerable.Empty<SelectListItem>());
            MonthsList = new SelectList(Enumerable.Empty<SelectListItem>());
        }
        public string MonthsArray { get; set; }
        public IEnumerable<SelectListItem> YearList { get; set; }
        public IEnumerable<SelectListItem> TopTicketsList { get; set; }
        public IEnumerable<SelectListItem> MonthsList { get; set; }
    }
}