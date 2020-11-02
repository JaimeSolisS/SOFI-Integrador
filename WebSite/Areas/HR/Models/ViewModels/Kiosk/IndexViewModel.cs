using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.HR.Models.ViewModels.Kiosk
{
    public class IndexViewModel
    {
        public int ParentID { get; set; }
        public string BackgroundImage { get; set; }

        public List<KioskCarouselMedia> CarouselMedia;
        public int SessionTime { get; set; }
        public int ScreenSaverInterval { get; set; }
        public string ScreenSaverVideoPath { get; set; }
        public string FacilityName { get; set; }
        public string TransitionTime { get; set; }
        public List<KioskArea> KioskAreas { get; set; }
        public List<KioskAreaDetail> KioskAreaDetails { get; set; }

        public IndexViewModel()
        {
            ParentID = 0;
            BackgroundImage = "";
            ScreenSaverVideoPath = "";
            TransitionTime = "5000";
            CarouselMedia = new List<KioskCarouselMedia>();
            KioskAreas = new List<KioskArea>();
            KioskAreaDetails = new List<KioskAreaDetail>();
            SessionTime = 20;
            ScreenSaverInterval = 20;
            FacilityName = "";
        }
    }
}