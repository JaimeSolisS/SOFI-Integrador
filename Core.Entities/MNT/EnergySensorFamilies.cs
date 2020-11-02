using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class EnergySensorFamilies : TableMaintenance
    {
        public int EnergySensorFamilyID { get; set; }
        public string FamilyName { get; set; }
        public decimal? MaxValueperHour { get; set; }
        public string ImagePath { get; set; }
        public bool Enabled { get; set; }
        public int FacilityID { get; set; }

        public decimal CantOfVoltsByFamily { get; set; }
        public decimal TotalConsumptionQuantity { get; set; }
        public string AlertCssClass { get; set; }
    }
}
