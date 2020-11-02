using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class FormatsLoopsRulesDetail_TEMP : TableMaintenance
	{
		public int FormatLoopRuleDetailTempID { get; set; }
		public int FormatLoopRuleDetailID { get; set; }
		public Guid TransactionID { get; set; }
		public int FormatLoopRuleTempID { get; set; }
		public int FieldID { get; set; }
		public bool IsAdditionalField { get; set; }
		public string DatePartArgument { get; set; }
		public int JoinTypeID { get; set; }
		public int ComparisonOperator { get; set; }
		public decimal? Seq { get; set; }
		public string Description { get; set; }

	}
}
