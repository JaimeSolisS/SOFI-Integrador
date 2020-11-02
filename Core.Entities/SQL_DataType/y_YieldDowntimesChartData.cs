using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.SQL_DataType
{
    public class y_YieldDowntimesChartData
    {
        public List<t_SeriesChart> SeriesList { get; set; }
        public List<t_ChartData> TopDowntimes { get; set; }
        public List<t_ChartData> TopDefects { get; set; }

    }
}
