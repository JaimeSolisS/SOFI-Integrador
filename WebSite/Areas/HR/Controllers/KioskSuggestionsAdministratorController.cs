using Core.Entities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Areas.HR.Models.ViewModels.KioskSuggestionsAdministrator;
using WebSite.Models;

namespace WebSite.Areas.HR.Controllers
{
    public class KioskSuggestionsAdministratorController : BaseController
    {
        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();

            try
            {
                model.CategoriesList = new SelectList(vw_CatalogService.List4Select("KioskCommentCategories", BaseGenericRequest, true, Resources.Common.TagAll), "CatalogDetailID", "DisplayText");
                model.SuggestionsList = KioskSuggestionsAdministratorService.List(BaseGenericRequest);
                model.SugestionsHistoryDays = Convert.ToInt32(MiscellaneousService.Param_GetValue(VARG_FacilityID, "HR_Sugestions_History_Days", "30"));
                model.FacilitiesList = new SelectList(UserService.GetFacilities(VARG_UserID, VARG_FacilityID, VARG_CultureID), "FacilityID", "FacilityName");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
            }

            return View(model);
        }

        public ActionResult Search(int CategoryID, string FacilityIDs, DateTime? StartDate, DateTime? EndDate)
        {
            string ViewPath = "~/Areas/HR/Views/KioskSuggestionsAdministrator/_Tbl_KioskSuggestions.cshtml";
            List<KioskEmployeeSuggestion> model = new List<KioskEmployeeSuggestion>();

            try
            {
                model = KioskSuggestionsAdministratorService.List(null, null, CategoryID, FacilityIDs, StartDate, EndDate, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
            }

            return PartialView(ViewPath, model);
        }

        [HttpPost]
        public ActionResult SuggestionsExportToExcel(int? ddl_CommentsCategories, int[] ddl_Facilities, DateTime? txt_SuggestionStartDate, DateTime? txt_SuggestionEndDate)
        {
            string[] TabName = { Resources.HR.Kiosk.lbl_Suggestions };
            using (DataSet ds = KioskSuggestionsAdministratorService.ListDataSet(ddl_CommentsCategories, ddl_Facilities, txt_SuggestionStartDate, txt_SuggestionEndDate, BaseGenericRequest))
            {
                return this.ExcelReportWithTabName(ds, Resources.HR.Kiosk.lbl_Suggestions, TabName);
            }

        }

        [HttpPost]
        public JsonResult Delete(int KioskEmployeeSuggestionID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = KioskSuggestionsAdministratorService.Delete(KioskEmployeeSuggestionID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = e.ToString();
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

    }
}