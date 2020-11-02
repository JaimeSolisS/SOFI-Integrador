using Core.Entities;
using System.Collections.Generic;


namespace WebSite.Areas.MNT.Models.ViewModels.EnergySensors
{
    public class EnergySensorFamiliesViewModel
    {
        public int? EnergySensorFamilyID { get; set; }
        public string Familyame { get; set; }
        public decimal? MaxValueperHour { get; set; }
        public bool Enabled { get; set; }
        public List<EnergySensorFamilies> EnergySensorFamiliesList { get; set; }
        public bool IsEdit { get; set; }
        public string Date { get; set; }
        public string LastHour { get; set; }

        public decimal MaxValue { get; set; }

        public EnergySensorFamiliesViewModel()
        {
            EnergySensorFamilyID = 0;
            Familyame = "";
            MaxValueperHour = 0;
            Enabled = true;
            EnergySensorFamiliesList = new List<EnergySensorFamilies>();
            IsEdit = false;
            Date = "";
            LastHour = "";
            MaxValue = 0;
        }
    }
}