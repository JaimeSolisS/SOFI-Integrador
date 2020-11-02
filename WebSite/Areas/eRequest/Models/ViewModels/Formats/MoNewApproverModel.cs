using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.eRequest.Models.ViewModels.Formats
{
    public class MoNewApproverModel
    {
        public IEnumerable<SelectListItem> _SelectListDepartments;
        public IEnumerable<SelectListItem> _SelectListConfiguracion;
        public IEnumerable<SelectListItem> _SelectListPosition;
        public IEnumerable<SelectListItem> _SelectListUoM;
        public bool IsEdit;
        public bool ApproveAfter;
        public int FormatLoopFlowTempID;
        public int FormatLoopFlowApproverTempID;
        public int UserID;
        public string UserName;
        public MoNewApproverModel()
        {
            _SelectListDepartments = new SelectList(new List<Department>());
            _SelectListConfiguracion = new SelectList(new List<CatalogDetail>());
            _SelectListPosition = new SelectList(new List<CatalogDetail>());
            _SelectListUoM = new SelectList(new List<CatalogDetail>());
            IsEdit = false;
            ApproveAfter = false;
            FormatLoopFlowTempID = 0;
            FormatLoopFlowApproverTempID = 0;
            UserID = 0;
            UserName = "";
        }
    }
}