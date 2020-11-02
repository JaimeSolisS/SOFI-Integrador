using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.HR.Models.ViewModels.KioskEmployee
{
    public class LoginUserViewModel
    {
        public List<KioskUserInfo> UserInfoList;
        public KioskEmployeMovements EmployeMovements;
        public string BackgroundImage;
        public string FacilityName;
        public int SessionTime;
        public int SessionTimeWarning;
        public string EmployeeID;
        public int NotificationsUnreaded;
        public int PrePayRollTotalHours;
        public int PaymentReceiptsList;
        public int AvailablePoints;
        public int UserAccessID;

        public LoginUserViewModel()
        {
            UserInfoList = new List<KioskUserInfo>();
            EmployeMovements = new KioskEmployeMovements();
            BackgroundImage = "";
            FacilityName = "N/A";
            SessionTime = 15;
            SessionTimeWarning = 15;
            EmployeeID = "";
            NotificationsUnreaded = 0;
            PrePayRollTotalHours = 0;
            PaymentReceiptsList = 0;
            AvailablePoints = 0;
            UserAccessID = 0;
        }
    }
}