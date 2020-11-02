using Core.Entities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebSite.Areas.HR.Models.ViewModels.OpportunitiesProgram
{
    public class IndexViewModel
    {
        public List<OpportunitiesProgramCandidate> Candidate;
        public IEnumerable<SelectListItem> DepartmentsList;
        public IEnumerable<SelectListItem> DateTypesList;
        public IEnumerable<SelectListItem> ShiftsList;
        public IEnumerable<SelectListItem> GradesList;
        public int NotificationTypeID;
        public List<Core.Entities.OpportunitiesProgram> OpportunitiesProgramList;
        public bool AllowFullAccess;
        public bool AllowNewVacant;
        public bool AllowDiscardAccept;
        public bool AllowSendNotifications;
        public bool AllowEnableDisable;
        public bool AllowEdit;

        public IndexViewModel()
        {
            Candidate = new List<OpportunitiesProgramCandidate>();
            DepartmentsList = new SelectList(new List<SelectListItem>());
            DateTypesList = new SelectList(new List<SelectListItem>());
            ShiftsList = new SelectList(new List<SelectListItem>());
            GradesList = new SelectList(new List<SelectListItem>());
            NotificationTypeID = 0;
            OpportunitiesProgramList = new List<Core.Entities.OpportunitiesProgram>();
            AllowFullAccess = false;
            AllowNewVacant = false;
            AllowDiscardAccept = false;
            AllowSendNotifications = false;
            AllowEnableDisable = false;
            AllowEdit = false;
        }
    }
}