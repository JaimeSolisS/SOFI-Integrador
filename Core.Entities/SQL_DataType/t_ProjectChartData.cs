using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.SQL_DataType
{
    public class t_ProjectChartData
    {
        public int seq { get; set; }
        public string ProjectName { get; set; }
        public string Annotations { get; set; }
        public int startmonth { get; set; }
        public int months { get; set; }    
        public string backgroundColor { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public decimal xAdjust { get; set; }
    }
}
