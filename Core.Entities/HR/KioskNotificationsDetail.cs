using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class KioskNotificationsDetail
    {
        public int KioskNotificationDetailID { get; set; }
        public int KioskNotificationID { get; set; }
        public string EmployeeUserID { get; set; }
        public bool IsReaded { get; set; }

    }
}
