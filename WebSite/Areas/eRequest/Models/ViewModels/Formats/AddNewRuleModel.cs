using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.eRequest.Models.ViewModels.Formats
{
    public class AddNewRuleModel
    {
        public IEnumerable<SelectListItem> _ComparatorType;
        public IEnumerable<SelectListItem> _DatePart;
        public int FormatLoopRuleTempID { get; set; }
        public AddNewRuleModel()
        {
            _ComparatorType = new SelectList(new List<Catalog>());
            _DatePart = new SelectList(new List<Catalog>());
            FormatLoopRuleTempID = 0;
        }
    }
}