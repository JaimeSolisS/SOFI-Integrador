using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.CI.Models.ViewModels.Administration
{
    public class GeneralSettingsViewModel
    {
        public string imgTitle;
        public string imgOrientation;
        public string imgSource;
        public string ScreenSaverInterval;
        public string videoSrc;
        public string TransactionID;
        public string imgURL;
        public string videoURL;
        public string CloseWindowAfter;
        public List<DashboardCarouselVideosTmp> CarouselVideosList;

        public GeneralSettingsViewModel()
        {
            imgTitle = "";
            imgOrientation = "";
            imgSource = "";
            ScreenSaverInterval = "0";
            videoSrc = "";
            TransactionID = "";
            CloseWindowAfter = "0";
            CarouselVideosList = new List<DashboardCarouselVideosTmp>();
        }
    }
}