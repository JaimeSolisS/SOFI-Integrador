using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CheckInPersonInfo
    {
        public string UserName { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int UserTypeID { get; set; }
        public int VendorTypeID { get; set; }
        public int VendorUserID { get; set; }
        public string UserTypeName { get; set; }
        public int IdentificationTypeID { get; set; }
        public string IdentificationPath { get; set; }
        public bool SessionStateStarted { get; set; }
        public int? SecurityGuardLogID { get; set; }
    }
}
