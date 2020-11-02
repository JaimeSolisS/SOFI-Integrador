using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class GenericChartsAxis : TableMaintenance
    {
        public int? GenericChartAxisID { get; set; }
        public int? GenericChartID { get; set; }
        public string AxisName { get; set; }
        public int AxisTypeID { get; set; }
        public int AxisChartTypeID { get; set; }
        public int AxisDatatypeID { get; set; }
        public string AxisColor { get; set; }
        public string AxisFormat { get; set; }
        public string AxisPrefix { get; set; }
        public string AxisSufix { get; set; }

        public string Category { get; set; }
        public string DataTypeName { get; set; }
        public string DataChartTypeName { get; set; }
        public bool IsAxeX { get; set; }
        public int AxisFormatID { get; set; }
        public bool ShowLine { get; set; }
        public int DataLabelRotation { get; set; }
        public bool DataLabelShow { get; set; }
        public int DataLabelFontSize { get; set; }
        public string DataLabelFontColor { get; set; }
        public string DataLabelFontBGColor { get; set; }
    }
}
