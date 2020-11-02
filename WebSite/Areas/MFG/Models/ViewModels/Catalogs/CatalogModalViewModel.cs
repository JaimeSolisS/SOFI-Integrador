using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.MFG.Models.ViewModels.Catalogs
{
    public class CatalogModalViewModel
    {
        public string MachineName { get; set; }
        public string MachineDescription { get; set; }
        public int ProcessLineID { get; set; }

    }
}