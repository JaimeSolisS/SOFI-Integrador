using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class RequestsGenericDetail : TableMaintenance
    {
        public int RequestGenericDetailID { get; set; }
        public int RequestID { get; set; }
        public int RequestLine { get; set; }
        public string Concept { get; set; }
        public string ConceptName { get; set; }
        public string ConceptIsVisible { get; set; }
        public string Reference1 { get; set; }
        public string Reference1Name { get; set; }
        public string Reference1IsVisible { get; set; }
        public string Reference2 { get; set; }
        public string Reference2Name { get; set; }
        public string Reference2IsVisible { get; set; }
        public string Reference3 { get; set; }
        public string Reference3Name { get; set; }
        public string Reference3IsVisible { get; set; }
        public string Reference4 { get; set; }
        public string Reference4Name { get; set; }
        public string Reference4IsVisible { get; set; }
        public string Reference5 { get; set; }
        public string Reference5Name { get; set; }
        public string Reference5IsVisible { get; set; }
        public string ColumnConcept { get; set; }
        public string ColumnReference1 { get; set; }
        public string ColumnReference2 { get; set; }
        public string ColumnReference3 { get; set; }
        public string ColumnReference4 { get; set; }
        public string ColumnReference5 { get; set; }
        public bool IsReturn { get; set; }
        public int ReturnBy { get; set; }
        public string ReturnByName { get; set; }
        public DateTime? DateReturn { get; set; }
    }
}



