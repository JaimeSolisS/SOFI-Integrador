using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
	public class FormatGenericDetailTemp : TableMaintenance
	{
		public int FormatGenericDetailTempID { get; set; }
		public int FormatGenericDetailID { get; set; }
		public Guid TransactionID { get; set; }
		public int FormatID { get; set; }
		public string ColumnName { get; set; }
		public string TranslationTag { get; set; }
		public string Description { get; set; }
		public int DataTypeID { get; set; }
		public int FieldLength { get; set; }
		public int FieldPrecision { get; set; }
		public string CatalogTag { get; set; }
		public bool IsMandatory { get; set; }
		public string CssClass { get; set; }
		public string FieldIcon { get; set; }
		public bool Enabled { get; set; }
		public string DataTypeName { get; set; }
		public string ShowAs { get; set; }
		public int IsList { get; set; }
		public string CatalogNameSelect { get; set; }
		public bool IsCheck { get; set; }
	}
}
