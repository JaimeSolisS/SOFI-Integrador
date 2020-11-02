

namespace Core.Entities
{
    public class ChartOption : TableMaintenance
    {
        public int ChartOptionID { get; set; }
        public int ChartID { get; set; }
        public int OptionID { get; set; }
        public string OptionName { get; set; }
        public string OptionType { get; set; }
        public string OptionTypeCatalog { get; set; }
        public string OptionValue { get; set; }
    }
}
