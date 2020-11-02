using Core.Entities;
using System.Collections.Generic;

namespace WebSite.Areas.Administration.Models.ViewModels.GenericCharts
{
    public class DataChartFiltersViewModel
    {
        public List<GenericChartsFilters> FilterList { get; set; }
        public List<CatalogDetail> ValuesOfFilters { get; set; }
        public string ChartType { get; set; }
        public string ChartName { get; set; }
        public string ChartTitle { get; set; }

        public DataChartFiltersViewModel()
        {
            FilterList = new List<GenericChartsFilters>();
            ValuesOfFilters = new List<CatalogDetail>();
            ChartType = "";
            ChartName = "";
            ChartTitle = "";
        }
    }
}