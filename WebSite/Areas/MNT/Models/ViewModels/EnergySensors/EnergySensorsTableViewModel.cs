using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MNT.Models.ViewModels.EnergySensors
{
    public class EnergySensorsTableViewModel
    {
        public IEnumerable<SelectListItem> FamiliesList { get; set; }
        //public IEnumerable<SelectListItem> SensorNamesList { get; set; }
        public IEnumerable<string> SensorNamesList { get; set; }
        public IEnumerable<SelectListItem> UsesList { get; set; }
        public List<Core.Entities.EnergySensors> EnergySensorsList { get; set; }

        public EnergySensorsTableViewModel()
        {
            FamiliesList = new SelectList(new List<SelectListItem>());
            //SensorNamesList = new SelectList(new List<SelectListItem>());
            UsesList = new SelectList(new List<SelectListItem>());
            EnergySensorsList = new List<Core.Entities.EnergySensors>();
        }
    }
}