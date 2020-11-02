using Core.Entities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Areas.MNT.Models.ViewModels.EnergySensors;
using WebSite.Models;

namespace WebSite.Areas.MNT.Controllers
{
    public class EnergySensorsController : BaseController
    {
        // GET: MNT/EnergySensors
        public ActionResult Index()
        {
            EnergySensorsTableViewModel model = new EnergySensorsTableViewModel();
            try
            {
                List<string> NamesList = new List<string>();
                NamesList.Add(Resources.MNT.EnergySensors.lbl_SelectName);
                NamesList.AddRange(MNT_EnergySensorsService.NamesList(BaseGenericRequest));

                model.EnergySensorsList = MNT_EnergySensorsService.List(BaseGenericRequest);
                model.FamiliesList = new SelectList(MNT_EnergySensorsFamiliesService.List(BaseGenericRequest), "EnergySensorFamilyID", "FamilyName");
                model.UsesList = new SelectList(vw_CatalogService.List("EnergySensorsUses", BaseGenericRequest), "CatalogdetailID", "DisplayText");
                model.SensorNamesList = NamesList;

            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return View(model);
        }


        public ActionResult New()
        {
            EnergySensorsEditViewModel model = new EnergySensorsEditViewModel();
            string viewPath = "~/Areas/MNT/Views/EnergySensors/Edit.cshtml";
            try
            {
                var ListOfSensorConfigs = MNT_EnergySensorsService.List(BaseGenericRequest);
                ListOfSensorConfigs.Insert(0, new EnergySensors() { EnergySensorID = 0, SensorName = Resources.MNT.EnergySensors.lbl_SelectSensor });


                model.Header = Resources.MNT.EnergySensors.lbl_NewFamilySensor;
                model.IsEdit = false;
                model.SensorConfigForCopyList = new SelectList(ListOfSensorConfigs, "EnergySensorID", "SensorName");

            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return View(viewPath, model);

        }

        public ActionResult Edit(int EnergySensorID)
        {
            EnergySensorsEditViewModel model = new EnergySensorsEditViewModel();
            string viewPath = "~/Areas/MNT/Views/EnergySensors/Edit.cshtml";

            List<EnergySensors> es = new List<EnergySensors>();

            try
            {
                var ListOfSensorConfigs = MNT_EnergySensorsService.List(BaseGenericRequest);
                ListOfSensorConfigs.Insert(0, new EnergySensors() { EnergySensorID = 0, SensorName = Resources.MNT.EnergySensors.lbl_SelectSensor });

                model.ES = MNT_EnergySensorsService.List(EnergySensorID, BaseGenericRequest).FirstOrDefault();
                model.Header = Resources.MNT.EnergySensors.title_SensorEdit;
                model.IsEdit = true;

                model.SensorsConfigList = MNT_EnergySensorValuesService.List(EnergySensorID, BaseGenericRequest);
                model.SensorConfigForCopyList = new SelectList(ListOfSensorConfigs, "EnergySensorID", "SensorName");

            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return View(viewPath, model);

        }

        public JsonResult Search(string[] SensorFamiliesIDs, string[] SensorNames, string[] SensorUsesIDs)
        {
            List<EnergySensors> model = new List<EnergySensors>();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/MNT/Views/EnergySensors/_Tbl_EnergySensors.cshtml";

            try
            {
                model = MNT_EnergySensorsService.List(SensorFamiliesIDs, SensorNames, SensorUsesIDs, BaseGenericRequest);
            }
            catch (Exception e)
            {
                ViewBag.Exception = e.Message;
            }

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString(),
                View = RenderRazorViewToString(ViewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetModalAddNewFamily()
        {
            GenericReturn result = new GenericReturn();
            EnergySensorFamiliesViewModel model = new EnergySensorFamiliesViewModel();
            string ViewPath = "~/Areas/MNT/Views/EnergySensors/_Mo_AddNewFamily.cshtml";
            model.IsEdit = false;


            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString(),
                View = RenderRazorViewToString(ViewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetModalCopyEnergySensorRecord(int EnergySensorID, string SensorName)
        {
            GenericReturn result = new GenericReturn();
            EnergySensorsEditViewModel model = new EnergySensorsEditViewModel();
            string ViewPath = "~/Areas/MNT/Views/EnergySensors/_Mo_CopyEnergySensor.cshtml";

            try
            {
                model.ES.EnergySensorID = EnergySensorID;
                model.ES.SensorName = SensorName;
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

        public JsonResult GetModalAlarmConfiguration(int? EnergysensorID)
        {
            GenericReturn result = new GenericReturn();
            EnergySensorsAlarmsViewModel model = new EnergySensorsAlarmsViewModel();
            string ViewPath = "~/Areas/MNT/Views/EnergySensors/_Mo_AlarmsConfiguration.cshtml";

            try
            {
                var SensorConfigs = MNT_EnergySensorsService.List(BaseGenericRequest);
                SensorConfigs.Insert(0, new EnergySensors() { EnergySensorID = 0 });
                model.SensorConfigForCopyList = new SelectList(SensorConfigs, "EnergySensorID", "SensorName");
                model.SensorsConfigList = MNT_EnergySensorValuesService.List(EnergysensorID, BaseGenericRequest);
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

        public JsonResult GetModalAttachments(int? EnergySensorID, string AttachmentType)
        {
            GenericReturn result = new GenericReturn();
            EnergySensorsEditViewModel model = new EnergySensorsEditViewModel();

            model.AttachmentType = AttachmentType;
            string ViewPath = "~/Areas/MNT/Views/EnergySensors/_Mo_Attachments.cshtml";

            return Json(new
            {
                result.ErrorCode,
                result.ErrorMessage,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString(),
                View = RenderRazorViewToString(ViewPath, model)
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEnergySensorValuesList(int EnergySensorID)
        {
            IEnumerable<SelectListItem> list = new List<SelectListItem>();
            GenericReturn result = new GenericReturn();

            try
            {
                list = new SelectList(MNT_EnergySensorValuesService.List(EnergySensorID, BaseGenericRequest), "ValueHour", "MaxValue");
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



        public JsonResult GetEnergySensorFamiliesList()
        {
            IEnumerable<SelectListItem> FamiliesList = new SelectList(new List<SelectListItem>());
            GenericReturn result = new GenericReturn();

            try
            {
                FamiliesList = new SelectList(MNT_EnergySensorsFamiliesService.List(BaseGenericRequest), "EnergySensorFamilyID", "FamilyName");
                if (FamiliesList != null)
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
                FamiliesList
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEnergySensorUsesList()
        {
            IEnumerable<SelectListItem> UsesList = new SelectList(new List<SelectListItem>());
            GenericReturn result = new GenericReturn();

            try
            {
                UsesList = new SelectList(vw_CatalogService.List("EnergySensorsUses", BaseGenericRequest), "CatalogdetailID", "DisplayText");
                if (UsesList != null)
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
                UsesList
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEnergySensorUnitsList()
        {
            IEnumerable<SelectListItem> UnitsList = new SelectList(new List<SelectListItem>());
            GenericReturn result = new GenericReturn();

            try
            {
                var EnergyList = vw_CatalogService.List("UoM", BaseGenericRequest).Where(w => w.Param1 == "Energy").ToList();
                UnitsList = new SelectList(EnergyList, "CatalogdetailID", "DisplayText");

                if (UnitsList != null)
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
                UnitsList
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEnergySensorNamesList()
        {
            IEnumerable<SelectListItem> UnitsList = new SelectList(new List<SelectListItem>());
            GenericReturn result = new GenericReturn();

            try
            {
                UnitsList = new SelectList(MNT_EnergySensorsService.List(BaseGenericRequest).Distinct().Select(x => x.SensorName), "EnergySensorID", "SensorName");
                if (UnitsList != null)
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
                UnitsList
            }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult AddNewFamily(int? EnergySensorFamilyID, string Name, decimal? MaxValue, string ImagePath, bool Enabled)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                if (EnergySensorFamilyID == null)
                {
                    result = MNT_EnergySensorsFamiliesService.Insert(Name, MaxValue, ImagePath, Enabled, BaseGenericRequest);
                }
                else
                {
                    result = MNT_EnergySensorsFamiliesService.Update(EnergySensorFamilyID, Name, MaxValue, ImagePath, Enabled, BaseGenericRequest);
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
                result.ID,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString(),
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddEnergySensor(int? EnergySensorID, string SensorName, int? EnergySensorFamilyID, int? SensorUseID, int? SensorUnitID, int? IndexKey, string DeviceID, bool? Enabled)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = MNT_EnergySensorsService.Insert(EnergySensorID, SensorName, EnergySensorFamilyID, SensorUseID, SensorUnitID, IndexKey, DeviceID, Enabled, BaseGenericRequest);
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
                result.ID,
                notifyType = result.ErrorCode == 0 ? StaticModels.NotifyType.success.ToString() : StaticModels.NotifyType.error.ToString(),
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteEnergySensor(int? EnergySensorID)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = MNT_EnergySensorsService.Delete(EnergySensorID, BaseGenericRequest);
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
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveCopiedEnergySensorRecord(int EnergySensorID, string SensorName)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = MNT_EnergySensorsFamiliesService.CopyAs(EnergySensorID, SensorName, BaseGenericRequest);
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
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveEditedEnergySensor(int? EnergySensorID, string SensorName, int? EnergySensorFamilyID, int? SensorUseID, int? SensorUnitID, int? IndexKey, string DeviceID, bool? Enabled)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = MNT_EnergySensorsService.Update(EnergySensorID, SensorName, EnergySensorFamilyID, SensorUseID, SensorUnitID, IndexKey, DeviceID, Enabled, BaseGenericRequest);
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
            }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult SaveAlarmConfiguration(int? EnergySensorID, List<EnergySensorValue> MaxValuesFourHours)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = MNT_EnergySensorValuesService.Insert(EnergySensorID, MaxValuesFourHours, BaseGenericRequest);
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
            }, JsonRequestBehavior.AllowGet);
        }
    }
}