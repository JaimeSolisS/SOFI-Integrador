using Core.Entities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebSite.Areas.Administration.Models.ViewModels.GenericCharts
{
    public class DataChartsInfoViewModel
    {
        public List<GenericChart> GenericChartsList { get; set; }
        public List<GenericChartsFilters> GenericChartFiltersList { get; set; }
        public List<GenericChartsAxis> GenericChartsAxisList { get; set; }
        //public List<GenericChartFilterData> GenericChartFilterDataServiceList { get; set; }
        public IEnumerable<SelectListItem> GenericChartFilterDataList { get; set; }
        public List<GenericChartData> GenericChartDataList { get; set; }
        public List<GenericChartHeaderData> GenericChartHeaderDataID { get; set; }

        public DataChartsInfoViewModel()
        {
            GenericChartsList = new List<GenericChart>();
            GenericChartFiltersList = new List<GenericChartsFilters>();
            GenericChartsAxisList = new List<GenericChartsAxis>();
            //GenericChartFilterDataServiceList = new List<GenericChartFilterData>();
            GenericChartFilterDataList = new SelectList(new List<SelectListItem>());
            GenericChartDataList = new List<GenericChartData>();
            GenericChartHeaderDataID = new List<GenericChartHeaderData>();
        }

    }
}