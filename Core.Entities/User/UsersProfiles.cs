using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities{
    public class UsersProfile : TableMaintenance
    {
        public int ProfileID { get; set; }
        public int ChangedBy { get; set; }
    }
}
