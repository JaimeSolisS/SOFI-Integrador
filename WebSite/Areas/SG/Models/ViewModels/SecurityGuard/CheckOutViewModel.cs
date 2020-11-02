using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.SG.Models
{
    public class CheckOutViewModel
    {
        public string PersonName;
        public string CheckoutDateTime;
        public IEnumerable<SelectListItem> IdentificationsList;
        public IEnumerable<SelectListItem> PersonCheckIngTypeList;


        public CheckOutViewModel()
        {
            PersonName = "";
            CheckoutDateTime = "";
            PersonCheckIngTypeList = new SelectList(new List<SelectListItem>());
            IdentificationsList = new SelectList(new List<SelectListItem>());
        }
    }
}