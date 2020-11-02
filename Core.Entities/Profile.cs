using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Profile : TableMaintenance
    {
        public int? ProfileID { get; set; }
        public string ProfileName { get; set; }
        public int? OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public int? DefaultMenuID { get; set; }
        public bool? ProductionProcessRequired { get; set; }
        public int RowNumber { get; set; }
    }
}
