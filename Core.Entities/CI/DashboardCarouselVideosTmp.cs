using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class DashboardCarouselVideosTmp : TableMaintenance
    {
        public Guid TransactionID { get; set; }
        public int FileID { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public int Seq { get; set; }
        public string VirtualPath { get; set; }

    }
}
