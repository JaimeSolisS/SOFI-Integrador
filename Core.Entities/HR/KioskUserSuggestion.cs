using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class KioskEmployeeSuggestion : TableMaintenance
    {
        public int KioskEmployeeSuggestionID { get; set; }
        public string EmployeeNumber { get; set; }
        public string SuggestedBy { get; set; }
        public int FacilityID { get; set; }
        public string FacilityName { get; set; }

        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Comments { get; set; }
    }
}
