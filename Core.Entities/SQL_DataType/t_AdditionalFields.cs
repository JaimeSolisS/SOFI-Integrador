using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.SQL_DataType
{
    public class t_AdditionalFields
    {
        public int TableAdditionalFieldID { get; set; }
        public int TableAdditionalFieldValueID { get; set;}
        public string FieldValue { get; set; }
    }
}
