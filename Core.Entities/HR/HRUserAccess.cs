using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class HRUserAccess
    {

        public bool AllowFullAccess { get; set; }
        public bool AllowAssign { get; set; }
        public bool AllowMarkDone { get; set; }
        public bool AllowCancel { get; set; }
        public bool AllowClose { get; set; }
        public bool AllowReOpen { get; set; } 


        public bool AllowNewVacant { get; set; }
        public bool AllowDiscardAccept { get; set; }
        public bool AllowSendNotifications { get; set; }
        public bool AllowEnableDisable { get; set; }
        public bool AllowEdit { get; set; }
    }
}
