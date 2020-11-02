using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.eRequest.Models.ViewModels.Request
{
    public class MaterialReturnViewModel
    {
        public Core.Entities.Request RequestEntity;
        public List<Core.Entities.RequestsGenericDetail> RequestDetailList;

        public MaterialReturnViewModel()
        {
            RequestEntity = new Core.Entities.Request();
            RequestDetailList = new List<Core.Entities.RequestsGenericDetail>();
        }
    }
}