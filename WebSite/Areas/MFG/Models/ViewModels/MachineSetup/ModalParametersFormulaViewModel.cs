using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.MachineSetup
{
    public class ModalParametersFormulaViewModel
    {
        public IEnumerable<SelectListItem> ParametersList { get; set; }
        public string ParamTypeIdentifier { get; set; }
        public string Formula { get; set; }
        public bool AddFormulaToParameter { get; set; }
        public string ParameterChoosedToFormula { get; set; }
        public string CurrentRowOdParameter { get; set; }

        public ModalParametersFormulaViewModel()
        {
            ParametersList = new SelectList(new List<SelectListItem>());
            ParamTypeIdentifier = "";
            Formula = "";
            AddFormulaToParameter = true;
            ParameterChoosedToFormula = "";
            CurrentRowOdParameter = "";
        }
    }
}