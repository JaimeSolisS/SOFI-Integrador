using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class EnergySensorValue : TableMaintenance
    {
        public int EnergySensorValueID { get; set; }
        public int EnergySensorID { get; set; }
        public int ValueHour { get; set; }
        public decimal? MaxValue { get; set; }
        public int FacilityID { get; set; }
    }
}
