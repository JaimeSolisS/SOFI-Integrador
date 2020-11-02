using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class UserFacility
    {
        public string ID { get { return string.Format("{0}|{1}", UserID, FacilityID); } }
        public int UserID { get; set; }
        public string UserAccountID { get; set; }
        public int? OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public int? CompanyID { get; set; }
        public string CompanyName { get; set; }
        public int? FacilityID { get; set; }
        public string FacilityName { get; set; }
        public int ChangedBy { get; set; }
        public string ChangedByName { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateLastMaint { get; set; }

    }
}
