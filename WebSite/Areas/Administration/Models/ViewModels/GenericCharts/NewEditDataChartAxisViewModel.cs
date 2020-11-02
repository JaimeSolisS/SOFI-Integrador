using System.Collections.Generic;
using System.Web.Mvc;
using Core.Entities;

namespace WebSite.Areas.Administration.Models.ViewModels.GenericCharts
{
    public class NewEditDataChartAxisViewModel
    {
        public int? GenericChartAxisID { get; set; }
        public string AxisName { get; set; }
        public IEnumerable<SelectListItem> AxisCategories_List { get; set; }
        public IEnumerable<SelectListItem> DataChartsTypes_List { get; set; }
        public IEnumerable<SelectListItem> DataTypes_List { get; set; }
        public List<Catalog> DataFormat_List { get; set; }
        public string Color { get; set; }
        public string Prefix { get; set; }
        public string Sufix { get; set; }
        public bool IsEdit { get; set; }
        public string NewEditTitle { get; set; }
        public bool ShowLine { get; set; }
        public int Rotation { get; set; }
        public bool ShowLabel { get; set; }
        public int FontSize { get; set; }
        public string FontColor { get; set; }
        public string FontBackgroundColor { get; set; }


        public NewEditDataChartAxisViewModel()
        {
            AxisName = "";
            AxisCategories_List = new SelectList(new List<SelectListItem>());
            DataChartsTypes_List = new SelectList(new List<SelectListItem>());
            DataTypes_List = new SelectList(new List<SelectListItem>());
            DataFormat_List = new List<Catalog>();
            Color = "#000000";
            Prefix = "";
            Sufix = "";
            IsEdit = false;
            NewEditTitle = Resources.GenericCharts.title_NewAxis;
            ShowLine = true;
            Rotation = 0;
            ShowLabel = true;
            FontSize = 50;
            FontColor = "#000000";
            //FontBackgroundColor = "#000000";
        }
    }
}