using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.HR.Models.ViewModels.KioskAdministration
{
    public class ModalReorderSectionsViewModel
    {
        public List<KioskArea> SectionList { get; set; }
        public ModalReorderSectionsViewModel()
        {
            SectionList = new List<KioskArea>();
        }
    }
}