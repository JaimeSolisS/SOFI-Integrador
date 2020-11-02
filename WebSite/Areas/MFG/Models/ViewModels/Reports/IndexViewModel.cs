using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.MFG.Models.ViewModels.Reports
{
    public class IndexViewModel
    {
        public List<Catalog> ReportsList;

        public IndexViewModel()
        {
            ReportsList = new List<Catalog>();
        }
    }
}