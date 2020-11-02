using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class FormatsEntity : TableMaintenance
    {
		public int FormatID { get; set; }
		public string FormatName { get; set; }
		public bool Use2FA { get; set; }
		public bool DirectApproval { get; set; }
		public bool HasDetail { get; set; }
		public bool HasAttachment { get; set; }
		public int FileID { get; set; }
		public bool Enabled { get; set; }

	}
}
