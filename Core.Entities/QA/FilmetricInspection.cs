using Core.Entities.QA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities{
    public class FilmetricInspection : TableMaintenance
    {
        public int FilmetricInspectionID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int SubstractID { get; set; }
        public string SubstractName { get; set; }
        public int BaseID { get; set; }
        public string BaseName { get; set; }
        public string MaterialName { get; set; }
        public decimal? AdditionID { get; set; }
        public int LineID { get; set; } 
        public string LineName { get; set; } 
        public List<FilmetricInspectionDetail> Details { get; set; }
    }
}
