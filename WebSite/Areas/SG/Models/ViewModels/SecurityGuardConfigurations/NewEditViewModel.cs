using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.SG.Models.ViewModels.SecurityGuardConfigurations
{
    public class NewEditViewModel
    {
        public VendorUser VendorUserEntity;
        public Vendor VendorEntity;
        public string ModalTitle;
        public string Type;

        public NewEditViewModel()
        {
            Type = "New";
            ModalTitle = "";
            VendorUserEntity = new VendorUser();
            VendorEntity = new Vendor();
        }
    }
}