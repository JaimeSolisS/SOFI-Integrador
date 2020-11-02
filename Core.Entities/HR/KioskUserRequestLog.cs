using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class KioskUserRequestLog : TableMaintenance
	{
		public int RequestLogID { get; set; }
		public int RequestID { get; set; }
		public int MvtType { get; set; }
		public string MvtTypeName { get; set; }
		public string Comments { get; set; }
	}
}
