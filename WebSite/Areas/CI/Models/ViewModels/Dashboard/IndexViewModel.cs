using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.CI.Models.ViewModels.Dashboard
{
    public class IndexViewModel
    {
        public List<DashboardArea> _ListAreas;
        public string ScreenSaverVideoPath { get; set; }
        public int ScreenSaverInterval { get; set; }
        public int ClosedWindowAfter { get; set; }
        public string BackgroundImage { get; set; }
        public List<DashboardCarouselVideos> _CarouselVideos;
        public int TotalVisits { get; set; }
        public string MonthName { get; set; }
        public int ParentID { get; set; }

        public IndexViewModel()
        {
            ParentID = 0;
            ScreenSaverVideoPath = "";
            ScreenSaverInterval = 20;
            ClosedWindowAfter = 20;
            _ListAreas = new List<DashboardArea>();
            _CarouselVideos = new List<DashboardCarouselVideos>();
            TotalVisits = 0;
            MonthName = "";
        }
    }
}