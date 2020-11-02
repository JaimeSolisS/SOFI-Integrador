using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.HR.Models.ViewModels.KioskAdministration
{
    public class KioskAreaUpsertViewModel
    {
        public int Sequence;
        public List<Catalog> CulturesList;
        public IEnumerable<SelectListItem> SizeList;

        public KioskAreaUpsertViewModel()
        {
            SizeList = new SelectList(new List<Catalog>());
            CulturesList = new List<Catalog>();
        }

    }
}