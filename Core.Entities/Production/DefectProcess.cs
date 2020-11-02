using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Production
{
    public class DefectProcess : TableMaintenance
    {
        public int? DefectProcessID { get; set; }
        public int? ProductionProcessID { get; set; }
        public string ProductionProcessName { get; set; }
        public int? ProductionLineID { get; set; }
        public string ProductionLineName { get; set; }
        public int? VAID { get; set; }
        public string VAName { get; set; }
        public int? DesignID { get; set; }
        public string DesignName { get; set; }
        public int DefectID { get; set; }
        public string Color { get; set; }
        public string FontColor { get; set; }
        public int? ShiftID { get; set; }
        public string ShiftName { get; set; }
        /*F.Vera - No cambiar a decimal por que se consume a travez de un web method*/
        public float? GoalValue { get; set; }
        public string GoalValueFormat
        {
            get { return string.Format("{0:###.#0} %", this.GoalValue); }
        }
        public string ColorDefault
        {
            get
            {
                if (!string.IsNullOrEmpty(Color))
                {
                    return "#000000";
                }
                return Color;
            }
        }

        public string FontColorDefault
        {
            get
            {
                if (!string.IsNullOrEmpty(FontColor))
                {
                    return "#000000";
                }
                return FontColor;
            }
        }
    }
}
