using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class FormatsLoopsRulesTemp : TableMaintenance
	{
		public int FormatLoopRuleTempID { get; set; }
		public int FormatLoopRuleID { get; set; }
		public Guid TransactionID { get; set; }
		public int FormatLoopMasterID { get; set; }
		public string Description { get; set; }
	}
}