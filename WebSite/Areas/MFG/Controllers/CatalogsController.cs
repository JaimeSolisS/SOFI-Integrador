using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Service;
using System.Web.Mvc;
using WebSite.Areas.MFG.Models.ViewModels.Catalogs;
using Core.Entities;
using WebSite.Models;
using WebSite.Areas.MFG.Models.ViewModels.MachineSetup;

namespace WebSite.Areas.MFG.Controllers
{
    public class CatalogsController : BaseController
    {

        public ActionResult Index()
        {
            CatalogsViewModel model = new CatalogsViewModel();
            model.CatalogsList = MFG_CatalogsService.List(BaseGenericRequest);
            return View(model);
        }

        public ActionResult GetTableOfCatalog(string ReferenceID, int? CatalogID)
        {
            CatalogTableViewModel model = new CatalogTableViewModel();
            var viewPath = "";
            try
            {
                switch (ReferenceID)
                {
                    case "MFG.Machines":
                        model.MachinesList = MFG_CatalogsService.TableForMachines(ReferenceID, BaseGenericRequest);
                        model.ProductionLinesList = new SelectList(ProductionLineService.List(VARG_FacilityID, BaseGenericRequest), "ProductionLineID", "LineNumber");
                        viewPath = "~/Areas/MFG/Views/Catalogs/_Tbl_Machines.cshtml";
                        break;
                    case "MFG.MachineParameters":
                        model.ParameterTypesList = new SelectList(vw_CatalogService.List4Select("MachineParameterTypes", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
                        model.MachinesParametersList = MFG_CatalogsService.TableForMachineParameters(ReferenceID, BaseGenericRequest);
                        viewPath = "~/Areas/MFG/Views/Catalogs/_Tbl_MachineParameters.cshtml";
                        break;
                    case "dbo.Gaskets":
                        model.CatalogList = MFG_CatalogsService.TableForCatalog(ReferenceID, CatalogID, BaseGenericRequest);
                        model.OperationProcessList = new SelectList(vw_CatalogService.List4Select("ProductionProcess", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
                        viewPath = "~/Areas/MFG/Views/Catalogs/_Tbl_Gaskets.cshtml";
                        break;
                    case "dbo.Downtimes":
                        model.CatalogList = MFG_CatalogsService.TableForCatalog(CatalogID, BaseGenericRequest);
                        viewPath = "~/Areas/MFG/Views/Catalogs/_Tbl_Downtimes.cshtml";
                        break;
                    case "dbo.Defects":
                        model.CatalogList = MFG_CatalogsService.TableForCatalog(CatalogID, BaseGenericRequest);
                        viewPath = "~/Areas/MFG/Views/Catalogs/_Tbl_Defects.cshtml";
                        break;
                }
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return PartialView(viewPath, model);
        }

        [HttpPost]
        public JsonResult DeleteCatalogRow(int EntityID, string Identifier)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                switch (Identifier)
                {
                    case "Machines":
                        result = MachineService.Delete(EntityID, BaseGenericRequest);
                        break;
                    case "MachineParameters":
                        result = MachineParametersService.Delete(EntityID, BaseGenericRequest);
                        break;
                    case "Gaskets":
                        result = MFG_CatalogsService.Delete(EntityID, BaseGenericRequest);
                        break;
                    case "Catalogs":
                        result = MFG_CatalogsService.Delete(EntityID, BaseGenericRequest);
                        break;
                }
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
                result.ID
            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetModalToEditMachine(int EntityID, string Type)
        {
            GenericReturn result = new GenericReturn();
            ModalNewMachineViewModel model = new ModalNewMachineViewModel();
            string ViewPath = "~/Areas/MFG/Views/MachineSetup/_Mo_NewMachine.cshtml";
            //var Entity = MachineService.List(BaseGenericRequest).OrderBy(x => x.MachineID);
            var MachineInfo = MachineService.GetSpecificMachine(EntityID, BaseGenericRequest);
            ProductionLine ps = new ProductionLine();
            ps.Enabled = true;

            if (Type == "Edit")
                model.Title = Resources.Common.lbl_Edit;
            else
                model.Title = Resources.Common.btn_New;

            try
            {
                if (EntityID != 0)
                {
                    model.MachineName = MachineInfo.FirstOrDefault().MachineName;
                    model.MachineDescrption = MachineInfo.FirstOrDefault().MachineDescription;
                    model.ProductionLineID = MachineInfo.FirstOrDefault().ProductionLineID;
                    model.Enabled = MachineInfo.FirstOrDefault().Enabled;
                    model.MachineID = MachineInfo.FirstOrDefault().MachineID;
                    model.MachineCategoriesList = new SelectList(vw_CatalogService.List4Select("MachineDashboardCategories", BaseGenericRequest, true, Resources.Common.chsn_SelectOption), "CatalogDetailID", "DisplayText", MachineInfo.FirstOrDefault().MachineCategoryID);
                    model.ProductionLineList = new SelectList(ProductionLineService.List(ps, BaseGenericRequest), "ProductionLineID", "LineNumber", MachineInfo.FirstOrDefault().ProductionLineID);
                }
                else
                {
                    model.ProductionLineList = new SelectList(ProductionLineService.List(ps, BaseGenericRequest), "ProductionLineID", "LineNumber");
                    model.MachineCategoriesList = new SelectList(vw_CatalogService.List4Select("MachineDashboardCategories", BaseGenericRequest, true, Resources.Common.chsn_SelectOption), "CatalogDetailID", "DisplayText");
                }
                model.Type = Type;
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
                View = RenderRazorViewToString(ViewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetModalToEditMachineParameters(int EntityID, string Type)
        {
            GenericReturn result = new GenericReturn();
            ModalAddParameterViewModel model = new ModalAddParameterViewModel();
            ProductionLine ps = new ProductionLine();
            string ViewPath = "~/Areas/MFG/Views/MachineSetup/_Mo_NewParameter.cshtml";
            ps.Enabled = true;
            int? SelectedOption = 0;

            model.Title = Resources.Common.btn_New;
            model.IsEdit = false;

            try
            {
                var MachineParametersInfo = MachineParametersService.List(EntityID, BaseGenericRequest).FirstOrDefault();
                model.ParameterList = new SelectList(vw_CatalogService.List4Select("MachineParameterTypes", BaseGenericRequest, false), "CatalogDetailID", "DisplayText");
                model.CatalogsList = new SelectList(vw_CatalogService.List4Select("CatalogsForMachineParameters", BaseGenericRequest, false), "ValueID", "DisplayText");
                model.ReferenceTypesList = new SelectList(vw_CatalogService.List4Select("ParameterReferenceTypes", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
                model.ReferenceList = new SelectList(vw_CatalogService.List4Select("CatalogsForParameterReference", BaseGenericRequest, false), "CatalogDetailID", "DisplayText");
                model.Type = Type;
                model.Title = Resources.Common.btn_New;
                model.IsEdit = false;

                if (EntityID != 0)
                {
                    model.ParameterName = MachineParametersInfo.ParameterName;
                    model.ParameterType = MachineParametersInfo.ParameterTypeID;
                    model.ParameterLength = MachineParametersInfo.ParameterLength;
                    model.NumberOfDecimals = MachineParametersInfo.ParameterPrecision;
                    model.UserReference = MachineParametersInfo.UseReference;
                    model.IsCavity = MachineParametersInfo.IsCavity;
                    model.Enabled = MachineParametersInfo.Enabled;
                    model.MachineParameterID = MachineParametersInfo.MachineParameterID;
                    model.ReferenceName = MachineParametersInfo.ReferenceName == null ? "" : MachineParametersInfo.ReferenceName;
                    SelectedOption = MachineParametersInfo.ParameterTypeID;
                    model.Title = Resources.Common.lbl_Edit;
                    model.IsEdit = true;
                    model.ReferenceTypesList = new SelectList(vw_CatalogService.List4Select("ParameterReferenceTypes", BaseGenericRequest, true), "CatalogDetailID", "DisplayText", MachineParametersInfo.ReferenceTypeID);
                }
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
                View = RenderRazorViewToString(ViewPath, model),
                SelectedOption
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetModalToEditGasket(int EntityID, string Type)
        {
            GenericReturn result = new GenericReturn();
            GasketModalViewModel model = new GasketModalViewModel();
            string ViewPath = "~/Areas/MFG/Views/Catalogs/_Mo_EditGasket.cshtml";
            var GasketInfo = new CatalogDetail();
            if (EntityID != 0)
                GasketInfo = CatalogDetailService.Get(EntityID, BaseGenericRequest);
            model.Type = Type;

            try
            {
                model.OperationProcess = new SelectList(vw_CatalogService.List("ProductionProcess", BaseGenericRequest), "CatalogDetailID", "ValueID");
                if (EntityID != 0)
                {
                    model.CatalogDetailID = EntityID;
                    model.ValueID = GasketInfo.ValueID;
                    model.Min = GasketInfo.Param2;
                    model.Max = GasketInfo.Param3;
                }
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
                View = RenderRazorViewToString(ViewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetModalToEditDownTimes(int EntityID, string Type)
        {
            GenericReturn result = new GenericReturn();
            CatalogEditModalViewModel model = new CatalogEditModalViewModel();
            string ViewPath = "~/Areas/MFG/Views/Catalogs/_Mo_EditDownTimes.cshtml";
            var CatalogInfo = vw_CatalogService.Get(EntityID, BaseGenericRequest);
            model.Type = Type;
            model.Title = Resources.MFG.OperationRecords.lbl_Downtimes;
            model.Param2 = "#000000";

            try
            {
                if (EntityID != 0)
                {
                    model.CatalogDetailID = EntityID;
                    model.ValueID = CatalogInfo.ValueID;
                    model.Param2 = CatalogInfo.Param2;
                }
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
                View = RenderRazorViewToString(ViewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetModalToEditDefects(int EntityID, string Type)
        {
            GenericReturn result = new GenericReturn();
            CatalogEditModalViewModel model = new CatalogEditModalViewModel();
            string ViewPath = "~/Areas/MFG/Views/Catalogs/_Mo_EditDefects.cshtml";
            var CatalogInfo = vw_CatalogService.Get(EntityID, BaseGenericRequest);
            model.Type = Type;
            model.Title = Resources.MFG.OperationRecords.lbl_Defects;
            model.Param1 = "#000000";

            try
            {
                if (EntityID != 0)
                {
                    model.CatalogDetailID = EntityID;
                    model.ValueID = CatalogInfo.ValueID;
                    model.Param1 = CatalogInfo.Param1;
                }
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
                View = RenderRazorViewToString(ViewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateMachineParameters(int MachineParameterID, string ParameterName, int? ParameterTypeID, int? ParameterLength, int? ParameterPrecision, string ParameterTag, bool? UseReference, string ReferenceName, int? ReferenceTypeID, int? ReferenceListID, bool? IsCavity, bool? Enabled)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = MachineParametersService.Update(MachineParameterID, ParameterName, ParameterTypeID, ParameterLength, ParameterPrecision, null, UseReference, ReferenceName, ReferenceTypeID, ReferenceListID, null, IsCavity, Enabled, BaseGenericRequest);
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
        public JsonResult UpdateGaskets(int CatalogDetailID, string ValueID, string OperationProcess, string Min, string Max)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = CatalogDetailService.Update(CatalogDetailID, ValueID, OperationProcess, Min, Max, BaseGenericRequest);
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
        public JsonResult UpdateCatalog(int CatalogDetailID, string ValueID, string BackgroundColor, int DownDefectsID)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                if (DownDefectsID == 1) //downtimes
                    result = CatalogDetailService.Update(CatalogDetailID, ValueID, null, BackgroundColor, null, BaseGenericRequest);
                else //defects
                    result = CatalogDetailService.Update(CatalogDetailID, ValueID, BackgroundColor, null, null, BaseGenericRequest);

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
        public JsonResult InsertMachineParameters(string ParameterName, int? ParameterTypeID, int? ParameterLength, int? ParameterPrecision, string ParameterTag, bool? UseReference, string ReferenceName, int? ReferenceTypeID, int? ReferenceListID, bool? IsCavity, bool? Enabled)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = MachineParametersService.Insert(ParameterName, ParameterTypeID, ParameterLength, ParameterPrecision, null, UseReference, null, null, null, IsCavity, Enabled, BaseGenericRequest);
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
        public JsonResult InsertGaskets(string ValueID, string OperationProcess, string Min, string Max)
        {
            GenericReturn result = new GenericReturn();
            CatalogDetail cd = new CatalogDetail();
            cd.ValueID = ValueID;
            cd.Param1 = OperationProcess;
            cd.Param2 = Min;
            cd.Param3 = Max;
            cd.CatalogID = vw_CatalogService.List("GasketsProducts", BaseGenericRequest).FirstOrDefault().CatalogID;

            try
            {
                result = CatalogDetailService.Add(cd, BaseGenericRequest);
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
        public JsonResult InsertCatalog(int CatalogDetailID, string ValueID, string BackgroundColor, int DownDefectsID)
        {
            GenericReturn result = new GenericReturn();
            CatalogDetail cd = new CatalogDetail();
            cd.ValueID = ValueID;



            try
            {
                if (DownDefectsID == 1) //downtimes
                {
                    cd.Param1 = "";
                    cd.Param2 = BackgroundColor;
                    cd.Param3 = "NO";
                    cd.CatalogID = vw_CatalogService.List("DowntimeReasons", BaseGenericRequest).FirstOrDefault().CatalogID;
                    result = CatalogDetailService.Add(cd, BaseGenericRequest);
                }
                else //defects{
                {
                    cd.Param1 = BackgroundColor;
                    cd.Param2 = "";
                    cd.Param3 = "";
                    cd.CatalogID = vw_CatalogService.List("ProductionGasketDefects", BaseGenericRequest).FirstOrDefault().CatalogID;
                    result = CatalogDetailService.Add(cd, BaseGenericRequest);
                }

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


        #region Filtros de catalogos
        public ActionResult SearchMachine(string MachineName, string ProcessLineID)
        {
            CatalogTableViewModel model = new CatalogTableViewModel();
            var viewPath = "~/Areas/MFG/Views/Catalogs/_Tbl_Machines.cshtml";

            try
            {
                model.MachinesList = MFG_CatalogsService.TableForMachinesFiltered("MFG.Machines", MachineName, ProcessLineID, BaseGenericRequest);
                model.ProductionLinesList = new SelectList(ProductionLineService.List(VARG_FacilityID, BaseGenericRequest), "ProductionLineID", "LineNumber");

            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return PartialView(viewPath, model);
        }

        public ActionResult SearchMachineParameters(string MachineParameterName, string MachineParameterType)
        {
            CatalogTableViewModel model = new CatalogTableViewModel();
            var viewPath = "~/Areas/MFG/Views/Catalogs/_Tbl_MachineParameters.cshtml";
            try
            {
                model.MachinesParametersList = MFG_CatalogsService.TableForMachineParametersFiltered("MFG.MachineParameters", MachineParameterName, MachineParameterType, BaseGenericRequest);
                model.ParameterTypesList = new SelectList(vw_CatalogService.List4Select("MachineParameterTypes", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");

            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return PartialView(viewPath, model);
        }

        public ActionResult SearchGaskets(string ValueID, string OperationProcess)
        {
            CatalogTableViewModel model = new CatalogTableViewModel();
            var viewPath = "~/Areas/MFG/Views/Catalogs/_Tbl_Gaskets.cshtml";
            try
            {
                model.CatalogList = MFG_CatalogsService.TableForCatalogsFiltered(null, "dbo.Gaskets", ValueID, OperationProcess, BaseGenericRequest);
                model.OperationProcessList = new SelectList(vw_CatalogService.List4Select("ProductionProcess", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return PartialView(viewPath, model);
        }

        public ActionResult SearchDowntimes(string ValueID)
        {
            CatalogTableViewModel model = new CatalogTableViewModel();
            var viewPath = "~/Areas/MFG/Views/Catalogs/_Tbl_DownTimes.cshtml";
            try
            {
                model.CatalogList = MFG_CatalogsService.TableForCatalogsFiltered(null, "DowntimeReasons", ValueID, null, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return PartialView(viewPath, model);
        }

        public ActionResult SearchDefects(string ValueID)
        {
            CatalogTableViewModel model = new CatalogTableViewModel();
            var viewPath = "~/Areas/MFG/Views/Catalogs/_Tbl_Defects.cshtml";
            try
            {
                model.CatalogList = MFG_CatalogsService.TableForCatalogsFiltered(null, "ProductionGasketDefects", ValueID, null, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return PartialView(viewPath, model);
        }

        [HttpPost]
        public JsonResult SaveChangesOfEditable(int CatalogDetailID, string ColumnName, string Value)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = CatalogDetailService.QuickUpdate(CatalogDetailID, ColumnName, Value, BaseGenericRequest.FacilityID, BaseGenericRequest.UserID, BaseGenericRequest.CultureID);
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

        #endregion

        public JsonResult GetOperationProcessList()
        {
            var list = vw_CatalogService.List4Select("ProductionProcess", BaseGenericRequest, true);
            return Json(list.Select(s => new { value = s.CatalogDetailID, text = s.ValueID }), JsonRequestBehavior.AllowGet);
        }
    }
}