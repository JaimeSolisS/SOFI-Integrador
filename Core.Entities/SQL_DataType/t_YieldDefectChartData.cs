using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.SQL_DataType
{
    public class t_YieldDefectChartData
    {
        public List<t_SeriesChart> SeriesList { get; set; }
        public List<t_ChartData> TopDefects { get; set; }
    }
}
