using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class OpportunitiesProgram_Responsable : TableMaintenance
    {
        public int OPResponsableID { get; set; }
        public int OpportunityProgramID { get; set; }
        public int ResponsableID { get; set; }
        public string FullName { get; set; }
        public string eMail { get; set; }
        public string DepartmentName { get; set; }
        public string AddedBy { get; set; }
        public string EmployeeNumber { get; set; }
    }
}
