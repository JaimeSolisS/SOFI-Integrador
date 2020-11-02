using Core.Entities;
using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Web.Mvc;

namespace WebSite.Areas.HR.Models.ViewModels.KioskRequestAdministrator
{
    public class IndexViewModel
    {
        public List<KioskUserRequest> RequestsList;
        public int PageCount;
        public bool? OnlyRead;
        public string ClaveEmpleado;
        public int RowsPerPage;
        public int RequestHistoryDays;
        public DateTime CurrentDate;
        public string DefaultStatus;
        public string PostedRequestNumber;
        public IEnumerable<SelectListItem> RequestTypesList { get; set; }
        public IEnumerable<SelectListItem> RequestStatusList { get; set; }
        public List<Catalog> RequestStatusNormalList { get; set; }
        public IEnumerable<SelectListItem> RequestDepartmentsList { get; set; }
        public IEnumerable<SelectListItem> UserFacilitiesList { get; set; }
        public IEnumerable<SelectListItem> ShiftsList { get; set; }
        public List<string> ResponsiblesList { get; set; }
        public bool AllowViewAllRequests { get; set; }
        public bool AllowAssign { get; set; }
        public bool AllowMarkDone { get; set; }
        public bool AllowCancel { get; set; }
        public bool AllowClose { get; set; }
        public bool AllowReOpen { get; set; }
        public IndexViewModel()
        {
            RequestsList = new List<KioskUserRequest>();
            RequestTypesList = new SelectList(new List<SelectListItem>());
            RequestStatusList = new SelectList(new List<SelectListItem>());
            RequestStatusNormalList = new List<Catalog>();
            RequestDepartmentsList = new SelectList(new List<SelectListItem>());
            UserFacilitiesList = new SelectList(new List<SelectListItem>());
            ShiftsList = new SelectList(new List<SelectListItem>());
            DefaultStatus = "Assigned,Open";
            PageCount = 0;
            ClaveEmpleado = "";
            ResponsiblesList = new List<string>();
            RowsPerPage = 10;
            AllowViewAllRequests = false;
            AllowAssign = false;
            AllowMarkDone = false;
            AllowCancel = false;
            AllowClose = false;
            AllowReOpen = false;
            OnlyRead = false;
            RequestHistoryDays = 7;
            CurrentDate = DateTime.Now;
            PostedRequestNumber = null;
        }
    }
}