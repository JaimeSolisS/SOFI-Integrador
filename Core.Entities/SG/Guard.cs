using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class Guard : TableMaintenance
	{
		public int GuardID { get; set; }
		public string GuardName { get; set; }
		public string UniqueNumber { get; set; }
		public bool Enabled { get; set; }
		public int ReferenceTypeID { get; set; }

	}
}
