using Core.Entities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebSite.Areas.HR.Models.ViewModels.KioskAdministration
{
    public class IndexViewModel
    {
        public IEnumerable<SelectListItem> ListAreas;
        public List<KioskArea> KioskAreaList;
        public IEnumerable<SelectListItem> FileTypeList;
        public IEnumerable<SelectListItem> ViewsList;
        public int DFT_View;
        public IndexViewModel()
        {
            ListAreas = new SelectList(new List<DashboardArea>());
            FileTypeList = new SelectList(new List<Catalog>());
            KioskAreaList = new List<KioskArea>();
            ViewsList = new SelectList(new List<Catalog>());
        }
    }
}