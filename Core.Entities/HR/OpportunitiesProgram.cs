using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class OpportunitiesProgram : TableMaintenance
    {
        public int OpportunityProgramID { get; set; }
        public string OpportunityNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DescriptionTypeID { get; set; }
        public string DescriptionTypeName { get; set; }
        public string DescriptionTypeValueID { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int ShiftID { get; set; }
        public string ShiftDescription { get; set; }
        public int GradeID { get; set; }
        public string GradeName { get; set; }
        public bool EnableToApply { get; set; }
        public DateTime ExpirationDate { get; set; }

        public string ExpirationDateFormatted {
            get { return ExpirationDate.ToString("yyyy-MM-dd"); }
        }

        public int? NotificationsQty { get; set; }
        public int? Candidates { get; set; }
        public bool Enabled { get; set; }
        public int FacilityID { get; set; }
        public int CreatedBy { get; set; }
      
    }
}