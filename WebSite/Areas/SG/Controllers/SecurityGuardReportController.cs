using Core.Entities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Areas.SG.Models.SecurityGuardReport;
using WebSite.Filters;

namespace WebSite.Areas.SG.Controllers
{
    public class SecurityGuardReportController : BaseController
    {
        [SecurityFilter]
        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();
            try
            {
                model.CheckInTypesList = new SelectList(vw_CatalogService.List4Select("CheckInTypes", BaseGenericRequest, false), "ValueID", "DisplayText");
                model.CheckInPersonTypesList = new SelectList(vw_CatalogService.List4Select("PersonCheckIngType", BaseGenericRequest, false), "CatalogDetailID", "DisplayText");
                model.CompaniesList = new SelectList(FacilityService.List(null, null, null, VARG_FacilityID, VARG_UserID, VARG_CultureID), "FacilityID", "FacilityName");
                model.SecurityGuardLogList = SecurityGuardService.List(BaseGenericRequest);
                model.CurrentDate = MiscellaneousService.Facility_GetDate(VARG_FacilityID);
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex.Message;
                return View(model);
            }

            return View(model);
        }

        public ActionResult Search(string CheckInPersonTypes, string EmployeeNumber, string VehiclePlates, string CheckTypeID, string PersonName, DateTime? StartDate, DateTime? EndDate)
        {
            List<SecurityGuardLog> securityGuardLogList = new List<SecurityGuardLog>();
            string viewPath = "~/Areas/SG/Views/SecurityGuardReport/_Tbl_SecurityGuardLog.cshtml";
            try
            {
                securityGuardLogList = SecurityGuardService.List(CheckInPersonTypes, EmployeeNumber, VehiclePlates, CheckTypeID, PersonName, StartDate, EndDate, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex.Message;
                return PartialView(viewPath, securityGuardLogList);
            }
            return PartialView(viewPath, securityGuardLogList);
        }

        public ActionResult SecurityLogReport_Export(int[] ddl_CheckInPersonTypes, int? ddl_Facilities, string txt_PersonName, string ddl_DateTypes,
            DateTime? txt_StartDate, DateTime? txt_EndDate, string txt_EmpNum, string txt_Plates)
        {
            System.Data.DataSet ds = new System.Data.DataSet();

            ds = SecurityGuardService.ExportToExcel(ddl_CheckInPersonTypes, ddl_Facilities, txt_PersonName,
                ddl_DateTypes, txt_StartDate, txt_EndDate, txt_EmpNum, txt_Plates, BaseGenericRequest);

            return this.ExcelExport(ds, Resources.SG.SecurityGuard.title_ExcelReport);
        }

        public ActionResult GetSecurityGuardTools(int SecurityGuardLogID)
        {
            List<SecurityGuardTool> securityGuardToolList = new List<SecurityGuardTool>();
            string viewPath = "~/Areas/SG/Views/SecurityGuardReport/_Tbl_SecurityGuardLogTools.cshtml";
            try
            {
                securityGuardToolList = SecurityGuardToolsService.List(null, SecurityGuardLogID, BaseGenericRequest);
            }
            catch (Exception ex)
            {

            }

            return PartialView(viewPath, securityGuardToolList);
        }

    }
}