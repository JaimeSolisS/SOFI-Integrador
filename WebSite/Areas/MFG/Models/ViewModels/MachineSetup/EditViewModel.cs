using Core.Entities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.MachineSetup
{
    public class EditViewModel
    {
        public int MachineSetupID { get; set; }
        public string MachineSetupName { get; set; }
        public bool? Enabled { get; set; }
        public IEnumerable<SelectListItem> MachinesList { get; set; }
        public IEnumerable<SelectListItem> MaterialList { get; set; }
        public IEnumerable<SelectListItem> ParameterList { get; set; }
        public List<MaterialSetup> MaterialSetupsList { get; set; }
        public List<MachineSetupParameters> MaterialParametersList { get; set; }
        public List<MachineSetupParameters> MachineParametersList { get; set; }
        public string NewEditSetup { get; set; }
        public string EnableTranslate { get; set; }
        public List<MachineSetupParameters> SectionWithMachineParameterList { get; set; }
        public int YesID { get; set; }

        public List<Catalog> SectionsList { get; set; }
        public IEnumerable<SelectListItem> ProductionProcessList { get; set; }

        public EditViewModel()
        {
            MachinesList = new SelectList(new List<SelectListItem>());
            MaterialList = new SelectList(new List<SelectListItem>());
            MaterialSetupsList = new List<MaterialSetup>();
            MaterialParametersList = new List<MachineSetupParameters>();
            MachineParametersList = new List<MachineSetupParameters>();
            ParameterList = new SelectList(new List<SelectList>());
            NewEditSetup = "";
            EnableTranslate = "";

            SectionsList = new List<Catalog>();
            ProductionProcessList = new SelectList(new List<SelectListItem>());
            SectionWithMachineParameterList = new List<MachineSetupParameters>();

            YesID = 0;
        }
    }
}