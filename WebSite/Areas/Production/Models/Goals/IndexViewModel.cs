using Core.Entities;
using Core.Entities.Production;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebSite.Areas.Production.Models.Goals
{
    public class IndexViewModel
    {
        public IEnumerable<SelectListItem> ProductionProcessList;
        public IEnumerable<SelectListItem> CatalogGoalList;
        public IEnumerable<SelectListItem> UserProcessesLine;
        public IEnumerable<SelectListItem> VAList;
        public IEnumerable<SelectListItem> DesignList;
        public IEnumerable<SelectListItem> ShiftList;
        public IEnumerable<SelectListItem> GoalsNameList;
        public string ClassProcessLine;
        public List<ProductionGoal> ProductionGoalsList;
        public ProductionGoal ProductionGoalInfo;
        public string ModalGoalHeader;
        public string ModalGoalAcceptButton;


        #region constructor
        public IndexViewModel()
        {
            ProductionProcessList = new SelectList(new List<ProductionLine>());
            CatalogGoalList = new SelectList(new List<Catalog>());
            UserProcessesLine = new SelectList(new List<Catalog>());
            VAList = new SelectList(new List<Catalog>());
            DesignList = new SelectList(new List<Catalog>());
            GoalsNameList = new SelectList(new List<Catalog>());
            ShiftList = new SelectList(new List<ShiftsMaster>());
            ClassProcessLine = "hidden";
            ProductionGoalsList = new List<ProductionGoal>();
            ProductionGoalInfo = new ProductionGoal();
            ModalGoalHeader = "";
            ModalGoalAcceptButton = "";

        }
        #endregion
    }
}