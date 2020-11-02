using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class SecurityGuardLog : TableMaintenance
    {
        public int? SecurityGuardLogID { get; set; }
        public string PersonID { get; set; }
        public string PersonName { get; set; }
        public int? PersonTypeID { get; set; }
        public int? VendorTypeID { get; set; }
        public string PersonTypeName { get; set; }
        public string WhoVisit { get; set; }
        public string VehiclePlates { get; set; }
        public int? VehicleMarkID { get; set; }
        public string VehicleMarkName { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string SecurityOfficerID { get; set; }
        public int? BadgeID { get; set; }
        public string BadgeNumber { get; set; }
        public int? IdentificationTypeID { get; set; }
        public string IdentificationTypeName { get; set; }
        public string IdentificationImgPath { get; set; }
        public bool HaveTools { get; set; }
        public string EquipmentIDs { get; set; }
        public int? FacilityID { get; set; }

        public string CheckInFormat
        {
            get
            {
                if (CheckIn != null)
                    return DateLastMaint.ToString("yyyy-MM-dd HH:mm");
                else
                    return " - ";
            }
        }

        public string CheckOutFormat
        {
            get
            {
                if (CheckOut != null)
                    return DateLastMaint.ToString("yyyy-MM-dd HH:mm");
                else
                    return " - ";
            }
        }
    }
}
