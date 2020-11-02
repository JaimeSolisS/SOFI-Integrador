using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Core.Entities
{
	public class CheckListTemplate : TableMaintenance
	{
		public int CheckListTemplateID { get; set; }
		public string CheckListName { get; set; }
		public bool Enabled { get; set; }
		public int OrganizationID { get; set; }
		public int FacilityID { get; set; }
	}
}
