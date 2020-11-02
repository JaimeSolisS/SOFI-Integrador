using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class GenericChartsFilters
    {
        public int? GenericChartFilterID { get; set; }
        public int? GenericChartID { get; set; }
        public string FilterName { get; set; }
        public int FilterTypeID { get; set; }
        public int FilterListID { get; set; }
        public string DefaultValue { get; set; }
        public int? DefaultValueFormula { get; set; }
        public bool Enabled { get; set; }

        public string FilterTypeName { get; set; }
        public string FilterListName { get; set; }
        public string DefaultValueName { get; set; }
        public string DefaultValueText { get; set; }
    }
}
