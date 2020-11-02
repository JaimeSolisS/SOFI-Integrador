using Core.Entities;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebSite.Areas.MNT.Models.ViewModels.EnergySensors
{
    public class EnergySensorValuesViewModel
    {
        public int EnergySensorID { get; set; }
        public int? EnergySensorFamilyID { get; set; }
        public string FamilyName { get; set; }
        public IEnumerable<SelectListItem> EnergySensorsList { get; set; }
        public List<EnergySensorValue> EnergySensorValuesList { get; set; }
        public string DateFormat { get; set; }
        public string HoursArray { get; set; }


        public EnergySensorValuesViewModel()
        {
            EnergySensorValuesList = new List<EnergySensorValue>();
            EnergySensorsList = new SelectList(new List<SelectListItem>());
            EnergySensorID = 0;
            EnergySensorFamilyID = 0;
            FamilyName = "";
            DateFormat = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            HoursArray = "";
        }
    }
}