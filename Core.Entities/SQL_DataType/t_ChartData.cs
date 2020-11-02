using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.SQL_DataType
{
    public class t_ChartData
    {
        public string xdata { get; set; }
        public string ydata { get; set; }
        public string color { get; set; }
        public decimal GoalValue { get; set; }
        public string FontColor { get; set; }
    }
}
