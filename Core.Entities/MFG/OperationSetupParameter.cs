

namespace Core.Entities
{
    public class OperationSetupParameter : TableMaintenance
    {
        public int OperationSetupParameterID { get; set; }
        public int OperationSetupID { get; set; }
        public string ProductionProcessValue { get; set; }
        public int ParameterSectionID { get; set; }
        public string ParameterSectionName { get; set; }
        public int Seq { get; set; }
        public int MachineParameterID { get; set; }
        public string OperationParameterName { get; set; }
        public int? ParameterUoMID { get; set; }
        public string ParameterUoMName { get; set; }
        public int? ParameterTypeID { get; set; }
        public string ParameterTypeValue { get; set; }
        public int? ParameterLength { get; set; }
        public int? ParameterPrecision { get; set; }
        public int? ParameterListID { get; set; }
        public bool? IsMandatory { get; set; }
        public bool? UseReference { get; set; }
        public string Reference { get; set; }
        public string ReferenceName { get; set; }
        public string ReferenceTypeValue { get; set; }
        public int? ReferenceListID { get; set; }
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
        public string Value { get; set; }
        public string ValueList { get; set; }

        public string FunctionValue { get; set; }
        public string FunctionMin { get; set; }
        public string FunctionMax { get; set; }

    }
}
