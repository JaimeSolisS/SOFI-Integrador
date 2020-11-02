using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Company : TableMaintenance
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}
