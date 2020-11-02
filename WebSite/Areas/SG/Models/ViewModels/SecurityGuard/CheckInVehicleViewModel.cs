using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.SG.Models.SecurityGuard
{
    public class CheckInVehicleViewModel
    {
        public string VehiclePlates;
        public IEnumerable<SelectListItem> VehicleMarksList;

        public CheckInVehicleViewModel()
        {
            VehiclePlates = "";
            VehicleMarksList = new SelectList(new List<SelectListItem>());
        }
    }
}