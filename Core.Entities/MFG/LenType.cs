using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class LenType : TableMaintenance
    {
        public int MoldFamilyLensTypeID { get; set; }
        public int LensTypeID { get; set; }
        public string LensTypeName { get; set; }
        public int ProductionDesignID { get; set; }
        public string IsSimpleVision { get; set; }
        public bool Enabled { get; set; }
        public int FacilityID { get; set; }

    }
}
