
using System.Collections.Generic;


namespace Core.Entities
{
    public class Chart_ComputerSetting : TableMaintenance
    {
        public int Chart_SettingID { get; set; }
        public string ComputerName { get; set; }
        public int ChartID { get; set; }
        public string ChartName { get; set; }
        public string OptionsTitle { get; set; }
        public int ChartType { get; set; }
        public int Seq { get; set; }
        public int ShiftID { get; set; }
        public int IntervalRefreshTime { get; set; }
        public IEnumerable<Charts_ComputerSettingsDetail> Options { get; set; }
    }
}
