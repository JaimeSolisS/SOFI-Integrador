using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class GenericChartFilterData
    {

        public int GenericChartFilterDataID { get; set; }
        public int GenericChartHeaderDataID { get; set; }
        public int GenericChartFilterID { get; set; }
        public string FilterValue { get; set; }
    }
}
