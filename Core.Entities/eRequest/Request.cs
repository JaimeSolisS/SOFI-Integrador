using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Request : TableMaintenance
    {
		public int RequestID { get; set; }
		public int FacilityID { get; set; }
		public string Folio { get; set; }
		public DateTime RequestDate { get; set; }
	    public int FormatID { get; set; }
		public string Concept { get; set; }
		public string ConceptName { get; set; }
		public int DepartmetID { get; set; }
		public string Specification { get; set; }
		public string SpecificationName { get; set; }
		public int CreateBy { get; set; }
		public string DepartmentName { get; set; }
		public string FormatName { get; set; }
		public string FacilityName { get; set; }
		public string Status { get; set; }
		public string StatusClass { get; set; }
		public string StatusValueID { get; set; }
		public string RequestDateFormat
		{
			get { return RequestDate.ToString("yyyy-MM-dd"); }
		}
	}
}
