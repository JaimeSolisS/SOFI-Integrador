using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class PDFFilesDetail_TEMP : TableMaintenance
	{
		public int FileDetailTempID { get; set; }
		public int FileDetailID { get; set; }
		public Guid TransactionID { get; set; }
		public int FileID { get; set; }
		public string FieldName { get; set; }
		public string FieldType { get; set; }
		public int? ValueToShowID { get; set; }
		public int? ShowWhenID { get; set; }
		public string ShowWhenValue { get; set; }
		public string FontName { get; set; }
		public decimal? FontSize { get; set; }
		public string FontColor { get; set; }
		public int PositionX { get; set; }
		public int PositionY { get; set; }
		public int ImageWidth { get; set; }
		public int ImageHeight { get; set; }
	}
}
