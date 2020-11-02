using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.SQL_DataType
{
    public class t_DemoldDefectsCharts
    {
        public string labelData { get; set; }
        public string[] label { get; set; }
        public string yAxisID { get; set; }
        public string type { get; set; }
        public string borderColor { get; set; }
        public string backgroundColorData { get; set; }
        public string[] backgroundColor { get; set; }
        public string borderWidth { get; set; }
        public string pointBorderColor { get; set; }
        public decimal? ydata { get; set; }
        public decimal?[] data { get; set; }

        public string Category { get; set; }
        public int IndexValue { get; set; }
        public string FontColor { get; set; }
        public string GrossData { get; set; }
        public string[] Gross { get; set; }
    }
}
