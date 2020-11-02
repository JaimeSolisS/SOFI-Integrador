using Core.Service;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Areas.SG.Models.ViewModels.SecurityGuardConfigurations;
using static WebSite.Models.StaticModels;
using Resources.SG;
using Core.Entities.Utilities;
using DocumentFormat.OpenXml.Wordprocessing;

namespace WebSite.Areas.SG.Controllers
{
    public class SecurityGuardConfigurationsController : BaseController
    {
        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();
            try
            {
                model.CompaniesList = VendorsService.List("0,1", BaseGenericRequest);
                model.CompaniesSelectList = new SelectList(VendorsService.List4Select("0,1", BaseGenericRequest, true, Resources.Common.TagAll), "VendorID", "VendorName");
            }
            catch (Exception ex)
            {
                ViewBag.Exception = ex.Message;
                return View(model);
            }
            return View(model);
        }

        public ActionResult SearchVendors(int? VendorID, string VendorName, string StatusIDs)
        {
            List<Vendor> VendorsList = new List<Vendor>();
            string viewPath = "~/Areas/SG/Views/SecurityGuardConfigurations/_Tbl_Companies.cshtml";
            try
            {
                VendorsList = VendorsService.List(VendorID, VendorName, StatusIDs, BaseGenericRequest);
            }
            catch (Exception ex)
            {

            }

            return PartialView(viewPath, VendorsList);
        }

        public ActionResult SearchGuard(string GuardName, string UniqueNumber, string StatusIDs)
        {
            List<Guard> VendorsList = new List<Guard>();
            string viewPath = "~/Areas/SG/Views/SecurityGuardConfigurations/_Tbl_Guards.cshtml";
            try
            {
                VendorsList = GuardsService.List(null, GuardName, UniqueNumber, StatusIDs, BaseGenericRequest);
            }
            catch (Exception ex)
            {

            }

            return PartialView(viewPath, VendorsList);
        }

        public ActionResult SearchBadges(string badgeNumber, string uniqueNumber, string badgeTypeIDs)
        {
            List<Badge> BadgesList = new List<Badge>();
            string viewPath = "~/Areas/SG/Views/SecurityGuardConfigurations/_Tbl_Badges.cshtml";
            try
            {
                BadgesList = BadgesService.List(null, badgeNumber, uniqueNumber, badgeTypeIDs, BaseGenericRequest);
            }
            catch (Exception ex)
            {

            }

            return PartialView(viewPath, BadgesList);
        }

        public ActionResult SearchEquipments(int? equipmentID, string serial, string equipmentName, string equipmentTypeIDs, string upc)
        {
            List<Equipment> EquipmentsList = new List<Equipment>();
            string viewPath = "~/Areas/SG/Views/SecurityGuardConfigurations/_Tbl_Equipments.cshtml";
            try
            {
                EquipmentsList = EquipmentService.List(equipmentID, serial, equipmentName, equipmentTypeIDs, upc, BaseGenericRequest);
            }
            catch (Exception ex)
            {

            }

            return PartialView(viewPath, EquipmentsList);
        }


        public ActionResult LoadEmployeesByVendor(int? VendorID, string FullName)
        {
            List<VendorUser> VendorsList = new List<VendorUser>();
            string viewPath = "~/Areas/SG/Views/SecurityGuardConfigurations/_Tbl_CompanyEmployees.cshtml";
            try
            {
                VendorsList = VendorUsersService.ListByVendor(VendorID, FullName, BaseGenericRequest);
            }
            catch (Exception ex)
            {

            }

            return PartialView(viewPath, VendorsList);
        }

