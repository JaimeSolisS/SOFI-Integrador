using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.Catalogs
{
    public class GasketModalViewModel
    {
        public int CatalogDetailID { get; set; }
        public string ValueID { get; set; }
        public IEnumerable<SelectListItem> OperationProcess { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }

        public GasketModalViewModel()
        {
            CatalogDetailID = 0;
            ValueID = "";
            OperationProcess = new SelectList(new List<SelectListItem>());
            Min = "0";
            Max = "0";
            Type = "";
            Title = "Gasket";
        }
    }
}