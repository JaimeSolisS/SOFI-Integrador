

namespace Core.Entities
{
    public class MachineParameters
    {
        public int MachineParameterID { get; set; }
        public int Seq { get; set; }
        public string ParameterName { get; set; }
        public int? ParameterTypeID { get; set; }
        public int? ParameterLength { get; set; }
        public int? ParameterPrecision { get; set; }
        public bool? UseReference { get; set; }
        public bool? IsCavity { get; set; }
        public bool? Enabled { get; set; }
        public string UoM { get; set; }
        public bool? IsMandatory { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public int? ParameterListID { get; set; }
        public string ReferenceName { get; set; }
        public int ReferenceTypeID { get; set; }

        public string ParameterTypeName { get; set; }

    }
}
