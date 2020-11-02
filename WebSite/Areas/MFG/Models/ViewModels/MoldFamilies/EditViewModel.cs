using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.MoldFamilies
{
    public class EditViewModel
    {
        public string Title { get; set; }
        public bool IsEdit { get; set; }
        public MoldFamily MoldFamilyObj { get; set; }
        public IEnumerable<SelectListItem> LensTypesList { get; set; }
        public List<LenType> LensOfFamily { get; set; }
        public string AuxiliarClass { get; set; }

        public EditViewModel()
        {
            Title = "";
            IsEdit = false;
            AuxiliarClass = "table_record";
            MoldFamilyObj = new MoldFamily();
            LensTypesList = new SelectList(new List<SelectListItem>());
            LensOfFamily = new List<LenType>();
        }
    }
}