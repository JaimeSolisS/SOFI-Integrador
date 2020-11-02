using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class FormatsLoopsApprovers_TEMP : TableMaintenance
	{
		public int FormatLoopApproverTempID { get; set; }
		public int FormatLoopApproverID { get; set; }
		public Guid TransactionID { get; set; }
		public int FormatLoopFlowTempID { get; set; }
		public int DepartmentOriginID { get; set; }
		public int JobPositionID { get; set; }
		public int DepartmentID { get; set; }
		public int ApproverID { get; set; }
		public int Priority { get; set; }
		public int Tolerance { get; set; }
		public int ToleranceUoM { get; set; }
		public string DepartmentOriginName { get; set; }
		public string DepartmentName { get; set; }
		public string ApproverName { get; set; }
		public string JobPositionName { get; set; }
		public string ToleranceUoMName { get; set; }
	}
}
