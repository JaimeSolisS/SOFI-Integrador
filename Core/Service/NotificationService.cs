namespace Core.Service
{
    #region namespaces

    using Data;
    using Entities;
    using System.Collections.Generic;
    using System.Data;
    using Core.Entities.Utilities;

    #endregion


    public static class NotificationService
    {
        private static NotificationRepository _rep;

        static NotificationService()
        {
            _rep = new NotificationRepository();
        }

        #region Methods

        public static List<Notification> NotificationsPendingToSendList()
        {
            using (DataTable dt = _rep.NotificationsPendingToSendList())
            {
                List<Notification> _list = dt.ConvertToList<Notification>();
                return _list;
            }
        }

        public static GenericReturn UpdateNotificationsStatus(int? Id, bool? Sent, string CultureId)
        {
            return _rep.UpdateNotificationsStatus(Id, Sent, CultureId);
        }

        public static GenericReturn UpdateNotificationsTries(int? Id, string ErrorMessageToSave, string CultureId)
        {
            return _rep.UpdateNotificationsTries(Id, ErrorMessageToSave, CultureId);
        }

        #endregion
    }
}
