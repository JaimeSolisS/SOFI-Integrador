using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Attachment : TableMaintenance
    {
        public int FileID { get; set; }
        public int ReferenceID { get; set; }
        public int ReferenceType { get; set; }
        public int FileType { get; set; }
        public string FilePathName { get; set; }
        public bool SentByEDI { get; set; }
    }
}
