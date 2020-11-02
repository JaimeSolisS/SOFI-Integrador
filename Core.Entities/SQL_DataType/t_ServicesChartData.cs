using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.SQL_DataType
{
    public class t_ServicesChartData
    {
        public string name { get; set; }
        public decimal?[] data { get; set; }
    }
}
