using Core.Entities;
using System.Collections.Generic;



namespace WebSite.Models.ViewModels.ChartSlider
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            ChartList = new List<Chart>();
            ComputerName = "";
        }
        public int CurrentShift { get; set; }
        public List<Chart_ComputerSetting> Charts { get; set; }
        public string ComputerName { get; set; }
        public List<Chart> ChartList { get; set; }
        public string HoursArray { get; set; }
        public int ProductionProcessID { get; set; }
        public int LineID { get; set; }
    }
}