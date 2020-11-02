using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class TranslationDetail : TableMaintenance
    {
        public string Tag { get; set; }
        public string CultureID { get; set; }
        public string Description { get; set; }

        public Nullable<int> CatalogDetailID
        {
            get { return int.Parse(Tag.Replace("Cat_", "")); }
        }

        public bool EditAccess { get; set; }

        public string classeditable
        {
            get { return EditAccess == true ? "x-editable editableField" : ""; }
        }
    }
}
