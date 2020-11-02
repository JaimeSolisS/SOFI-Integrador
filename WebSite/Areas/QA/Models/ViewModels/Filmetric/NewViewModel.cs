using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.QA.Models.ViewModels.Filmetric
{
    public class NewViewModel
    {
        public IEnumerable<SelectListItem> ProductList  { get; set; }
        public IEnumerable<SelectListItem> SubstractList  { get; set; }
        public IEnumerable<SelectListItem> BaseList { get; set; }
        public IEnumerable<SelectListItem> MaterialList { get; set; }
        public string DateFormat;
        public NewViewModel()
        {
            ProductList  = new SelectList(new List<SelectListItem>());
            SubstractList = new SelectList(new List<SelectListItem>());
            BaseList = new SelectList(new List<SelectListItem>());

        }
    }
}