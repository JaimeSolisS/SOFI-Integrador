using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ProductionVAT : TableMaintenance
    {
        public int VATID { get; set; }
        public string VATName { get; set; }
        public int ShiftID { get; set; }
        public int ProductionLineID { get; set; }
        public int FacilityID { get; set; }
        public bool Enabled { get; set; }

        public string LineNumber { get; set; }
        public string ShiftDescription { get; set; }

    }
}
