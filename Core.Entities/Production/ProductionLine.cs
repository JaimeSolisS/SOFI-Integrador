using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ProductionLine : TableMaintenance
    {
        public int? ProductionLineID { get; set; }
        public int? ProductionProcessID { get; set; }
        public string LineNumber { get; set; }
        public bool Enabled { get; set; }
        public int FacilityID { get; set; }
        public string FacilityName { get; set; }
    }
}
