using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Facility : TableMaintenance
    {
        public int FacilityID { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string FacilityName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public Nullable<int> CityID { get; set; }
        public string CityName { get; set; }
        public Nullable<int> StateID { get; set; }
        public string StateName { get; set; }
        public Nullable<int> CountryID { get; set; }
        public string CountryName { get; set; }
        public string Location { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public bool Enabled { get; set; }
        public bool AllowEdit { get; set; }
    }
}
