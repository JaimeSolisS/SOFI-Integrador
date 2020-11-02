using Core.Entities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebSite.Areas.Administration.Models.ViewModels.GenericCharts
{
    public class DataChartAdminViewModel
    {
        public IEnumerable<SelectListItem> ChartAreasList { get; set; }
        public IEnumerable<SelectListItem> ChartsOfAreaList { get; set; }
        public List<GenericChart> GenericChartsList { get; set; }

        public DataChartAdminViewModel()
        {
            ChartAreasList = new SelectList(new List<SelectListItem>());
            ChartsOfAreaList = new SelectList(new List<SelectListItem>());
            GenericChartsList = new List<GenericChart>();
        }
    }
}