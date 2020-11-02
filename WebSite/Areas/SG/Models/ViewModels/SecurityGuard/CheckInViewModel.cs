using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.SG.Models
{
    public class CheckInViewModel
    {
        public int TempAttachmentID;
        public string PersonalType;
        public string IdenTificationPath;
        public string WhoVisits;
        public bool UseVehicle;
        public bool HaveTools;
        public int VendorTypeID;
        public List<Equipment> EquipmentList;
        public IEnumerable<SelectListItem> BadgesList;
        public IEnumerable<SelectListItem> IdentificationsList;
        public List<Catalog> PersonCheckIngTypeList;
        List<CheckListTemplatesDetail> QuestionsList;
        public CheckListTemplate CheckListTemplateEntity;


        public CheckInViewModel()
        {
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            TempAttachmentID = (int)t.TotalSeconds;
            PersonalType = "";
            UseVehicle = false;
            HaveTools = false;
            WhoVisits = "";
            IdenTificationPath = "/Content/img/UploadCloud.png";
            EquipmentList = new List<Equipment>();
            BadgesList = new SelectList(new List<SelectListItem>());
            IdentificationsList = new SelectList(new List<SelectListItem>());
            PersonCheckIngTypeList = new List<Catalog>();
            CheckListTemplateEntity = new CheckListTemplate();
            QuestionsList = new List<CheckListTemplatesDetail>();
        }
    }
}