using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Core.Entities
{
    public class VendorUser : TableMaintenance
    {
        public int? VendorUserID { get; set; }
        public int? VendorID { get; set; }
        public string FullName { get; set; }
        public string AccessCode { get; set; }
        public Int64 InsuranceNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool Enabled { get; set; }
        public int ReferenceTypeID { get; set; }
        public int VendorTypeID { get; set; }
        public string ExpirationDateClass { get; set; }
        public string InactiveVendorUserClass
        {
            get
            {
                if (Enabled)
                {
                    return "";
                }
                else
                {
                    return "vendor-user-inactive";
                }
            }
        }
    }
}
