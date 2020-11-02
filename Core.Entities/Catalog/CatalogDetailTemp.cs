using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CatalogDetailTemp : TableMaintenance
    {
        public int CatalogDetailTempID { get; set; }
        public int CatalogDetailID { get; set; }
        public int CatalogID { get; set; }
        public Guid TransactionID { get; set; }
        public string CatalogName { get; set; }
        public string ValueID { get; set; }
        public string ValueTranslation { get; set; }
        public string Param1 { get; set; }
        public bool Param1_IsConfigured { get; set; }
        public string Param1_Name { get; set; }
        public string Param2 { get; set; }
        public bool Param2_IsConfigured { get; set; }
        public string Param2_Name { get; set; }
        public string Param3 { get; set; }
        public bool Param3_IsConfigured { get; set; }
        public string Param3_Name { get; set; }
        public string Param4 { get; set; }
        public bool Param4_IsConfigured { get; set; }
        public string Param4_Name { get; set; }
        public bool UserHasAccess { get; set; }
        public string CultureID { get; set; }
        public int PhaseTempID { get; set; }
        public int Seq { get; set; }
        public int FormatLoopFlowTempID { get; set; }
        public string UserName { get; set; }
        public string PhaseCheck 
        {
            get { return (PhaseTempID != 0) ? "checked" : "";  }
        }
        public string Tag
        {
            get { return string.Format("Cat_{0}", CatalogDetailID); }
        }

        public bool EditAccess { get; set; }

        public string classeditable
        {
            get { return (EditAccess == true) && (UserHasAccess ==  true) ? "x-editable editableField" : ""; }
        }
    }
}
