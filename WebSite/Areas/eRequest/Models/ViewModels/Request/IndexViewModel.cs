using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.eRequest.Models.ViewModels.Request
{
    public class IndexViewModel
    {
        public IEnumerable<SelectListItem> FacilityList;
        public IEnumerable<SelectListItem> FormatList;
        public IEnumerable<SelectListItem> DateTypeList;
        public IEnumerable<SelectListItem> DepartmentList;
        public IEnumerable<SelectListItem> StatusList;
        public IEnumerable<SelectListItem> UserFacilitiesList;
        public string RequestDefaultStatus;
        public bool AllowFullAccess;
        public bool AllowEdit;
        public bool AllowCancel;
        public bool AllowViewLog;
        public bool AllowPdf;
        public bool AllowCreateReq;
        public bool AllowApproveReq;
        public List<Core.Entities.Request> RequestList { get; set; }
        public IndexViewModel()
        {
            FacilityList = new SelectList(new List<Facility>());
            FormatList = new SelectList(new List<FormatsEntity>());
            DateTypeList = new SelectList(new List<FormatsEntity>());
            DepartmentList = new SelectList(new List<FormatsEntity>());
            StatusList = new SelectList(new List<SelectListItem>());
            UserFacilitiesList = new SelectList(new List<SelectListItem>());
            AllowFullAccess = false;
            AllowEdit = false;
            AllowCancel = false;
            AllowViewLog = false;
            AllowPdf = false;
            AllowCreateReq = false;
            AllowApproveReq = false;
            RequestDefaultStatus = "0";
        }
    }
}
