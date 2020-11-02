using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Defect : TableMaintenance
    {
        public int? DefectID { get; set; }
        public string DefectName { get; set; }
        public bool? Enabled { get; set; }

    }
}
