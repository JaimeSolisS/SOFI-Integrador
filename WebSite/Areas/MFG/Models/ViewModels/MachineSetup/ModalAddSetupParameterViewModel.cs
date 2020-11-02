using Core.Entities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.MachineSetup
{
    public class ModalAddSetupParameterViewModel
    {
        public IEnumerable<SelectListItem> ParameterList { get; set; }
        public IEnumerable<SelectListItem> UoMList { get; set; }
        public IEnumerable<SelectListItem> SectionsList { get; set; }
        public List<MachineParameters> MachineSetupParametersList { get; set; }


        public ModalAddSetupParameterViewModel()
        {
            ParameterList = new SelectList(new List<SelectListItem>());
            UoMList = new SelectList(new List<SelectListItem>());
            SectionsList = new SelectList(new List<SelectListItem>());
            MachineSetupParametersList = new List<MachineParameters>();
        }
    }
}