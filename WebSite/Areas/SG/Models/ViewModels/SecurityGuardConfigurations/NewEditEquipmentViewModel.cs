using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.SG.Models.ViewModels.SecurityGuardConfigurations
{
    public class NewEditEquipmentViewModel
    {
        public IEnumerable<SelectListItem> EquipmentTypesList;
        public Equipment equipmentEntity;

        public NewEditEquipmentViewModel()
        {
            EquipmentTypesList = new SelectList(new List<SelectListItem>());
            equipmentEntity = new Equipment();
        }
    }
}