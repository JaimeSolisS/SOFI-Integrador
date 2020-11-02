using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Catalog
    {
        public int CatalogDetailID { get; set; }
        public int CatalogID { get; set; }
        public string CatalogTag { get; set; }
        public string CatalogName { get; set; }
        public string CatalogDescription { get; set; }
        public Nullable<int> OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public Nullable<int> FacilityID { get; set; }
        public string FacilityName { get; set; }
        public bool IsSystemValue { get; set; }
        public bool Enabled { get; set; }
        public string ValueID { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }
        public string Param4 { get; set; }
        public string CultureID { get; set; }
        public string DisplayText { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }

        public string ValueIDCatalogID
        {
            get { return Convert.ToString(CatalogID) + '|' + ValueID; }
        }
    }
}
