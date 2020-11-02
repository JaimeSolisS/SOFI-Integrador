using Core.Entities;
using System.Collections.Generic;

namespace WebSite.Areas.Administration.Models.ViewModels.GenericCharts
{
    public class DataChartFieldsViewModel
    {
        public List<GenericChartsAxis> DataFieldsList { get; set; }
        public List<GenericChartData> GenericChartDataList { get; set; }
        public bool ContainsData { get; set; }
        public int GenericChartHeaderDataID { get; set; }


        public DataChartFieldsViewModel()
        {
            DataFieldsList = new List<GenericChartsAxis>();
            GenericChartDataList = new List<GenericChartData>();
            ContainsData = false;
            GenericChartHeaderDataID = 0;
        }
    }
}