using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class AppEvents
    {
        public int EventID { get; set; }
        public Nullable<int> MenuID { get; set; }
        public string EventDescription { get; set; }
        public bool Enabled { get; set; }
        public Nullable<int> UserID { get; set; }
        public System.DateTime DateAdded { get; set; }
        public System.DateTime DateLastMaint { get; set; }
        public bool NeedAccessType { get; set; }
        public int ReadOnlyID { get; set; }
        public int FullAccessID { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsFullAccess { get; set; }

        public string VisibleNeedAccessType
        {
            get { return NeedAccessType ? "" : "hidden"; }
        }

        public string PercentageDesc
        {
            get { return NeedAccessType ? "60" : ""; }
        }
    }
}
