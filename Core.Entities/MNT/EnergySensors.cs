using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class EnergySensors : TableMaintenance
    {
        public int EnergySensorID { get; set; }
        public string SensorName { get; set; }
        public int EnergySensorFamilyID { get; set; }
        public string FamilyName { get; set; }
        public int SensorUseID { get; set; }
        public string SensorUseName { get; set; }
        public int UnitID { get; set; }
        public string IndexKey { get; set; }
        public string Deviceid { get; set; }
        public bool Enabled { get; set; }
        public string ImagePath { get; set; }

        public string AlertCssClass { get; set; }
    }
}
