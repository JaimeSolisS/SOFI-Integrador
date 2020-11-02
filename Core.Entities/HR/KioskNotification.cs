using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class KioskNotification : TableMaintenance
	{
		public int KioskNotificationID { get; set; }
		public int FacilityID { get; set; }
		public int MessageTypeID { get; set; }
		public string MessageTypeName { get; set; }
		public int ReferenceID { get; set; }
		public int ReferenceTypeID { get; set; }
		public string Title { get; set; }
		public string Message { get; set; }

	}
}
