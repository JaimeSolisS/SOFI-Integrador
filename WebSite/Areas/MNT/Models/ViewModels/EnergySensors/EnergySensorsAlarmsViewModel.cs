using Core.Entities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebSite.Areas.MNT.Models.ViewModels.EnergySensors
{
    public class EnergySensorsAlarmsViewModel
    {
        public List<EnergySensorValue> SensorsConfigList { get; set; }
        public IEnumerable<SelectListItem> SensorConfigForCopyList { get; set; }
        public string SensorName { get; set; }
        public int EnergySensorID { get; set; }

        public EnergySensorsAlarmsViewModel()
        {
            SensorsConfigList = new List<EnergySensorValue>();
            SensorName = "";
            EnergySensorID = 0;
            SensorConfigForCopyList = new SelectList(new List<SelectListItem>());
        }
    }
}