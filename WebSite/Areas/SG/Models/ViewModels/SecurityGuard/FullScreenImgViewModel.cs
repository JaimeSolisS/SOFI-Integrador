using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.SG.Models
{
    public class FullScreenImgViewModel
    {
        public int TempAttachmentID;
        public string Reference;

        public FullScreenImgViewModel()
        {
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            TempAttachmentID = (int)t.TotalSeconds;
            Reference = "";
        }
    }
}