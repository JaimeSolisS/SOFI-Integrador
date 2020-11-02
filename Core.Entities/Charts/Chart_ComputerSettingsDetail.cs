

namespace Core.Entities
{
    public class Charts_ComputerSettingsDetail : TableMaintenance
    {
        public int Chart_SetttingDetailID { get; set; }
        public int Chart_SettingID { get; set; }
        public int OptionID { get; set; }
        public string OptionName { get; set; }
        public string OptionType { get; set; }
        public string OptionTypeCatalog { get; set; }
        public string OptionValue { get; set; }
    }
}
