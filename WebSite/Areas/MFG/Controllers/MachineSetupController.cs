using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Core.Entities;
using Core.Service;
using WebSite.Areas.MFG.Models.ViewModels.MachineSetup;
using WebSite.Models;

namespace WebSite.Areas.MFG.Controllers
{
    public class MachineSetupController : BaseController
    {
        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();
            try
            {
                var materialList = vw_CatalogService.List4Select("OperationMaterials", BaseGenericRequest, false);
                materialList.Insert(0, new Catalog() { CatalogDetailID = 0, DisplayText = Resources.Common.TagAll });


                model.MachinesList = new SelectList(MachineService.List4Select(BaseGenericRequest, Resources.Common.TagAll), "MachineID", "MachineName");
                model.MaterialList = materialList;
                model.MachineSetupList = MachineSetupService.List(BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult Save(int? MachineSetupID, string MachineSetupName, bool Enabled, List<MaterialSetup> MaterialsetupList, List<MachineSetupParameters> MachineSetupParametrsList, List<MachineSetupParameters> TempListDeletedSections)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = MachineSetupService.Upsert(MachineSetupID, MachineSetupName, Enabled, MaterialsetupList, MachineSetupParametrsList, TempListDeletedSections, BaseGenericRequest);
                result.ErrorMessage = Resources.MFG.MachineSetups.msg_SuccessSaveSetup;
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

        public ActionResult New()
        {
            string viewPath = "~/Areas/MFG/Views/MachineSetup/Edit.cshtml";

            EditViewModel model = new EditViewModel();
            try
            {
                model.MachineSetupName = "";
                model.Enabled = true;
                model.MachinesList = new SelectList(MachineService.List4Select(BaseGenericRequest, Resources.Common.TagAll), "MachineID", "MachineName");
                model.MaterialList = new SelectList(vw_CatalogService.List4Select("OperationMaterials", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
                model.NewEditSetup = Resources.MFG.MachineSetups.btn_New_Setup;
                model.SectionsList = vw_CatalogService.List4Select("OperationParameterSections", BaseGenericRequest, false);
                model.ProductionProcessList = new SelectList(vw_CatalogService.List4Select("ProductionProcess", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
                model.YesID = vw_CatalogService.List4Select("YesNo", BaseGenericRequest, true).Where(x => x.ValueID == "Y").FirstOrDefault().CatalogDetailID;

            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return View(viewPath, model);
        }

        public ActionResult Edit(int MachineSetupID)
        {
            string viewPath = "~/Areas/MFG/Views/MachineSetup/Edit.cshtml";
            EditViewModel model = new EditViewModel();
            try
            {
                List<MachineSetup> machines = MachineSetupService.List(MachineSetupID, "", null, BaseGenericRequest);
                model.MachineSetupID = MachineSetupID;
                model.MachineSetupName = machines.FirstOrDefault().MachineSetupName;
                model.Enabled = machines.FirstOrDefault().Enabled;
                model.MaterialSetupsList = MaterialSetupService.List(MachineSetupID, BaseGenericRequest);
                model.MachineParametersList = MachineSetupParametersService.List(MachineSetupID, null, BaseGenericRequest);
                model.MaterialList = new SelectList(vw_CatalogService.List4Select("OperationMaterials", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
                model.NewEditSetup = Resources.MFG.MachineSetups.lbl_EditSetup;
                model.SectionsList = vw_CatalogService.List4Select("OperationParameterSections", BaseGenericRequest, false);
                model.ProductionProcessList = new SelectList(vw_CatalogService.List4Select("ProductionProcess", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
                model.SectionWithMachineParameterList = MachineSetupParametersService.List(MachineSetupID, null, BaseGenericRequest).GroupBy(x => x.ParameterSectionID).Select(x => x.First()).ToList();
                model.YesID = vw_CatalogService.List4Select("YesNo", BaseGenericRequest, true).Where(x => x.DisplayText == "Yes" || x.DisplayText == "Si").Select(x => x.CatalogDetailID).FirstOrDefault();
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return View(viewPath, model);
        }

        public ActionResult LoadMaterials(int MachineSetupID)
        {
            List<MaterialSetup> model = new List<MaterialSetup>();
            try
            {
                model = MaterialSetupService.List(MachineSetupID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return PartialView("~/Areas/MFG/Views/MachineSetup/_MachineMaterialTable.cshtml", model);
        }

        public ActionResult LoadMachineSetupParameters(int MachinesetupID, int ParametersetupID)
        {
            MachineSetupParameterViewModel model = new MachineSetupParameterViewModel();
            try
            {
                var list = vw_CatalogService.List("OperationParameterSections", BaseGenericRequest);
                model.MachineSetupParameterList.AddRange(MachineSetupParametersService.List(MachinesetupID, ParametersetupID, BaseGenericRequest));

                model.ParameterSectionID = ParametersetupID;

            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }
            return PartialView("~/Areas/MFG/Views/MachineSetup/_Tbl_MachineMaterialsParameters.cshtml", model);
        }

        public ActionResult Search(string MachineSetupName, int? MachineID, int? MaterialID)
        {
            List<MachineSetup> model = new List<MachineSetup>();
            string ViewPath = "~/Areas/MFG/Views/MachineSetup/_MachineInfoTable.cshtml";

            try
            {
                model = MachineSetupService.List(MachineSetupName, MachineID, MaterialID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return PartialView(ViewPath, model);
        }

        public JsonResult GetModalAddSetupParameter()
        {
            ModalAddSetupParameterViewModel model = new ModalAddSetupParameterViewModel();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/MFG/Views/MachineSetup/_Mo_AddSetupParameter.cshtml";

            try
            {
                model.UoMList = new SelectList(vw_CatalogService.List4Select("UoM", BaseGenericRequest, false), "CatalogDetailID", "DisplayText");
                model.SectionsList = new SelectList(vw_CatalogService.List4Select("OperationParameterSections", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
                result.ErrorCode = 0;
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

        public JsonResult GetModalNewMaterial()
        {
            ModalAddSetupParameterViewModel model = new ModalAddSetupParameterViewModel();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/MFG/Views/MachineSetup/_Mo_NewMaterial.cshtml";

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString(),
                View = RenderRazorViewToString(ViewPath, null)
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetModalNewMachine()
        {
            ModalNewMachineViewModel model = new ModalNewMachineViewModel();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/MFG/Views/MachineSetup/_Mo_NewMachine.cshtml";
            ProductionLine ps = new ProductionLine();
            ps.Enabled = true;
            model.Title = Resources.MFG.MachineSetups.lbl_NewMachine;

            try
            {
                model.ProductionLineList = new SelectList(ProductionLineService.List(ps, BaseGenericRequest), "ProductionLineID", "LineNumber");
                model.MachineCategoriesList = new SelectList(vw_CatalogService.List4Select("MachineDashboardCategories", BaseGenericRequest, true, Resources.Common.chsn_SelectOption), "CatalogDetailID", "DisplayText");
                model.MachineID = 0;
                result.ErrorCode = 0;
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

        public JsonResult GetModalNewParameter()
        {
            ModalAddParameterViewModel model = new ModalAddParameterViewModel();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/MFG/Views/MachineSetup/_Mo_NewParameter.cshtml";
            model.Title = @Resources.MFG.MachineSetups.lbl_AddSetupParameter;

            try
            {
                model.ParameterList = new SelectList(vw_CatalogService.List4Select("MachineParameterTypes", BaseGenericRequest, false), "CatalogDetailID", "DisplayText");
                model.CatalogsList = new SelectList(vw_CatalogService.List4Select("CatalogsForMachineParameters", BaseGenericRequest, false), "ValueID", "DisplayText");
                model.ReferenceTypesList = new SelectList(vw_CatalogService.List4Select("ParameterReferenceTypes", BaseGenericRequest, true), "CatalogDetailID", "DisplayText");
                model.ReferenceList = new SelectList(vw_CatalogService.List4Select("CatalogsForParameterReference", BaseGenericRequest, false), "CatalogDetailID", "DisplayText");
                model.MachineParameterID = 0;

                result.ErrorCode = 0;
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

        public JsonResult GetModalNewSection()
        {
            ModalAddSetupParameterViewModel model = new ModalAddSetupParameterViewModel();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/MFG/Views/MachineSetup/_Mo_NewSection.cshtml";

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString(),
                View = RenderRazorViewToString(ViewPath, null)
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateSection(int pk, string name, string value)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = CatalogDetailService.QuickUpdate(pk, name, value, VARG_FacilityID, VARG_UserID, VARG_CultureID);
            }
            catch (Exception e)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = e.Message;
            }
            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMachineList()
        {
            List<Machine> list = new List<Machine>();
            GenericReturn result = new GenericReturn();

            try
            {
                list = MachineService.List4Select(BaseGenericRequest, Resources.Common.TagAll, false);
                if (list != null)
                {
                    result.ErrorCode = 0;
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
                list
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMaterialList()
        {
            List<Catalog> list = new List<Catalog>();
            GenericReturn result = new GenericReturn();

            try
            {
                list = vw_CatalogService.List4Select("OperationMaterials", BaseGenericRequest, false);
                if (list != null)
                {
                    result.ErrorCode = 0;
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
                list
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetParametersList()
        {
            List<MachineParameters> list = new List<MachineParameters>();
            GenericReturn result = new GenericReturn();

            try
            {
                list = MachineParametersService.List(null, null, null, null, null, null, null, null, null, true, BaseGenericRequest);

                if (list != null)
                {
                    result.ErrorCode = 0;
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
                list
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSectionsList()
        {
            List<Catalog> list = new List<Catalog>();
            GenericReturn result = new GenericReturn();

            try
            {
                list = vw_CatalogService.List("OperationParameterSections", BaseGenericRequest);

                if (list != null)
                {
                    result.ErrorCode = 0;
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
                list
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveNewMachine(int? MachineID, string MachineName, string MachineDescription, int? ProductionLineID, bool? Enabled, int ReferenceID, int? MachineCategoryID)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = MachineService.Insert(MachineName, MachineDescription, ProductionLineID, Enabled, MachineCategoryID, BaseGenericRequest);
                result.ErrorMessage = Resources.MFG.MachineSetups.msg_SuccessSaveMachine;

                if (result.ErrorCode == 0)
                {
                    string oErrorOutput = "";
                    var ReferenceType = MiscellaneousService.Catalog_GetDetailID(VARG_FacilityID, "SystemReferenceTypes", "MACHINEID", out oErrorOutput);
                    var AttachmentResult = CopyAttachments(ReferenceID, "TEMPID", result.ID, "MACHINEID");
                    var Attachments = Core.Services.AttachmentService.List(null, "MACHINEID", result.ID, ReferenceType, VARG_CompanyID, 0, null, BaseGenericRequest);
                    if (Attachments.Count > 0)
                    {
                        result = MachineService.Update(result.ID, null, null, null, Attachments.FirstOrDefault().FilePathName, null, MachineCategoryID, BaseGenericRequest);
                    }
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

        [HttpPost]
        public JsonResult UpdateMachines(int MachineID, string MachineName, string MachineDescription, int ProductionLineID, bool Enabled, int ReferenceID, int? MachineCategoryID)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                var AttachmentResult = CopyAttachments(ReferenceID, "TEMPID", MachineID, "MACHINEID");
                result = MachineService.Update(MachineID, MachineName, MachineDescription, ProductionLineID, null, Enabled, MachineCategoryID, BaseGenericRequest);

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
        public JsonResult SaveNewMaterial(string MaterialName)
        {
            GenericReturn result = new GenericReturn();
            CatalogDetail catalog = new CatalogDetail();

            try
            {
                var catalogListData = vw_CatalogService.List("OperationMaterials", BaseGenericRequest).FirstOrDefault();

                catalog.CatalogID = catalogListData.CatalogID;
                catalog.ValueID = MaterialName;

                result = CatalogDetailService.Add(catalog, BaseGenericRequest);
                if (result.ErrorCode == 2601)
                {
                    result.ErrorMessage = Resources.MFG.MachineSetups.msg_DuplicatedMaterial;
                }
                else
                {
                    result.ErrorMessage = Resources.MFG.MachineSetups.msg_SuccessSaveMaterial;

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

        [HttpPost]
        public JsonResult SaveNewSection(string SectionName)
        {
            GenericReturn result = new GenericReturn();
            CatalogDetail catalog = new CatalogDetail();

            try
            {
                var catalogListData = vw_CatalogService.List("OperationParameterSections", BaseGenericRequest).FirstOrDefault();

                catalog.CatalogID = catalogListData.CatalogID;
                catalog.ValueID = SectionName;

                result = CatalogDetailService.Add(catalog, BaseGenericRequest);
                result.ErrorMessage = Resources.MFG.MachineSetups.msg_SuccessSaveSection;

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

        [HttpPost]
        public JsonResult SaveNewParameter(string ParameterName, int? ParameterTypeID, int? ParameterLength, int? ParameterPrecision, string ParameterTag, bool? UseReference, string ReferenceName, int? ReferenceTypeID, int? ReferenceListID, bool? IsCavity, bool? Enabled)
        {
            GenericReturn result = new GenericReturn();
            CatalogDetail catalog = new CatalogDetail();

            try
            {
                result = MachineParametersService.Insert(ParameterName, ParameterTypeID, ParameterLength, ParameterPrecision, ParameterTag, UseReference, ReferenceName, ReferenceTypeID, ReferenceListID, IsCavity, Enabled, BaseGenericRequest);
                result.ErrorMessage = Resources.MFG.MachineSetups.msg_SuccessSaveParameter;
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

        public ActionResult UoMList()
        {
            var list = vw_CatalogService.List4Select("UoM", BaseGenericRequest, true);
            return Json(list.Select(s => new { value = s.CatalogDetailID, text = s.DisplayText }), JsonRequestBehavior.AllowGet);
        }
        public ActionResult YesNoList()
        {
            var list = vw_CatalogService.List4Select("YesNo", BaseGenericRequest, true);
            return Json(list.Select(s => new { value = s.CatalogDetailID, text = s.DisplayText }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult MachinesList()
        {
            var list = MachineService.List(BaseGenericRequest);
            return Json(list.Select(s => new { value = s.MachineID, text = s.MachineName }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult MaterialsList()
        {
            var list = vw_CatalogService.List4Select("OperationMaterials", BaseGenericRequest, true);
            return Json(list.Select(s => new { value = s.CatalogDetailID, text = s.DisplayText }), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ProductionProcessList()
        {
            var list = vw_CatalogService.List4Select("ProductionProcess", BaseGenericRequest, true);
            return Json(list.Select(s => new { value = s.CatalogDetailID, text = s.DisplayText }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ParameterList()
        {
            var list = MachineParametersService.List(null, null, null, null, null, null, null, null, null, true, BaseGenericRequest);
            return Json(list.Select(s => new { value = s.MachineParameterID, text = s.ParameterName }), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteMachineSetup(int entityID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = MachineSetupService.Delete(entityID, BaseGenericRequest);
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

        [HttpPost]
        public JsonResult CleanSectionData(int MachineSetupID, int ParameterSectionID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = MachineSetupParametersService.Delete(MachineSetupID, ParameterSectionID, null, BaseGenericRequest);
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

        public JsonResult GetModalParametersFormulas(string ValueSectionID, int? MachineSetupParameterID, string ParameterChoosedToFormula, string CurrentRowOdParameter)
        {
            GenericReturn result = new GenericReturn();
            ModalParametersFormulaViewModel model = new ModalParametersFormulaViewModel();
            string ViewPath = "~/Areas/MFG/Views/MachineSetup/_Mo_SetParametersFormulas.cshtml";
            string formula = "";

            try
            {
                model.ParametersList = new SelectList(MachineParametersService.List(null, null, null, null, null, null, null, null, null, null, BaseGenericRequest), "MachineParameterID", "ParameterName");
                model.ParamTypeIdentifier = ValueSectionID;
                model.Formula = formula;
                model.CurrentRowOdParameter = CurrentRowOdParameter;

                if (ParameterChoosedToFormula != null)
                    model.ParameterChoosedToFormula = ParameterChoosedToFormula;

                if (CurrentRowOdParameter != null)
                    model.CurrentRowOdParameter = CurrentRowOdParameter;

                if (MachineSetupParameterID != null)
                {
                    model.AddFormulaToParameter = false;
                    switch (ValueSectionID)
                    {
                        case "value":
                            formula = MachineSetupParametersService.List(null, null, BaseGenericRequest).Where(x => x.MachineSetupParameterID == MachineSetupParameterID).FirstOrDefault().FunctionValue;
                            break;
                        case "min":
                            formula = MachineSetupParametersService.List(null, null, BaseGenericRequest).Where(x => x.MachineSetupParameterID == MachineSetupParameterID).FirstOrDefault().FunctionMinValue;
                            break;
                        case "max":
                            formula = MachineSetupParametersService.List(null, null, BaseGenericRequest).Where(x => x.MachineSetupParameterID == MachineSetupParameterID).FirstOrDefault().FunctionMaxValue;
                            break;
                    }

                    var starPosition = formula.IndexOf(",");
                    if (starPosition > 0)
                    {
                        formula = formula.Substring(starPosition, formula.Length - starPosition).Replace(",", " ");
                    }

                    if (formula != "")
                        model.Formula = formula;
                }

                result.ErrorCode = 0;
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
    }
}