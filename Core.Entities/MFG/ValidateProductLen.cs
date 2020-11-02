using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ValidateProductLen : TableMaintenance
    {
        public string LenType { get; set; }
        public string Base { get; set; }
        public string Addition { get; set; }
        public string Side { get; set; }

    }
}