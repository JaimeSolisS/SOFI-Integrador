using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.SQL_DataType
{
    public class t_MoldScrap
    {
        public int RowNumber { get; set; }
        public int ScrapID { get; set; }
        public int HourValue { get; set; }
        public decimal Quantity  { get; set; }
    }
}
