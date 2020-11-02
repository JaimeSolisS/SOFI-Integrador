

namespace Core.Entities
{
    public class MachineSetupParameters : TableMaintenance
    {
        public int? MachineSetupParameterID { get; set; }
        public int MachineSetupID { get; set; }
        public int ParameterSectionID { get; set; }
        public string ParameterName { get; set; }
        public string UoM { get; set; }
        public int Seq { get; set; }
        public int MachineParameterID { get; set; }
        public int ParameterUoMID { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsAlert { get; set; }
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
        public string IsMandatoryText { get; set; }
        public string IsAlertText { get; set; }

        public int MaxLength { get; set; } 
        public string FunctionValue { get; set; }
        public string FunctionMinValue { get; set; }
        public string FunctionMaxValue { get; set; }

        public string FunctionFormatedValue { get; set; }
        public string FunctionFormatedMinValue { get; set; }
        public string FunctionFormatedMaxValue { get; set; }

        public int IsCalculated { get; set; }
    }
}
