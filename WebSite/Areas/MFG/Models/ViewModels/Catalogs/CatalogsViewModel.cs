using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.Catalogs
{
    public class CatalogsViewModel
    {
        public string CatalogTitle { get; set; }
        public List<Core.Entities.Catalogs> CatalogsList { get; set; }

        public CatalogsViewModel()
        {
            CatalogTitle = "";
            CatalogsList = new List<Core.Entities.Catalogs>();
        }
    }
}