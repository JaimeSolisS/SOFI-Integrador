using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class AppMenu
    {
        public int MenuID { get; set; }
        public Nullable<int> ParentMenuID { get; set; }
        public string Description { get; set; }
        public string NavigateTo { get; set; }
        public int Sequence { get; set; }
        public bool LoginRequired { get; set; }
        public bool Enabled { get; set; }
        public int UserID { get; set; }
        public System.DateTime DateAdded { get; set; }
        public System.DateTime DateLastMaint { get; set; }
        public string MenuCode { get; set; }
        public string Icon { get; set; }
    }
}
