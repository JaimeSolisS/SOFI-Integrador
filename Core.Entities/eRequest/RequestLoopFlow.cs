using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class RequestLoopFlow : TableMaintenance
    {
		public int RequestLoopFlowID { get; set; }
		public int RequestID { get; set; }
		public int FormatLoopRuleID { get; set; }
		public int Seq { get; set; }
		public int PhaseID { get; set; }
		public bool IsCompleted { get; set; }
		public int ApprovalStatusID { get; set; }
		public string Status { get; set; }
		public string StatusClass { get; set; }
		public string StatusValueID { get; set; }
		public string PhaseName { get; set; }

	}
}
