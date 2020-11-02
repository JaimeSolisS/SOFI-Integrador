using Core.Entities;
using Core.Entities.Production;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebSite.Areas.Production.Models.Defects
{
    public class IndexViewModel
    {
        public List<Defect> DefectsList;
        public IEnumerable<SelectListItem> DefectsListCatalog;
        public IEnumerable<SelectListItem> UserProcessesLine;
        public IEnumerable<SelectListItem> ProductionProcessList;
        public IEnumerable<SelectListItem> ShiftList;
        public IEnumerable<SelectListItem> VAList;
        public IEnumerable<SelectListItem> DesignList;
        public DefectProcess DefectProcessInfo;
        public int DefectID;
        public string ClassProcessLine;
        public string ModalHeader_AddDetailTag;
        public string Modal_SaveButtonTag;


        #region constructor
        public IndexViewModel()
        {
            DefectsList = new List<Defect>();
            DefectsListCatalog = new SelectList(new List<Defect>());
            UserProcessesLine = new SelectList(new List<Catalog>());
            ProductionProcessList = new SelectList(new List<ProductionLine>());
            VAList = new SelectList(new List<Catalog>());
            ShiftList = new SelectList(new List<ShiftsMaster>());
            DesignList = new SelectList(new List<Catalog>());
            DefectProcessInfo = new DefectProcess();
            DefectID = 0;
            ClassProcessLine = "hidden";
            ModalHeader_AddDetailTag = "";
            Modal_SaveButtonTag = "";



        }
        #endregion
    }
}