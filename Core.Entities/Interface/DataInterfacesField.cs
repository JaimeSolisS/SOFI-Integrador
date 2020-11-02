using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class DataInterfacesField : TableMaintenance
    {
        public int DataInterfacesFieldID { get; set; }
        public int DataInterfaceID { get; set; }
        public int ColumnNumber { get; set; }
        public string FileName { get; set; }
        public string FieldName { get; set; }
        public int FieldTypeID { get; set; }
        public int FieldLength { get; set; }
        public int FieldPrecision { get; set; }

    }
}
