using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.MNT.Models.ViewModels.EnergyDashboard
{
    public class ChartFamiliesGaugeViewModel
    {
        public string DateFormat { get; set; }
        public string DateFormatWithTime { get; set; }
        public int EnergySensorID { get; set; }

        public ChartFamiliesGaugeViewModel()
        {
            DateFormat = String.Format("{0:yyyy-MM-dd}", DateTime.Now);
            DateFormatWithTime = String.Format("{0:yyyy-MM-dd HH:mm}", DateTime.Now);
            EnergySensorID = 0;
        }
    }
}