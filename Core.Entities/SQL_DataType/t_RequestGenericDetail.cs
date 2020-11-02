using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.SQL_DataType
{
    public class t_RequestGenericDetail
    {
		public int RequestGenericDetailID { get; set; }
		public int RequestID { get; set; }
		public int RequestLine { get; set; }
		public string Concept { get; set; }
		public string Reference1 { get; set; }
		public string Reference2 { get; set; }
		public string Reference3 { get; set; }
		public string Reference4 { get; set; }
		public string Reference5 { get; set; }
		

	}
}
