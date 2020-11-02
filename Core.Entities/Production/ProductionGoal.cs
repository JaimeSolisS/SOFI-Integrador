using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ProductionGoal : TableMaintenance
    {
        public int? GoalID { get; set; }
        public int? GoalNameID { get; set; }
        public string GoalName { get; set; }
        public int? ProductionProcessID { get; set; }
        public string ProductionProcessName { get; set; }
        public int? ProductionLineID { get; set; }
        public string ProductionLineName { get; set; }
        public int? VAID { get; set; }
        public string VAName { get; set; }
        public int? DesignID { get; set; }
        public string DesignName { get; set; }
        public int? ShiftID { get; set; }
        public string ShiftName { get; set; }
        /*F.Vera - No cambiar a decimal por que se consume a travez de un web method*/
        public float GoalValue { get; set; }
        public string GoalValueFormat
        {
            get { return string.Format("{0:##0.#0} %", this.GoalValue); }
        }
        public string ClassNameShowDetails { get; set; }
    }
}
