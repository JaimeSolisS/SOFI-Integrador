using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class RequestApprovalLog : TableMaintenance
    {
		public int RequestApprovalLogID { get; set; }
		public int RequestID { get; set; }
		public int JobPositionID { get; set; }
		public string JobPositionName { get; set; }
		public int ApprovedBy { get; set; } 
		public DateTime ApprovedDate { get; set; }
		public string Commets { get; set; } 
		public int ApprovedStatusID { get; set; }
		public string ApprovedStatusName { get; set; }
	}
}
