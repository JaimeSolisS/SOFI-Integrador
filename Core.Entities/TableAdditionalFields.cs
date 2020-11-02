using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class TableAdditionalFields : TableMaintenance
    {
        public int TableAdditionalFieldID { get; set; }
        public int TableAdditionalFieldValueID { get; set; }
        public int OrganizationID { get; set; }
        public int CompanyID { get; set; }
        public int FacilityID { get; set; }
        public string TableName { get; set; }
        public int Seq { get; set; }
        public string ColumnName { get; set; }
        public string DataTypeID { get; set; }
        public int FieldLength { get; set; }
        public int FieldPrecision { get; set; }
        public string CatalogTag { get; set; }
        public bool IsMandatory { get; set; }
        public string CssClass { get; set; }
        public bool Enabled { get; set; }
        public string DataTypeControl { get; set; }
        public string FieldValue { get; set; }
        public string FieldIcon { get; set; }
        public string CatalogJSON { get; set; }
        public string RealName { get; set; }
        public string FieldName { get { if (IsMandatory) { return string.Format("{0} {1}", ColumnName, "*"); } return ColumnName; } }
        public List<Catalog> CatalogContext;

        public string ColsBootstrapCssClass
        {
            get
            {
                if (DataTypeControl.ToLower().Equals("text"))
                {
                    if (FieldLength <= 40) { return "col-xs-12 col-sm-4 col-md-3"; }
                    else if (FieldLength > 40 && FieldLength <= 90) { return "col-xs-12 col-sm-6"; }
                    else if (FieldLength > 90) { return "col-xs-12"; }
                }

                return "col-xs-12 col-sm-4 col-md-3";
            }
        }

        public int TextAreaRows
        {
            get
            {
                if (DataTypeControl.ToLower().Equals("text"))
                {
                    if (FieldLength > 130) { return FieldLength / 130; }
                }
                return 1;
            }
        }
    }
}
