using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models.ViewModels.Attachments
{
    public class AttachmentViewModel
    {
        public IEnumerable<IAttachment> Attachments { get; set; }
        public List<Catalog> FileTypeList { get; set; }
        public bool? EnableFileType { get; set; }
        public bool? EnableDelete { get; set; }

        public int OperationTaskID { get; set; }
        public int OperationTaskDetailID { get; set; }
    }
}