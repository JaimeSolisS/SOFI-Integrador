using Core.Entities;
using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.eRequest.Models.ViewModels.Formats
{
    public class MoNewRuleModel
    {
        public List<CatalogDetailTemp> _CatalogDetailTemp;
        public string RuleName;
        public bool IsEdit;
        public int FormatLoopRuleTempID;
        public MoNewRuleModel()
        {
            _CatalogDetailTemp = new List<CatalogDetailTemp>();
            RuleName = "";
            IsEdit = false;
            FormatLoopRuleTempID = 0;
        }
    }
}