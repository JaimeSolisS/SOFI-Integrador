using Core.Entities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.Catalogs
{
    public class CatalogTableViewModel
    {
        public List<Machine> MachinesList { get; set; }
        public List<MachineParameters> MachinesParametersList { get; set; }
        public List<Catalog> CatalogList { get; set; }
        public IEnumerable<SelectListItem> ProductionLinesList { get; set; }
        public IEnumerable<SelectListItem> ParameterTypesList { get; set; }
        public IEnumerable<SelectListItem> OperationProcessList { get; set; }


        public CatalogTableViewModel()
        {
            MachinesList = new List<Machine>();
            MachinesParametersList = new List<MachineParameters>();
            CatalogList = new List<Catalog>();
            ProductionLinesList = new SelectList(new List<SelectListItem>());
            ParameterTypesList = new SelectList(new List<SelectListItem>());
            OperationProcessList = new SelectList(new List<SelectListItem>());
        }
    }
}