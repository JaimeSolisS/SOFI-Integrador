using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public partial class WMS_Attachment : IAttachment
    {
        public int FileID { get; set; }
        public int ReferenceID { get; set; }
        public int ReferenceType { get; set; }
        public Nullable<int> FileType { get; set; }
        public string FileTypeName { get; set; }
        public string FilePathName { get; set; }
        public string FileName { get; set; }
        public string AttachmentType { get; set; }
        public int UserID { get; set; }
        public string UserFullName { get; set; }
        public DateTime DateLastMaint { get; set; }
        public DateTime DateAdded { get; set; }
    }
}
