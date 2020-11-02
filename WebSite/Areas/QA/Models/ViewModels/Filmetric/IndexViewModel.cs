using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.QA.Models.ViewModels.Filmetric
{
    public class IndexViewModel
    {
        public IEnumerable<SelectListItem> MaterialList { get; set; }
        public IEnumerable<SelectListItem> ProductList { get; set; }
        public IEnumerable<SelectListItem> SubstractList { get; set; }
        public IEnumerable<SelectListItem> BaseList { get; set; }
        public List<FilmetricInspection> InspectionList { get; set; }
        public string DateFormat { get; set; }
        public IndexViewModel()
        {
            MaterialList = new SelectList(new List<SelectListItem>());
            ProductList = new SelectList(new List<SelectListItem>());
            SubstractList = new SelectList(new List<SelectListItem>());
            BaseList = new SelectList(new List<SelectListItem>());
           InspectionList = new List<FilmetricInspection>();

        }
    }
    
}