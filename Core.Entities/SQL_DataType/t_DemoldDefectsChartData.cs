using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.SQL_DataType
{
    public class t_DemoldDefectsChartData
    {
        public List<t_DemoldDefectsCharts> SeriesList { get; set; }
        public List<t_ChartData> TopDefects { get; set; }

    }
}
