using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.HR.Models.ViewModels.KioskEmployee
{
    public class KioskCoursesViewModel
    {
        public KioskCoursesPermit InscribePermit;
        public List<KioskCourses> KioskCoursesList;
        public List<KioskCourses> KioskCoursesInscribedList;
        public bool isInscribed;
        public int AvailableCoursesCount;

        public KioskCoursesViewModel()
        {
            InscribePermit = new KioskCoursesPermit();
            KioskCoursesList = new List<KioskCourses>();
            KioskCoursesInscribedList = new List<KioskCourses>();
            isInscribed = false;
            AvailableCoursesCount = 0;
        }
    }
}