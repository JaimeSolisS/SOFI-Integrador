using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class DemoldDefects : TableMaintenance
    {
        public int ProductionLineID { get; set; }
        public string ProductionLineName { get; set; }
        public int ShiftID { get; set; }
        public string ShiftName { get; set; }
        public string InspectorName { get; set; }
        public int StatusID { get; set; }
        public string VATID { get; set; }
        public string VATName { get; set; }
        public DateTime DefectDate { get; set; }
        public int ProductionProcessID { get; set; }

    }
}