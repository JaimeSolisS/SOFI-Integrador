using Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebSite.Models.ViewModels.ChartSlider
{
    public class OptionViewModel
    {
        public OptionViewModel()
        {
            OptionSelectList = new SelectList(Enumerable.Empty<SelectListItem>());
            OptionListBox = new SelectList(Enumerable.Empty<SelectListItem>());
        }
        public Catalog Option { get; set; }
        public string ControlName { get; set; }
        public string OptionValue { get; set; }
        public int Chart_SetttingDetailID { get; set; }
        public IEnumerable<SelectListItem> OptionSelectList { get; set; }
        public IEnumerable<SelectListItem> OptionListBox { get; set; }
        public List<int> SelectedItems { get; set; }
    }
}