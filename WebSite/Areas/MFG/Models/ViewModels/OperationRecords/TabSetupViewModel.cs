using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.MFG.Models.ViewModels.OperationRecords
{
    public class TabSetupViewModel
    {
        public OperationSetup Setup { get; set; }
        public List<Catalog> SectionsList { get; set; }
        public TabSetupViewModel()
        {
            Setup = new OperationSetup();
            SectionsList = new List<Catalog>();
        }
    }
}