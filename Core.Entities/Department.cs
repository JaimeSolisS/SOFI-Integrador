using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Department : TableMaintenance
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public bool Enabled { get; set; }
    }
}
