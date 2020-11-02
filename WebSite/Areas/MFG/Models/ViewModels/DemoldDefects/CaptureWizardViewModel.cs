using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.MFG.Models.ViewModels.DemoldDefects
{
    public class CaptureWizardViewModel
    {
        public Core.Entities.DemoldDefects DemoldDefectEntity { get; set; }
        public List<Catalog> FamiliesList { get; set; }
        public List<Catalog> BaseCategoriesList { get; set; }
        public List<Catalog> AditionsCategoriesList { get; set; }
        public List<Catalog> SidesList { get; set; }
        public List<Defect> DefectsList { get; set; }
        public List<Catalog> DefectTypes { get; set; }
        public List<DemoldDefectDetail> DemoldDefectDetailsList { get; set; }
        public string DayCode { get; set; }
        public string ExceptionMessage { get; set; }

        public CaptureWizardViewModel()
        {
            DemoldDefectEntity = new Core.Entities.DemoldDefects();
            FamiliesList = new List<Catalog>();
            BaseCategoriesList = new List<Catalog>();
            AditionsCategoriesList = new List<Catalog>();
            SidesList = new List<Catalog>();
            DefectsList = new List<Defect>();
            DefectTypes = new List<Catalog>();
            DemoldDefectDetailsList = new List<DemoldDefectDetail>();
            DayCode = "";
            ExceptionMessage = "";
        }
    }
}