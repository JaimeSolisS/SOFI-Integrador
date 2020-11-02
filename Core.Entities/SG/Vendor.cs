using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Vendor : TableMaintenance
    {
        public int VendorID { get; set; }
        public int OrganizationID { get; set; }
        public string VendorName { get; set; }
        public bool Enabled { get; set; }
    }
}