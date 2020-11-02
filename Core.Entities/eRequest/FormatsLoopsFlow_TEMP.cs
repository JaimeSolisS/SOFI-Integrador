using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class FormatsLoopsFlow_TEMP : TableMaintenance
	{
		public int FormatLoopFlowTempID { get; set; }
		public int FormatLoopFlowID { get; set; }
		public Guid TransactionID { get; set; }
		public int FormatLoopRuleTempID { get; set; }
		public int Seq { get; set; }
		public int PhaseTempID { get; set; }
		public string PhaseName { get; set; }
	}
}
