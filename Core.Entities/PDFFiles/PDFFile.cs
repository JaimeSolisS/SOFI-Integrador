using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class PDFFile : TableMaintenance
	{
		public int FileID { get; set; }
		public string FileName { get; set; }
		public string FullPath { get; set; }
		public int OrganizationID { get; set; }
		public int CompanyID { get; set; }
		public int FacilityID { get; set; }
		public bool Enabled { get; set; }
	}
}

