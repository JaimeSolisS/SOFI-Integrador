using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Badge : TableMaintenance
    {
        public int BadgeID { get; set; }
        public string BadgeNumber { get; set; }
        public string UniqueNumber { get; set; }
        public int BadgeTypeID { get; set; }
        public string BadgeTypeName { get; set; }
        public int ReferenceTypeID { get; set; }
        public int FacilityID { get; set; }
    }
}
