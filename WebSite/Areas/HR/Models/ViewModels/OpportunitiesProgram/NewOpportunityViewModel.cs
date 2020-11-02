using Core.Entities;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebSite.Areas.HR.Models.ViewModels.OpportunitiesProgram
{
    public class NewEditOpportunityViewModel
    {
        public string Title;
        public int? OpportunityProgramID;
        public string VacantName;
        public string ExpirationDate;
        public string CurrentUserName;
        public List<OpportunitiesProgram_Responsable> ResponsibleList;
        public List<Catalog> DescriptionTypesList;
        public string Btn_Save_ID;
        public int NotificationTypeID;
        public IEnumerable<SelectListItem> DepartmentsList;
        public IEnumerable<SelectListItem> FacilitiesList;
        public IEnumerable<SelectListItem> ShiftsList;
        public IEnumerable<SelectListItem> GradesList;
        public List<OpportunitiesProgramMedia> OpportunityMediaList;
        public int TempAttachmentID;
        public int OppotunityProgramMediaID;
        public int DescriptionTypeID;
        public string Comments;
        public NewEditOpportunityViewModel()
        {
            VacantName = "";
            DepartmentsList = new SelectList(new List<SelectListItem>());
            FacilitiesList = new SelectList(new List<SelectListItem>());
            ExpirationDate = "";
            CurrentUserName = "";
            ResponsibleList = new List<OpportunitiesProgram_Responsable>();
            Btn_Save_ID = "";
            NotificationTypeID = 0;
            OpportunityProgramID = 0;
            DescriptionTypesList = new List<Catalog>();
            ShiftsList = new SelectList(new List<SelectListItem>());
            GradesList = new SelectList(new List<SelectListItem>());
            OpportunityMediaList = new List<OpportunitiesProgramMedia>();
            OppotunityProgramMediaID = 0;
            DescriptionTypeID = 0;
            Title = Resources.HR.Kiosk.lbl_NewOpportunity;
            Comments = "";
            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            TempAttachmentID = (int)t.TotalSeconds;
        }
    }
}