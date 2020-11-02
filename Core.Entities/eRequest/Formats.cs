using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Formats : TableMaintenance
    {
		public int FormatID { get; set; }
		public string FormatName { get; set; }
		public bool Use2FA { get; set; }
		public bool DirectApproval { get; set; }
		public bool HasDetail { get; set; }
		public string FilePath { get; set; }

	}
}
