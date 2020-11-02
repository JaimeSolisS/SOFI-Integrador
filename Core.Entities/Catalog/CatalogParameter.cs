using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CatalogParameter : TableMaintenance
    {
        public int CatalogID { get; set; }
        public int ParamID { get; set; }
        public string ParamName { get; set; }
        public string Description { get; set; }
        public Boolean Configured { get; set; }
    }
}
