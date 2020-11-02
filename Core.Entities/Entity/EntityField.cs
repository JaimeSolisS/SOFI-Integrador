using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class EntityField
    {
        public Nullable<int> FieldConfigurationID { get; set; }
        public Nullable<int> ID { get; set; }
        public int FacilityID { get; set; }
        public int EntityID { get; set; }
        public int EntityIndicator { get; set; }
        public int SystemModule { get; set; }
        public int FieldID { get; set; }
        public string ValueID { get; set; }
        public bool IsEditable { get; set; }
        public bool IsVisible { get; set; }
        public bool IsMandatory { get; set; }
        public string FieldDescription { get; set; }
        public string DefaultTranslation { get; set; }
        public string CustomTranslation { get; set; }
        public string SystemModuleTag { get; set; }
        public bool AllowAcces { get; set; }
        public string FieldName { get { if (IsMandatory) { return string.Format("{0} {1}", FieldDescription, "*"); } return FieldDescription; } }
        public string ReadOnly
        {
            get
            {
                return AllowAcces ? "" : "readonly";
            }
        }


    }
}
