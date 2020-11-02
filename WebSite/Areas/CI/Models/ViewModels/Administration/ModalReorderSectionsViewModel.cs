using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.CI.Models.ViewModels.Administration
{
    public class ModalReorderSectionsViewModel
    {
        public List<DashboardArea> SectionList { get; set; }
        public ModalReorderSectionsViewModel()
        {
            SectionList = new List<DashboardArea>();
        }
    }
}