using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class VendorUserTool : TableMaintenance
    {
        public int VendorUserToolID { get; set; }
        public int VendorUserID { get; set; }
        public string ToolName { get; set; }
        public string ToolImgPath { get; set; }
    }
}
