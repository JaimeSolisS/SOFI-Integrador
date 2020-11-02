using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.HR.Models.ViewModels.Kiosk
{
    public class KioskAreaViewModel
    {
        public List<KioskArea> KioskAreasList { get; set; }
        public int? BackKioskAreaID { get; set; }
        public int? BackParentID { get; set; }
        public int ParentID { get; set; }

        public KioskAreaViewModel()
        {
            KioskAreasList = new List<KioskArea>();
            BackKioskAreaID = 0;
            BackParentID = null;
            ParentID = 0;
        }
    }
}