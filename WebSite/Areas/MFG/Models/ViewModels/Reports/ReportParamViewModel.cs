using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.MFG.Models.ViewModels.Reports
{
    public class ReportParamViewModel
    {
        public Catalog Report;

        public ReportParamViewModel()
        {
            Report = new Catalog();
        }
    }
}