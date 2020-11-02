using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ProductionGoalsDetail : TableMaintenance
    {
        public int? GoalDetailID { get; set; }
        public int? GoalID { get; set; }
        public int? Hour { get; set; }
        public string HourValue { get; set; }
        public string HourValueFormat { get; set; }
        public float? GoalValue { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string GoalValueFormat
        {
            get { return string.Format("{0:##0.#0} %", this.GoalValue); }
        }
    }
}
