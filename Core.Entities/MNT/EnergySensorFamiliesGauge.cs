using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class EnergySensorFamiliesGauge
    {
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
        public decimal ParameterValue { get; set; }

        public EnergySensorFamiliesGauge()
        {
            MinValue = 0;
            MaxValue = 0;
            ParameterValue = 0;
        }
    }
}
