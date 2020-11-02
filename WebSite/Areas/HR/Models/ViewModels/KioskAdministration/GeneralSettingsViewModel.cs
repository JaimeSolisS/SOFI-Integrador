using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.HR.Models.ViewModels.KioskAdministration { 
    public class GeneralSettingsViewModel
    {
        public string ScreenSaverInterval;
        public string SessionTime;
        public string TransitionTime;
        public string imgURL;
        public List<KioskCarouselMediaTmp> CarouselMediaList;
        public int TempAttachmentID;
        public int KioskCarouselMediaID;

        public GeneralSettingsViewModel()
        {
            ScreenSaverInterval = "0";
            SessionTime = "0";
            TransitionTime = "0";
            imgURL = @"~/Content/img/not_found.png";
            CarouselMediaList = new List<KioskCarouselMediaTmp>();
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            TempAttachmentID = (int)t.TotalSeconds;
            KioskCarouselMediaID = 0;
        }
    }
}