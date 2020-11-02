using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.MFG.Models.ViewModels.OperationRecords
{
    public class ParameterViewModel
    {
        public string ControlName { get; set; }
        public OperationSetupParameter Parameter { get; set; }
        public IEnumerable<SelectListItem> OptionSelectList { get; set; }
        public IEnumerable<SelectListItem> ReferenceSelectList { get; set; }
        public string OperationParameterName { get; set; }
        public string isCalculated { get; set; }
        public int OperationSetupParameterID { get; set; }
        public string FunctionValue { get; set; }

        public ParameterViewModel()
        {
            OptionSelectList = new SelectList(Enumerable.Empty<SelectListItem>());
            ReferenceSelectList = new SelectList(Enumerable.Empty<SelectListItem>());
            OperationParameterName = "";
            isCalculated = "";
            OperationSetupParameterID = 0;
            FunctionValue = "";
        }
    }
}