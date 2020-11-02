using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.HR.Models.ViewModels.KioskRequestAdministrator
{
    public class RequestStatusUpdateViewModel
    {
        public string StatusTypeChange { get; set; }
        public int? RequestID { get; set; }

        public RequestStatusUpdateViewModel()
        {
            StatusTypeChange = "";
            RequestID = 0;
        }
    }
}