using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public interface IAttachment
    {
        int FileID { get; set; }
        int? FileType { get; set; }
        string FileTypeName { get; set; }
        string FilePathName { get; set; }
        string FileName { get; set; }
        string AttachmentType { get; set; }
        int UserID { get; set; }
        string UserFullName { get; set; }
        DateTime DateLastMaint { get; set; }
        DateTime DateAdded { get; set; }
    }
}
