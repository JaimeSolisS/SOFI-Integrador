using System.Collections.Generic;
using System.Web.Mvc;
using Core.Entities;

namespace WebSite.Areas.Administration.Models.ViewModels.GenericCharts
{
    public class EditDataChartViewModel
    {
        public int GenericChartID { get; set; }
        public string ChartName { get; set; }
        public string ChartTitle { get; set; }
        public bool Enabled { get; set; }
        public List<GenericChart> GenericChartsList { get; set; }
        public IEnumerable<SelectListItem> ChartAreasList { get; set; }
        public IEnumerable<SelectListItem> ChartTypesList { get; set; }
        public List<GenericChartsFilters> GenericChartsFiltersList { get; set; }
        public List<GenericChartsAxis> GenericChartsAxisList { get; set; }
        public int SelectedAreaID { get; set; }
        public int SelectedChartTypeID { get; set; }
        public string NewEditTitle { get; set; }
        public int GenericChartHeaderDataID { get; set; }


        public EditDataChartViewModel()
        {
            GenericChartID = 0;
            ChartName = "";
            ChartTitle = "";
            Enabled = true;
            GenericChartsList = new List<GenericChart>();
            ChartAreasList = new SelectList(new List<SelectListItem>());
            ChartTypesList = new SelectList(new List<SelectListItem>());
            GenericChartsFiltersList = new List<GenericChartsFilters>();
            GenericChartsAxisList = new List<GenericChartsAxis>();
            SelectedAreaID = 0;
            SelectedChartTypeID = 0;
            GenericChartHeaderDataID = 0;
            NewEditTitle = Resources.GenericCharts.title_NewChart;
        }
    }
}