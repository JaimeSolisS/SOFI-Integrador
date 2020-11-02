using Core.Entities;
using Core.Entities.Production;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebSite.Areas.Production.Models.MoldScrap
{
    public class IndexViewModel
    {
        public IEnumerable<SelectListItem> UserProcessesLine;
        public IEnumerable<SelectListItem> ProductionProcessList;
        public IEnumerable<SelectListItem> ShiftList;
        public IEnumerable<SelectListItem> DesignList;
        public List<MoldScraps> MoldScrapsList;
        public string ClassProcessLine;
        public string ScrapDateFormat;

        #region Construnctor
        public IndexViewModel()
        {
            UserProcessesLine = new SelectList(new List<Catalog>());
            ProductionProcessList = new SelectList(new List<UsersProcessLine>());
            MoldScrapsList = new List<MoldScraps>();
            ShiftList = new SelectList(new List<ShiftsMaster>());
            DesignList = new SelectList(Enumerable.Empty<SelectListItem>());
            ClassProcessLine = "hidden";
        }
        #endregion
    }
}