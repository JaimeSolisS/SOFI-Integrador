using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Interface
{
    public class DataInterfaceConnectionParameters : TableMaintenance
    {
        public int DataInterfaceConnectionParameterID { get; set; }
        public int DataInterfaceID { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
    }
}
