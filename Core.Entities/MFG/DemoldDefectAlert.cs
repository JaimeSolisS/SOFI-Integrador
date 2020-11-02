using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class DemoldDefectAlert : TableMaintenance
    {
        public int DemoldDefectAlertID { get; set; }
        public int ShiftID { get; set; }
        public string ShiftDescription { get; set; }
        public int ProductionLineID { get; set; }
        public string LineNumber { get; set; }
        public int MoldFamilyID { get; set; }
        public string FamilyName { get; set; }
        public int LensTypeID { get; set; }
        public string LensTypeName { get; set; }
        public decimal Gross { get; set; }
        public string DefectCategory { get; set; }
        public int HourInterval { get; set; }
        public bool Enabled { get; set; }
        public int FacilityID { get; set; }
    }
}
