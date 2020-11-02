using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class KioskNotificationsService
    {
        private static KioskNotificationsRepository _rep;

        static KioskNotificationsService()
        {
            _rep = new KioskNotificationsRepository();
        }

        public static List<KioskNotification> List(int? KioskNotificationID, string EmployeeID, int? ReferenceID, int? ReferenceTypeID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(KioskNotificationID, EmployeeID, ReferenceID, ReferenceTypeID, request))
            {
                List<KioskNotification> _list = dt.ConvertToList<KioskNotification>();
                return _list;
            }
        }

    }
}
