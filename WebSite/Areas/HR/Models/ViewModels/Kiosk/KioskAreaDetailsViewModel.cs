using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.HR.Models.ViewModels.Kiosk
{
    public class KioskAreaDetailsViewModel
    {
        public List<KioskAreaDetail> KioskAreaDetailList { get; set; }
        public int BackKioskAreaID { get; set; } 
        public int? BackParentID { get; set; }
        public KioskAreaDetailsViewModel()
        {

            KioskAreaDetailList = new List<KioskAreaDetail>();
            BackKioskAreaID = 0;
            BackParentID = null;

        }
    }
}