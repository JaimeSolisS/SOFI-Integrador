using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class eReqUserAccess
    {

        public bool AllowFullAccess { get; set; }
        public bool AllowCancel { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowViewPdf { get; set; }
        public bool AllowViewLog { get; set; }
        public bool AllowCreateReq { get; set; }
        public bool AllowApproveReq { get; set; }
    }
}
