using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class MoldFamily : TableMaintenance
    {
        public int MoldFamilyID { get; set; }
        public string FamilyName { get; set; }
        public int Lens { get; set; }
        public bool Enabled { get; set; }
    }
}
