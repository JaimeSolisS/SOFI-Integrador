using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class FormatsLoopsFlow : TableMaintenance
	{
		public int FormatLoopFlowID { get; set; }
		public int FormatLoopRuleID { get; set; }
		public int Seq { get; set; }
		public int PhaseID { get; set; }
	}
}