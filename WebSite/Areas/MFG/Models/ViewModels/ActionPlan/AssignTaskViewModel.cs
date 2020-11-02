using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.MFG.Models.ViewModels.ActionPlan
{
    public class AssignTaskViewModel
    {
        public int OperationTaskID { get; set; }
        public List<User> UsersList;
        public string DateFormat { get; set; }
        public string SuggestedAction { get; set; }
        public string CurrentTime { get; set; }
        public DateTime CurrentDate { get; set; }
        public string Culture { get; set; }

        public AssignTaskViewModel()
        {
            UsersList = new List<User>();
            OperationTaskID = 0;
            SuggestedAction = "";
            CurrentTime = "";
            CurrentDate = new DateTime();
            Culture = "";
        }
    }
}