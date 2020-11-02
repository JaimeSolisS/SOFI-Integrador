using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class DataInterfacelog : TableMaintenance
    {
        public int? DataInterfaceLogID { get; set; }
        public int? DataInterfaceID { get; set; }
        public string FileName { get; set; }
        public int TotalRows { get; set; }
        public string Reference { get; set; }
        public DateTime? TransactionDate { get; set; }
        public bool Completed { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
