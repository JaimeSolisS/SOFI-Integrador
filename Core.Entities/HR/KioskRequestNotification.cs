using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class KioskRequestNotification : TableMaintenance
    {
        public int RequestLogNotificationID { get; set; }
        public int RequestLogID { get; set; }
        public int RequestID { get; set; }
        public string RequestedName { get; set; }
        public string Description { get; set; }
        public string PublicPrivateSeparator { get; set; }
        public string RequestNumber { get; set; }
        public int MvtType { get; set; }
        public string MvtTypeName { get; set; }
    }
}
