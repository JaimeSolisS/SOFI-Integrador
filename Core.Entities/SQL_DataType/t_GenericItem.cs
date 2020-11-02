using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.SQL_DataType
{
    public class t_GenericItem
    {
        private string _FieldValue;
        public int RowNumber { get; set; }
        public int FieldKey { get; set; }
        public string FieldValue
        {
            get
            {
                if (string.IsNullOrEmpty(_FieldValue))
                {
                    return string.Empty;
                }
                return _FieldValue;
            }
            set { _FieldValue = value; }
        }
    }
}
