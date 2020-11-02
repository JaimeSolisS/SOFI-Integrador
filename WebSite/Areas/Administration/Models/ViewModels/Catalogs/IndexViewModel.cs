using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Entities;

namespace WebSite.Areas.Administration.Models.ViewModels.Catalogs
{
    public class IndexViewModel
    {
        public bool AllowCreate;
        public bool AllowEdit;
        public IEnumerable<SelectListItem> OrganizationsList;
        public List<Catalog> _ListCatalogDetail;

        public IndexViewModel() {
            AllowCreate = false;
            AllowEdit = false;
            OrganizationsList = new SelectList(new List<Catalog>());
            _ListCatalogDetail = new List<Catalog>();
        }
    }
}