        public ActionResult GetNewUserVendorModal(int VendorID, string SectionName)
        {
            NewEditViewModel model = new NewEditViewModel();
            string viewPath = "~/Areas/SG/Views/" + SectionName + "/_Mo_NewEditVendorUser.cshtml";
            try
            {
                var vendorUser = new VendorUser();
                vendorUser.Enabled = true;
                vendorUser.ExpirationDate = DateTime.Now.AddYears(1);

                model.ModalTitle = Resources.SG.SecurityGuard.lbl_AddNewVendorUser;
                model.VendorEntity = VendorsService.Get(VendorID, BaseGenericRequest);
                model.VendorUserEntity = vendorUser;
            }
            catch (Exception ex)
            {

            }

            return PartialView(viewPath, model);
        }

        public ActionResult GetEditUserVendorModal(int vendorUserID, int vendorID, string SectionName)
        {
            NewEditViewModel model = new NewEditViewModel();
            string viewPath = "~/Areas/SG/Views/" + SectionName + "/_Mo_NewEditVendorUser.cshtml";
            try
            {
                model.VendorUserEntity = VendorUsersService.Get(vendorUserID, BaseGenericRequest);
                model.VendorEntity = VendorsService.Get(vendorID, BaseGenericRequest);
                model.ModalTitle = Resources.SG.SecurityGuard.lbl_EditVendorUser;
                model.Type = "Edit";
            }
            catch (Exception ex)
            {

            }

            return PartialView(viewPath, model);
        }

        public ActionResult GetSimpleModal(string modalName)
        {
            string ViewPath = "~/Areas/SG/Views/SecurityGuardConfigurations/" + modalName + ".cshtml";

            try
            {

            }
            catch (Exception ex)
            {

            }

            return PartialView(ViewPath, "");
        }

        public ActionResult GetNewBadgeModal()
        {
            List<Catalog> BadgesTypesList = new List<Catalog>();
            string ViewPath = "~/Areas/SG/Views/SecurityGuardConfigurations/_Mo_NewBadge.cshtml";

            try
            {
                BadgesTypesList = vw_CatalogService.List4Select("SecurityBadgesTypes", BaseGenericRequest);
            }
            catch (Exception ex)
            {

            }

            return PartialView(ViewPath, BadgesTypesList);
        }

        public ActionResult GetNewEditEquipmentModal(int? equipmentID)
        {
            NewEditEquipmentViewModel model = new NewEditEquipmentViewModel();
            string ViewPath = "~/Areas/SG/Views/SecurityGuardConfigurations/_Mo_NewEditEquipment.cshtml";

            try
            {
                if(equipmentID == null)
                {
                    model.EquipmentTypesList = new SelectList(vw_CatalogService.List4Select("EquipmentTypes", BaseGenericRequest, true, Resources.Common.chsn_SelectOption), "CatalogDetailID", "DisplayText");
                }
                else
                {
                    var equipmentEntity = EquipmentService.Get(equipmentID.ToInt(), BaseGenericRequest);
                    model.equipmentEntity = equipmentEntity;
                    model.EquipmentTypesList = new SelectList(vw_CatalogService.List4Select("EquipmentTypes", BaseGenericRequest, true, Resources.Common.chsn_SelectOption), "CatalogDetailID", "DisplayText", equipmentEntity.EquipmentTypeID);
                }

            }
            catch (Exception ex)
            {

            }

            return PartialView(ViewPath, model);
        }



