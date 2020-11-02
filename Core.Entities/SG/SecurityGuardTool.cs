using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class SecurityGuardTool : TableMaintenance
    {
        public int? SecurityGuardToolID { get; set; }
        public int? SecurityGuardLogID { get; set; }
        public string ToolName { get; set; }
        public string ToolImgPath { get; set; }
        public string FileName { get; set; }
        public int? ReferenceID { get; set; }
        public string ToolTypeImg { get; set; }
        public string ImgTypeClass { get; set; }
    }
}
