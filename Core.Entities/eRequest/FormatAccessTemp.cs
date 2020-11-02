using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class FormatAccessTemp : TableMaintenance
	{
		public int FormatAccessTempID { get; set; }
		public int FormatAccessID { get; set; }
		public Guid TransactionID { get; set; }
		public int FormatID { get; set; }
		public int FacilityID { get; set; }
		public int ChangedBy { get; set; }
		public string FacilityName { get; set; }
		public string CreateUserName { get; set; }
	}
}