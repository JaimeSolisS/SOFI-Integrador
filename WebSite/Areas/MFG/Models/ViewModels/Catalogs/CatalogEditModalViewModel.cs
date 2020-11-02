using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.Catalogs
{
    public class CatalogEditModalViewModel
    {
        public int CatalogDetailID { get; set; }
        public string ValueID { get; set; }
        public string DisplayText { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }


        public CatalogEditModalViewModel()
        {
            CatalogDetailID = 0;
            ValueID = "";
            DisplayText = "";
            Param1 = "";
            Param2 = "";
            Type = "";
            Title = "";
        }
    }
}