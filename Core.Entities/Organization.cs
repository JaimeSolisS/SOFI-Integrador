using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Organization :TableMaintenance
    {
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public bool Enabled { get; set; }
    }
}
