using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class RequestGenericDetail_tb
    {
		public int RequestGenericDetailID { get; set; }
		public int DataTypeIDConcept { get; set; }
		public string CatalogJSONConcept { get; set; }
		public string DataTypeControlConcept { get; set; }
		public string ColumnConcept { get; set; }
		public string ConceptValue { get; set; }
		public string DataTypeControlReference1 { get; set; }
		public string ColumnReference1{ get; set; }
		public int DataTypeIDReference1{ get; set; }
		public string CatalogJSONReference1{ get; set; }
		public string Reference1Value{ get; set; }
		public string DataTypeControlReference2{ get; set; }
		public string ColumnReference2{ get; set; }
		public int DataTypeIDReference2{ get; set; }
		public string CatalogJSONReference2{ get; set; }
		public string Reference2Value{ get; set; }
		public string DataTypeControlReference3{ get; set; }
		public string ColumnReference3{ get; set; }
		public int DataTypeIDReference3{ get; set; }
		public string CatalogJSONReference3{ get; set; }
		public string Reference3Value{ get; set; }
		public string DataTypeControlReference4{ get; set; }
		public string ColumnReference4{ get; set; }
		public int DataTypeIDReference4{ get; set; }
		public string CatalogJSONReference4{ get; set; }
		public string Reference4Value{ get; set; }
		public string DataTypeControlReference5{ get; set; }
		public string ColumnReference5{ get; set; }
		public int DataTypeIDReference5{ get; set; }
		public string CatalogJSONReference5{ get; set; }
		public string Reference5Value{ get; set; }
		public int RequestLine { get; set; }
		public List<Catalog> CatalogContextConcept;
		public List<Catalog> CatalogContextReference1;
		public List<Catalog> CatalogContextReference2;
		public List<Catalog> CatalogContextReference3;
		public List<Catalog> CatalogContextReference4;
		public List<Catalog> CatalogContextReference5;
	

	}
}
