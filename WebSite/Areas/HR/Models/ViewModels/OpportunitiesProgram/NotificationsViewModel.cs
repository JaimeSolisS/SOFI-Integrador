using Core.Entities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebSite.Areas.HR.Models.ViewModels.OpportunitiesProgram
{
    public class NotificationsViewModel
    {
        public int NotificationTypeID;
        public string Btn_SendNotificationID;
        public string Btn_SaveChanges;
        public string ModalTitle;
        public string ModalAlertText;
        public bool IsAble;

        public NotificationsViewModel()
        {
            NotificationTypeID = 0;
            Btn_SendNotificationID = "";
            Btn_SaveChanges = "";
            ModalTitle = "";
            ModalAlertText = "";
            IsAble = true;
        }
    }
}