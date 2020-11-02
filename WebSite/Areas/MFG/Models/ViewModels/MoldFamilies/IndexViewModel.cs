using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Entities;

namespace WebSite.Areas.MFG.Models.ViewModels.MoldFamilies
{
    public class IndexViewModel
    {
        public IEnumerable<SelectListItem> MoldFamiliesNamesList { get; set; }
        public IEnumerable<SelectListItem> LensTypesList { get; set; }
        public List<MoldFamily> MoldFamiliesRecordsList { get; set; }
        public string Message { get; set; }

        public IndexViewModel()
        {
            MoldFamiliesNamesList = new SelectList(new List<SelectListItem>());
            LensTypesList = new SelectList(new List<SelectListItem>());
            MoldFamiliesRecordsList = new List<MoldFamily>();
            Message = "";
        }
    }
}