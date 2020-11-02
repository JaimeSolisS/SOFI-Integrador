using Core.Entities;
using Core.Entities.SQL_DataType;
using Core.Entities.Utilities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Areas.Administration.Models.ViewModels.Users;
using WebSite.Models;
using static WebSite.Models.StaticModels;

namespace WebSite.Areas.Administration.Controllers
{
    public class UsersController : BaseController
    {
        // GET: Administration/Users
        public ActionResult Index()
        {

            var model = new IndexViewModel();

            try
            {
                // Listados para los combos
                var Profiles = ProfileService.List(new Profile { }, BaseGenericRequest, true);
                var Departments = DepartmentService.List4Select(BaseGenericRequest);
                var Shifts = ShiftService.List4Select(BaseGenericRequest, Resources.Common.TagAll);

                model.ProfilesList = new SelectList(Profiles, "ProfileID", "ProfileName");
                model.DepartmentsList = new SelectList(Departments, "DepartmentID", "DepartmentName");
                model.ShiftsList = new SelectList(Shifts, "ShiftID", "shiftDescription");
                // Listado principal
                model._List_Users = UserService.List(null, null, null, null, null, null, VARG_FacilityID, VARG_FacilityID, VARG_UserID, VARG_CultureID);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
                return View(model);
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }

            return View(model);
        }

        public ActionResult Search(int ddl_F_Profile, string txt_F_UserAccount, string txt_F_eMail, int ddl_F_Department, string txt_F_EmployeeNumber, int ddl_F_Shift)
        {
            var model = UserService.List(ddl_F_Profile, txt_F_UserAccount, txt_F_eMail, ddl_F_Department, txt_F_EmployeeNumber, ddl_F_Shift, VARG_FacilityID, VARG_FacilityID, VARG_UserID, VARG_CultureID);

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Areas/Administration/Views/Users/_Tbl_Users.cshtml", model);
            }
            return View("~/Areas/Administration/Views/Users/_Tbl_Users.cshtml", model);
        }

