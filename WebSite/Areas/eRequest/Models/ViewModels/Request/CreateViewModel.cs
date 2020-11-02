using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.eRequest.Models.ViewModels.Request
{
    public class CreateViewModel
    {
        public IEnumerable<SelectListItem> FacilityList { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> FormatList { get; set; }
        public List<EntityField> EntityFields { get; set; }
        public int ReferenceID { get; set; }

        public int FormatID { get; set; }
        public FormatsEntity EntityFormat { get; set; }
        public int? RequestID { get; set; }
        public int TempAttachmentID;
        public string RequestDate { get; set; }
        public string ConceptValue { get; set; }
        public string SpecificationValue { get; set; }
        public List<FormatsMedia> FormatMediaList;
        public string Folio { get; set; }
        public string Btn_Save_ID;
        public bool ViewReadOnly { get; set; }
        public string disabled { get { if (ViewReadOnly) { return "disabled"; } return string.Empty; } }
        public int DepartmentID;
        public CreateViewModel()
        {
            FacilityList = new SelectList(Enumerable.Empty<SelectListItem>());
            DepartmentList = new SelectList(Enumerable.Empty<SelectListItem>());
            FormatList = new SelectList(Enumerable.Empty<SelectListItem>());
            EntityFormat = new FormatsEntity();
            FormatMediaList = new List<FormatsMedia>();
            RequestDate = "";
            Btn_Save_ID = "";
            ConceptValue = "";
            SpecificationValue = "";
            Folio = "";
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            TempAttachmentID = (int)t.TotalSeconds;
            RequestID = 0;
			FormatID = 0;			DepartmentID = 0;        }
    }
}