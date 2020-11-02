using Core.Entities;
using Core.Entities.Production;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.MachineSetup
{
    public class IndexViewModel
    {
        public List<Core.Entities.MachineSetup> MachineSetupList;
        public IEnumerable<SelectListItem> MachinesList { get; set; }
        public List<Catalog> MaterialList { get; set; }
        public string EnableText { get; set; } 

        public IndexViewModel()
        {
            MachinesList = new SelectList(new List<SelectListItem>());
            MaterialList = new List<Catalog>();
            MachineSetupList = new List<Core.Entities.MachineSetup>();
            EnableText = "";
        }
    }
}