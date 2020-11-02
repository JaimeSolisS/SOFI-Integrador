using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.MFG.Models.ViewModels.MachineSetup
{
    public class MachineSetupParameterViewModel
    {
        public int ParameterSectionID { get; set; }
        public List<MachineSetupParameters> MachineSetupParameterList { get; set; }

        public MachineSetupParameterViewModel()
        {
            ParameterSectionID = 0;
            MachineSetupParameterList = new List<MachineSetupParameters>();
        }
    }
}