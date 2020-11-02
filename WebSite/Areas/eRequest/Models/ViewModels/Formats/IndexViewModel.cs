using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.eRequest.Models.ViewModels.Formats
{
    public class IndexViewModel
    {
        public List<FormatsEntity> FormatList { get; set; }
        public IndexViewModel()
        {
            FormatList = new List<FormatsEntity>();
        }
    }
}