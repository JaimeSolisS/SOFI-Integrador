using Core.Entities;
using System.Collections.Generic;


namespace WebSite.Models.ViewModels.ChartSlider
{
    public class ConfigViewModel
    {
        public ConfigViewModel()
        {
            ChartList = new List<Chart>();
        }
        public string CompName { get; set; }
        public List<Chart> ChartList { get; set; }
        public List<ShiftsMaster> ShiftList { get; set; }
        public int ShiftID { get; set; }
        public int DFT_TimerSetup { get; set; }
        public List<Chart_ComputerSetting> Charts { get; set; }
    }
}