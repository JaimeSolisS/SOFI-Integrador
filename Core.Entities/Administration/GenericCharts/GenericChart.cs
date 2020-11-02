using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class GenericChart : TableMaintenance
    {
        public int? GenericChartID { get; set; }
        public string ChartName { get; set; }
        public string ChartTitle { get; set; }
        public int ChartAreaID { get; set; }
        public int ChartTypeID { get; set; }
        public bool Enabled { get; set; }

        public string ChartTypeText { get; set; }
        public string ChartAreaName { get; set; }

    }
}
