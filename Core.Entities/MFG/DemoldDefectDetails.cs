using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class DemoldDefectDetail : TableMaintenance
    {
        public int DemoldDefectDetailID { get; set; }
        public int DemoldDefectID { get; set; }
        public int ProductID { get; set; }
        public int MoldFamilyID { get; set; }
        public int BaseID { get; set; }
        public int AdditionID { get; set; }
        public int SideEyeID { get; set; }
        public int Quantity { get; set; }
        public int DemoldDefectTypeID { get; set; }
        public string DemoldDefectTypeName { get; set; }
        public string DemoldDefectCategoryName { get; set; }
        public int ShiftID { get; set; }
        public string ShiftDescription { get; set; }
        public int ProductionLineID { get; set; }
        public string LineNumber { get; set; }
        public string InspectorName { get; set; }
        public string FamilyName { get; set; }
        public DateTime DefectDate { get; set; }
        public string DayCode { get; set; }
        public int Gross { get; set; }

        public string SideName { get; set; }
        public string AdditionName { get; set; }
        public string BaseName { get; set; }
        public string VATName { get; set; }
        public string InspectorNameDetail { get; set; }
        public int LensTypeID { get; set; }
        public string LensTypeName { get; set; }
        public decimal GrossPercentage { get; set; }
    }
}

