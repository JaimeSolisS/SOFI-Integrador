using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.SQL_DataType
{
    public class t_SeriesChart
    {
        public string label { get; set; }
        public string yAxisID { get; set; }
        public string type { get; set; }
        public string borderColor { get; set; }
        public string backgroundColor { get; set; }
        public string borderWidth { get; set; }
        public string pointBorderColor { get; set; }
        public decimal?[] data { get; set; }

    }
}
