using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.SG.Models.ViewModels.SecurityGuardConfigurations
{
    public class EquipmentViewModel
    {
        public List<Equipment> EquipmentList;
        public IEnumerable<SelectListItem> EquipmentTypesList;

        public EquipmentViewModel()
        {
            EquipmentList = new List<Equipment>();
            EquipmentTypesList = new SelectList(new List<SelectListItem>());
        }
    }
}