using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class CheckListTemplatesDetail : TableMaintenance
	{
		public int CheckListTemplateDetailID { get; set; }
		public int CheckListTemplateID { get; set; }
		public int Seq { get; set; }
		public string Question { get; set; }
	}
}
