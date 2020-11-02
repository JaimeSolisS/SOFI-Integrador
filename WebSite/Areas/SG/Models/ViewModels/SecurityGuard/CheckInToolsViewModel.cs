using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.SG.Models
{
    public class CheckInToolsViewModel
    {
        public int TempAttachmentID;
        public string ModalID;
        public List<SecurityGuardTool> SecurityGuardToolsList;
        public List<SecurityGuardTool> AvailableToolsList;

        public CheckInToolsViewModel()
        {
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            TempAttachmentID = (int)t.TotalSeconds;
            ModalID = "mo_CheckInTools";
            SecurityGuardToolsList = new List<SecurityGuardTool>();
            AvailableToolsList = new List<SecurityGuardTool>();
        }
    }
}