        [HttpPost]
        public JsonResult DeleteCompany(int vendorID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = VendorsService.Delete(vendorID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                ID = result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteBadge(int badgeID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = BadgesService.Delete(badgeID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                ID = result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteVendorUser(int vendorUserID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = VendorUsersService.Delete(vendorUserID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                ID = result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteEquipment(int equipmentID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = EquipmentService.Delete(equipmentID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                ID = result.ID
            }, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult SaveVendorUser(VendorUser vendorUser, bool isEdited)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                if (isEdited)
                {
                    result = VendorUsersService.Update(vendorUser, BaseGenericRequest);
                }
                else
                {
                    result = VendorUsersService.Insert(vendorUser, BaseGenericRequest);
                }

            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                ID = result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTabView(string tabName)
        {
            string viewPath = "";
            dynamic model = "";
            try
            {
                switch (tabName)
                {
                    case "Suppliers":
                        model = new IndexViewModel();
                        model.CompaniesList = VendorsService.List("0,1", BaseGenericRequest);
                        model.CompaniesSelectList = new SelectList(VendorsService.List4Select("0,1", BaseGenericRequest, true, Resources.Common.TagAll), "VendorID", "VendorName");
                        viewPath = "~/Areas/SG/Views/SecurityGuardConfigurations/_Tab_Suppliers.cshtml";
                        break;
                    case "Equipment":
                        model = new EquipmentViewModel();
                        model.EquipmentList = EquipmentService.List(BaseGenericRequest);
                        model.EquipmentTypesList = new SelectList(vw_CatalogService.List("EquipmentTypes", BaseGenericRequest), "CatalogDetailID", "DisplayText");
                        viewPath = "~/Areas/SG/Views/SecurityGuardConfigurations/_Tab_Equipment.cshtml";
                        break;
                    case "Guard":
                        model = GuardsService.List("0,1", BaseGenericRequest);
                        viewPath = "~/Areas/SG/Views/SecurityGuardConfigurations/_Tab_Guards.cshtml";
                        break;
                    case "Badges":
                        model = new BadgesViewModel();
                        model.BadgesList = BadgesService.List(BaseGenericRequest);
                        model.BadgesTypesList = new SelectList(vw_CatalogService.List("SecurityBadgesTypes", BaseGenericRequest), "CatalogDetailID", "DisplayText");
                        viewPath = "~/Areas/SG/Views/SecurityGuardConfigurations/_Tab_Badges.cshtml";
                        break;
                }
            }
            catch (Exception ex)
            {

            }

            return PartialView(viewPath, model);
        }

        [HttpPost]
        public JsonResult EnableDisable(int guardID, bool enable)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = GuardsService.EnableDisable(guardID, enable, BaseGenericRequest);

                if (result.ErrorCode == 0 && enable)
                {
                    result.ErrorMessage = Resources.SG.SecurityGuard.msg_SuccessGuardEnable;
                }
                else if (result.ErrorCode == 0)
                {
                    result.ErrorMessage = Resources.SG.SecurityGuard.msg_SuccessGuardDisable;
                }
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                ID = result.ID
            }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult SaveNewCompany(Vendor vendor)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = VendorsService.Insert(vendor, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                ID = result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveNewGuard(string guardName)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = GuardsService.Insert(guardName, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                ID = result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveNewBadge(string badgeNumber, int badgeTypeID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = BadgesService.Insert(badgeNumber, badgeTypeID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                ID = result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveEquipment(Equipment equipment)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                if(equipment.EquipmentID == 0)
                {
                    result = EquipmentService.Insert(equipment, BaseGenericRequest);
                }
                else
                {
                    result = EquipmentService.Update(equipment, BaseGenericRequest);
                }
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                ID = result.ID
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UpdateGuardName(int pk, string name, string value)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = GuardsService.Update(pk, value, null, null, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                ID = result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateBadgeName(int pk, string name, string value)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = BadgesService.UpdateBadgeNumber(pk, value, name, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                ID = result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateEquipment(Equipment equipment)
        {
            GenericReturn result = new GenericReturn();
            try
            {

            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                ID = result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidateGuardCode(string uniqueNumber)
        {
            GenericReturn result = new GenericReturn();
            bool IsValidGuardCode = false;
            try
            {
                IsValidGuardCode = GuardsService.ValidateGuardCode(uniqueNumber);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                IsValidGuardCode,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                ID = result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PrintLabel(int referenceID, int referenceTypeID)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = UsersPrintersService.SG_GenerateLabels(referenceID, referenceTypeID, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.ToString();
            }

            return Json(new
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                ID = result.ID
            }, JsonRequestBehavior.AllowGet);
        }

    }
}