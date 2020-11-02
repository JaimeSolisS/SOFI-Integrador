

namespace Core.Entities
{
    public class DashboardOperationParamater : TableMaintenance
    {
        public int OperationSetupParameterID { get; set; }
        public int ParameterSectionID { get; set; }
        public string ParameterSectionName { get; set; }
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
        public string MinValueFormat
        {
            get { return MinValue.Value.ToString("0.##"); }
        }
        public string MaxValueformat
        {
            get { return MaxValue.Value.ToString("0.##"); }
        }
        public string Value { get; set; }
        public string ValueListValue { get; set; }        
        public string ParameterName { get; set; }
        public string ParameterTypeValue { get; set; }
        public bool Alert { get; set; }
        public string AlertCssClass { get; set; }
        public bool? UseReference { get; set; }
        public string Reference { get; set; }
        public string ReferenceName { get; set; }
        public string ReferenceTypeValue { get; set; }
    }
}
