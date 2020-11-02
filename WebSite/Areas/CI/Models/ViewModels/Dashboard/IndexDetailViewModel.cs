using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.CI.Models.ViewModels.Dashboard
{
    public class IndexDetailViewModel
    {
        public List<DashboardAreaDetail> _List;

        public IndexDetailViewModel()
        {
            _List = new List<DashboardAreaDetail>();
        }
    }
}