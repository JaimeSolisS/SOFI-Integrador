using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class MoldScraps : TableMaintenance
    {
        public int ScrapID { get; set; }
        public DateTime ScrapDate { get; set; }
        public TimeSpan ScrapTime { get; set; }
        public Decimal Quantity { get; set; }
        public int HourValue { get; set; }
        public int ProductionProcessID { get; set; }
        public int ProductionLineID { get; set; }
        public string ShiftName { get; set; }
        public int ShiftID { get; set; }
        public int DesignID { get; set; }
        public string ScrapDateFormat
        {
            get { return DateLastMaint.ToString("yyyy-MM-dd HH:mm"); }
        }
        public string HourValueFormat { get; set; }

    }
}