        [HttpPost]
        public ActionResult SearchOnAD(string txt_F_UserAccount)
        {

            string Email = "";
            string FirstName = "";
            string LastName = "";
            try
            {
                bool ExistsUser = false;
                // Listados para los combos
                var SOFIDomainsList = vw_CatalogService.List4Select("SOFIDomains", BaseGenericRequest, true);


                if (SOFIDomainsList != null && SOFIDomainsList.Count > 0)
                {
                    foreach (Catalog item in SOFIDomainsList)
                    {
                        string URI = string.Format("{0}://{1}", "LDAP", item.DisplayText);
                        string ldapFilter = "(samAccountName=" + txt_F_UserAccount + ")";
                        DirectoryEntry Entry = new DirectoryEntry(URI);
                        DirectorySearcher Searcher = new DirectorySearcher(Entry, ldapFilter);


                        //foreach (SearchResult result in Searcher.FindAll())
                        //{
                        //    DirectoryEntry de = result.GetDirectoryEntry();
                        //    var args = de.Properties["name"].Value.ToString().Split('.');
                        //    FirstName = args[0];
                        //    if (args.Length > 2){
                        //        LastName = args[1];
                        //    }
                        //    Email = de.Properties["userprincipalname"].Value.ToString();
                        //    ExistsUser = true;
                        //}


                        if (ExistsUser) { break; }
                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return Json(new
            {
                Email = Email,
                FirstName = FirstName,
                eMail = LastName,
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Disable(int UserID)
        {
            GenericReturn result = UserService.Delete(UserID, BaseGenericRequest.FacilityID, BaseGenericRequest.UserID, BaseGenericRequest.CultureID);
            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(int UserID, int CompanyID, int FacilityID)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = UserFacilityService.Delete(UserID, CompanyID, FacilityID, null, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            var model = new CreateViewModel();

            try
            {
                // Listados para los combos
                var Profiles = ProfileService.List(new Profile { }, BaseGenericRequest, true);
                var Departments = DepartmentService.List4Select(BaseGenericRequest);
                var Shifts = ShiftService.List4Select(BaseGenericRequest, Resources.Common.TagAll);
                var Cultures = vw_CatalogService.List4Select("SystemCultures", BaseGenericRequest, true);

                model.ProfilesList = new SelectList(Profiles, "ProfileID", "ProfileName");
                model.DepartmentsList = new SelectList(Departments, "DepartmentID", "DepartmentName");
                model.ShiftsList = new SelectList(Shifts, "ShiftID", "shiftDescription");
                model.CulturesList = new SelectList(Cultures, "ValueID", "DisplayText");
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
                if (Request.IsAjaxRequest()) { return PartialView(model); }
                return View(model);
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(string UserAccountID, string eMail, string EmployeeNumber,
                                   string FirstName, string LastName, string ProfileID, int DepartmentID, int ShiftID, string DefaultCultureID, string ProfileArrayID)
        {
            int ErrorCode = 0, ID = 0;
            string ErrorMessage = "";

            if (!string.IsNullOrEmpty(ProfileArrayID))
            {
                ProfileArrayID = ProfileArrayID.TrimEnd(',');
            }
            User user = new User()
            {
                eMail = eMail,
                UserAccountID = UserAccountID,
                EmployeeNumber = EmployeeNumber,
                FirstName = FirstName,
                LastName = LastName,
                Enabled = true,
                DepartmentID = DepartmentID,
                ShiftID = ShiftID,
                DefaultCultureID = DefaultCultureID,
                BaseFacilityID = VARG_FacilityID,
                FacilityID = VARG_FacilityID,
                ChangedBy = VARG_UserID,
                CultureID = VARG_CultureID,
                ProfileArrayIds = ProfileArrayID
            };

            var result = UserService.Add(user);

            ID = result.ID;
            ErrorCode = result.ErrorCode;
            ErrorMessage = result.ErrorMessage;

            return Json(new
            {
                ErrorCode,
                ErrorMessage,
                Title = "",
                notifyType = ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                ID
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int UserID)
        {
            var model = new EditViewModel();
            try
            {
                model._User = UserService.Get(UserID, BaseGenericRequest);
                List<ProductionLine> ProductionLineList = new List<ProductionLine>();

                // Listados de accesos
                var UserFacilitiesList = UserFacilityService.List(UserID, null, VARG_FacilityID, VARG_UserID, VARG_CultureID);
                var Organizations = OrganizationService.List4Config(BaseGenericRequest);
                var Companies = CompaniesService.ListUserAccess(BaseGenericRequest);

                if (Organizations != null) { Organizations.Insert(0, new Organization() { OrganizationID = -1, OrganizationName = "" }); }
                model.OrganizationsList = new SelectList(Organizations, "OrganizationID", "OrganizationName");

                model.SelectedProfiles = model._User.ProfileArrayIds.Split(',').Select(Int32.Parse).ToList(); ;//model._User.ProfileArrayIds;
                model.ProfilesList = new SelectList(ProfileService.List(new Profile { }, BaseGenericRequest), "ProfileID", "ProfileName");




                model.CompaniesList = new SelectList(Companies, "CompanyID", "CompanyName");
                model.CompaniesPlantsTable = UserFacilitiesList;
                foreach (var item in UserFacilitiesList)
                {
                    ProductionLineList.AddRange(ProductionLineService.List(item.FacilityID.Value, BaseGenericRequest));
                }
                model.ProductionLinesList = ProductionLineList;
                model.ProductionLinesUserTable = UsersProcessesLinesService.List(null, null, UserID, null, BaseGenericRequest);

            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            // Retorno
            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }
            return View(model);
        }

        public ActionResult UpdateEditable(string name, int pk, string value)
        {
            var result = UserService.QuickUpdate(pk, name, value, VARG_FacilityID, VARG_UserID, VARG_CultureID);
            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateEditableArray(string name, int pk, string[] value)
        {
            GenericReturn result = new GenericReturn();
            var NewValue = "";
            if(value != null)
            {
                NewValue = string.Join<string>(",", value);
            }
            result = UserService.ProfilesUpdate(pk, NewValue, BaseGenericRequest);

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                Title = "",
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateUserLines(int UserID, List<t_GenericItem> SelectedIDs)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = UsersProcessesLinesService.Update(SelectedIDs, UserID, BaseGenericRequest);

                return Json(new
                {
                    result.ErrorCode,
                    result.ErrorMessage,
                    Title = "",
                    notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                    result.ID
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new
                {
                    ErrorCode = 99,
                    ErrorMessage = e.Message,
                    notifyType = NotifyType.error.ToString()
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateProfiles(int UserID, string SelectedIDs)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = UserService.ProfilesUpdate(UserID, SelectedIDs, BaseGenericRequest);

                return Json(new
                {
                    result.ErrorCode,
                    result.ErrorMessage,
                    Title = "",
                    notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                    result.ID
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json(new
                {
                    ErrorCode = 99,
                    ErrorMessage = e.Message,
                    notifyType = NotifyType.error.ToString()
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ProfilesList()
        {
            var list = ProfileService.List(new Profile { }, BaseGenericRequest);

            return Json(list.Select(s => new { value = s.ProfileID, text = s.ProfileName }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShitftsList()
        {
            var list = ShiftService.List(null, true, BaseGenericRequest);

            return Json(list.Select(s => new { value = s.ShiftID, text = s.ShiftDescription }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FacilitiesList()
        {
            var list = UserService.GetFacilities(VARG_UserID, VARG_FacilityID, VARG_CultureID);

            return Json(list.Select(s => new { value = s.FacilityID, text = s.FacilityName }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CulturesList()
        {
            var list = vw_CatalogService.List4Select("SystemCultures", BaseGenericRequest);

            return Json(list.Select(s => new { value = s.ValueID, text = s.DisplayText }), JsonRequestBehavior.AllowGet);
        }
        public ActionResult JobsPositionList()
        {
            var list = vw_CatalogService.List4Select("eReq_JobsPositions", BaseGenericRequest);

            return Json(list.Select(s => new { value = s.CatalogDetailID, text = s.DisplayText }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DepartmentsList()
        {
            var Departments = DepartmentService.List4Select(BaseGenericRequest);

            var list = new SelectList(Departments, "DepartmentID", "DepartmentName");

            return Json(Departments.Select(s => new { value = s.DepartmentID, text = s.DepartmentName }), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetProductionLines(int FacilityID)
        {
            GenericReturn result = new GenericReturn();
            List<ProductionLine> list = new List<ProductionLine>();

            try
            {
                list = ProductionLineService.List(FacilityID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString(),
                list
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveUserLine(int? UserID, int? ProductionProcessID, int? ProductionLineID)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = UsersProcessesLinesService.Insert(UserID, ProductionProcessID, ProductionLineID, null, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteUserLine(int? UserID, int? ProductionLineID)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = UsersProcessesLinesService.Delete(UserID, ProductionLineID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveUserFacility(int UserID, int CompanyID, int FacilityID)
        {
            GenericReturn result = new GenericReturn();
            UserFacility uf = new UserFacility();
            uf.UserID = UserID;
            uf.CompanyID = CompanyID;
            uf.FacilityID = FacilityID;
            uf.OrganizationID = 0;

            try
            {
                result = UserFacilityService.Add(uf, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult SaveProductionLines(int UserID, int ProductionProcessID, int ProductionLineID)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                //result =  UsersProcessesLinesService.
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult LoadFacilities(int CompanyID)
        {
            GenericReturn result = new GenericReturn();
            List<Facility> faciliesList = new List<Facility>();
            try
            {
                faciliesList = FacilityService.List(CompanyID, null, null, BaseGenericRequest.FacilityID, BaseGenericRequest.UserID, BaseGenericRequest.CultureID);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString(),
                faciliesList
            }, JsonRequestBehavior.AllowGet);

        }

    }
}