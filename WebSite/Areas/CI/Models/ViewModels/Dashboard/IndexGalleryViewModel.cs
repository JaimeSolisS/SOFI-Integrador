using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.CI.Models.ViewModels.Dashboard
{
    public class IndexGalleryViewModel
    {
        public List<GenericItem> _List;
        public string DataEffectClass;

        public IndexGalleryViewModel()
        {
            _List = new List<GenericItem>();
            DataEffectClass = "";
        }
    }
}