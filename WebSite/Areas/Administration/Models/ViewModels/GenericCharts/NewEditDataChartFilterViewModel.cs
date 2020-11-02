using System.Collections.Generic;
using System.Web.Mvc;
using Core.Entities;

namespace WebSite.Areas.Administration.Models.ViewModels.GenericCharts
{
    public class NewEditDataChartFilterViewModel
    {
        public int? GenericChartFilterID { get; set; }
        public string FilterName { get; set; }
        public IEnumerable<SelectListItem> FilterTypes_List { get; set; }
        public IEnumerable<SelectListItem> ListsForFilterTypes_List { get; set; }
        public List<Catalog> DefaultValue_List { get; set; }
        public IEnumerable<SelectListItem> FiltersFormulas_List { get; set; }
        public IEnumerable<SelectListItem> FiltersDefault_List { get; set; }
        public string InputValue { get; set; }
        public bool Enabled { get; set; }
        public bool IsEdit { get; set; }
        public string NewEditTitle { get; set; }
        public GenericChartsFilters FilterEntity { get; set; }


        public NewEditDataChartFilterViewModel()
        {
            FilterName = "";
            FilterTypes_List = new SelectList(new List<SelectListItem>());
            ListsForFilterTypes_List = new SelectList(new List<SelectListItem>());
            DefaultValue_List = new List<Catalog>();
            FiltersFormulas_List = new SelectList(new List<SelectListItem>());
            FiltersDefault_List = new SelectList(new List<SelectListItem>());
            InputValue = "";
            Enabled = true;
            IsEdit = false;
            NewEditTitle = Resources.GenericCharts.title_NewFilter;
            FilterEntity = new GenericChartsFilters();
            //GenericChartFilterID = 0;
        }
    }
}