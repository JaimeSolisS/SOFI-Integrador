using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.MNT.Models.ViewModels.EnergySensors
{
    public class EnergySensorDetailsViewModel
    {
        public List<Core.Entities.EnergySensors> EnergySensorsList { get; set; }
        public string LastHour { get; set; }
        public int? EnergySensorFamilyID { get; set; }
        public string FamilyName { get; set; }

        public EnergySensorDetailsViewModel()
        {
            EnergySensorsList = new List<Core.Entities.EnergySensors>();
            LastHour = "";
            EnergySensorFamilyID = 0;
            FamilyName = "";
        }
    }
}