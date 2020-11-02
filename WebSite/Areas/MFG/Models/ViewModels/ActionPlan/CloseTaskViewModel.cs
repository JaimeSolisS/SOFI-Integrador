using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.MFG.Models.ViewModels.ActionPlan
{
    public class CloseTaskViewModel
    {
        public int OperationTaskID { get; set; }
        public DateTime? Date { get; set; }
        public string Machine { get; set; }
        public int Shift { get; set; }
        public string Problem { get; set; }
        public string Comments { get; set; }
        public bool? Enabled { get; set; }
        public DateTime? CloseDate { get; set; }
        public List<Core.Entities.IAttachment> AttachmentsList { get; set; }

        public int TempAttachmentID
        {
            get
            {
                TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                return (int)t.TotalSeconds;
            }
        }


        public CloseTaskViewModel()
        {
            Date = new DateTime();
            Machine = "";
            Shift = 0;
            Problem = "";
            Comments = "";
            Enabled = true;
            CloseDate = new DateTime();
            AttachmentsList = new List<Core.Entities.IAttachment>();
        }
    }
}