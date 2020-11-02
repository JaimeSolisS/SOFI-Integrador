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
    public class KioskNotificationsDetailService
    {
        private static KioskNotificationsDetailRepository _rep;

        static KioskNotificationsDetailService()
        {
            _rep = new KioskNotificationsDetailRepository();
        }


        public static GenericReturn Update(int? KioskNotificationDetailID, int? KioskNotificationID, string EmployeeUserID, bool? IsReaded, GenericRequest request)
        {
            return _rep.Update(KioskNotificationDetailID, KioskNotificationID, EmployeeUserID, IsReaded, request);
        }

    }
}
