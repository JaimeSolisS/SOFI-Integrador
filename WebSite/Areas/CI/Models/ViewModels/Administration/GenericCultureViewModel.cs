using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.CI.Models.ViewModels.Administration
{
    public class GenericCultureViewModel
    {
        public string ModelHeader;
        public List<DashboardArea> FieldTranslates;
        public int ID;

        public GenericCultureViewModel()
        {
            FieldTranslates = new List<DashboardArea>();
            ID = 0;
        }
    }
}