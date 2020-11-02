using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.eRequest.Models.ViewModels.Formats
{
    public class Tbl_Format_LoopRuleDetailModel
    {
        public List<FormatsLoopsRulesDetail_TEMP> _ListFormatLoopsRulesDetail_Temp { get; set; }
        public int FormatID { get; set; }
        public Guid TransactionID { get; set; }

        public Tbl_Format_LoopRuleDetailModel()
        {
            _ListFormatLoopsRulesDetail_Temp = new List<FormatsLoopsRulesDetail_TEMP>();
            FormatID = 0;
        }
    }
}