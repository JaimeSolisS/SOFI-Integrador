using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class UsersProcessLine : TableMaintenance
    {
        public int ProductionProcessID { get; set; }
        public string ProductionProcessName { get; set; }
        public int ProductionLineID { get; set; }
        public string ProductionLineName { get; set; }
        public int ChangeBy { get; set; }
        public bool LineAccess { get; set; }
        public string LineNumber { get; set; }
    }
}
