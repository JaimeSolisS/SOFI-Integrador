using Core.Entities;
using Core.Entities.Utilities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Models.ViewModels.ChartSlider;
using static WebSite.Models.StaticModels;

namespace WebSite.Controllers
{
    public class ChartsSliderController : BaseController
    {

        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();
            GenericReturn result = new GenericReturn();
            int ChartType = MiscellaneousService.Param_GetValue(VARG_FacilityID, "ChartSlider_DFT_ChartType", "0").ToInt();//tipo produccion leer de un param
            try
            {
                model.ComputerName = VARG_ComputerName;
                model.ChartList = ChartService.List(null, ChartType, BaseGenericRequest);
                model.CurrentShift = DashboardService.GetCurrentShiftID(1, BaseGenericRequest);
            }
            catch (Exception e)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = e.Message;
            }

            return View(model);
        }
        public ActionResult GetShowConfig(string CompName)
        {
            ConfigViewModel model = new ConfigViewModel();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Views/ChartsSlider/_Mo_Config.cshtml";
            int ChartType = MiscellaneousService.Param_GetValue(VARG_FacilityID, "ChartSlider_DFT_ChartType", "0").ToInt();
            try
            {
                model.ChartList = ChartService.List(null, ChartType, BaseGenericRequest);
                model.Charts = ChartService.ComputerSettings_List(null, CompName, null, BaseGenericRequest);
                model.CompName = CompName;
                model.ShiftList = ShiftService.List(null, true, BaseGenericRequest);
                model.ShiftID = DashboardService.GetCurrentShiftID(1, BaseGenericRequest);
                model.DFT_TimerSetup = MiscellaneousService.Param_GetValue(VARG_FacilityID, "ChartSlider_DFT_TimerSetup", "0").ToInt();
            }
            catch (Exception e)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = e.Message;
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    result.ErrorCode,
                    result.ErrorMessage,
                    notifyType = NotifyType.error.ToString(),
                    View = RenderRazorViewToString(ViewPath, model)
                }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(ViewPath, model);
        }
        public ActionResult GetChartDivs(string CompName)
        {
            List<Chart_ComputerSetting> model = new List<Chart_ComputerSetting>();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Views/ChartsSlider/_ChartsDivs.cshtml";
            try
            {
                model = ChartService.ComputerSettings_List(null, CompName, null, BaseGenericRequest);
            }
            catch (Exception e)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = e.Message;
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    result.ErrorCode,
                    result.ErrorMessage,
                    notifyType = NotifyType.error.ToString(),
                    View = RenderRazorViewToString(ViewPath, model)
                }, JsonRequestBehavior.AllowGet);
            }
            return View(ViewPath, model);
        }
        public ActionResult AddChartConfig(int ChartID, string CompName, int ShiftID)
        {
            Chart_ComputerSetting model = new Chart_ComputerSetting();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Views/ChartsSlider/_ChartConfig.cshtml";

            try
            {
                result = ChartService.ComputerSettings_Insert(CompName, ChartID, ShiftID, BaseGenericRequest);

                model = ChartService.ComputerSettings_Get(result.ID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = e.Message;
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new
                {
                    result.ErrorCode,
                    result.ErrorMessage,
                    notifyType = NotifyType.error.ToString(),
                    View = RenderRazorViewToString(ViewPath, model)
                }, JsonRequestBehavior.AllowGet);
            }
            return PartialView(ViewPath, model);
        }
        public ActionResult DeleteChartConfig(int Chart_SettingID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = ChartService.ComputerSettings_Delete(Chart_SettingID, BaseGenericRequest);
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
                notifyType = result.ErrorCode == 0 ? NotifyType.error.ToString() : NotifyType.success.ToString(),
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UpdateChartConfig(List<Chart_ComputerSetting> Settings, List<Charts_ComputerSettingsDetail> Options)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = ChartService.Charts_ComputerSettings_UpdateConfiguration(Settings, Options, BaseGenericRequest);
                //foreach (var ChartSetting in Settings)
                //{
                //    result = ChartService.ComputerSettings_QuickUpdate(ChartSetting.Chart_SettingID, "IntervalRefreshTime", ChartSetting.IntervalRefreshTime.ToString(), BaseGenericRequest);
                //    if (result.ErrorCode == 0)
                //    {
                //        result = ChartService.ComputerSettings_QuickUpdate(ChartSetting.Chart_SettingID, "Seq", ChartSetting.Seq.ToString(), BaseGenericRequest);
                //        if (result.ErrorCode == 0)
                //        {
                //            foreach (var Opt in ChartSetting.Options)
                //            {
                //                result = ChartService.Charts_ComputerSettingsDetail_QuickUpdate(Opt.Chart_SetttingDetailID, "OptionValue", Opt.OptionValue, BaseGenericRequest);
                //            }
                //        }
                //    }
                //}
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
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ShowChartOptions(int Chart_SettingID)
        {
            List<Charts_ComputerSettingsDetail> model = new List<Charts_ComputerSettingsDetail>();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Views/ChartsSlider/_Tbl_ChartOptions.cshtml";
            try
            {
                model = ChartService.ComputerSettingsDetail_List(Chart_SettingID, BaseGenericRequest);
                result.ErrorCode = 0;
            }
            catch (Exception e)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = e.Message;
            }
            return PartialView(ViewPath, model);

        }
        public ActionResult ShowChartOption(int OptionID, string DefaultOptionValue, string ControlName, int Chart_SetttingDetailID)
        {
            string ViewPath = "~/Views/ChartsSlider/_ChartOption.cshtml";
            var Option = vw_CatalogService.Get(OptionID, BaseGenericRequest);

            OptionViewModel model = new OptionViewModel();
            model.Option = Option;
            model.ControlName = ControlName;
            model.Chart_SetttingDetailID = Chart_SetttingDetailID;
            if (Option.Param1 == "SelectList")
            {
                if (Option.Param2 == "Shifts")
                {
                    model.OptionSelectList = new SelectList(ShiftService.List(null, true, BaseGenericRequest), "ShiftID", "ShiftDescription", DefaultOptionValue);
                }
                else if (Option.Param2 == "ProductionLines")
                {
                    List<UsersProcessLine> productionLineList = new List<UsersProcessLine>();
                    productionLineList = UsersProcessesLinesService.AccessList(null, BaseGenericRequest, false);
                    productionLineList.Insert(0, new UsersProcessLine() { ProductionLineID = 0, ProductionLineName = Resources.Common.TagAll });
                    model.OptionSelectList = new SelectList(productionLineList, "ProductionLineID", "ProductionLineName", DefaultOptionValue);
                }
                else
                {
                    model.OptionSelectList = new SelectList(vw_CatalogService.List4Select(Option.Param2, BaseGenericRequest), "CatalogDetailID", "DisplayText", DefaultOptionValue);
                }

            }
            if (Option.Param1 == "ListBox")
            {
                model.SelectedItems = Array.ConvertAll<string, int>(DefaultOptionValue.Split(','), int.Parse).ToList();
                if (Option.Param2 == "Defects")
                {
                    model.OptionListBox = DefectService.SelectList(null, null, null, true, BaseGenericRequest).Select(s => new SelectListItem
                    {
                        Text = s.DefectName,
                        Value = s.DefectID.ToString(),
                        Selected = model.SelectedItems.Contains(s.DefectID.Value)
                    });
                }
                else
                {
                    model.OptionListBox = vw_CatalogService.List4Select(Option.Param2, BaseGenericRequest).Select(s => new SelectListItem
                    {
                        Text = s.DisplayText,
                        Value = s.CatalogDetailID.ToString(),
                        Selected = model.SelectedItems.Contains(s.CatalogDetailID)
                    });
                }

            }
            if (Option.Param1 == "FilePath")
            {
                model.OptionValue = DefaultOptionValue;
            }
            if (Option.Param1 == "Url")
            {
                model.OptionValue = DefaultOptionValue;
            }
            return PartialView(ViewPath, model);
        }

        public JsonResult GetCurrentShift()
        {
            int ShiftID = DashboardService.GetCurrentShiftID(1, BaseGenericRequest);
            return Json(new { ShiftID }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file, int? SetttingDetailID)
        {
            GenericReturn result = new GenericReturn();

            string fileName = file.FileName;
            string fileExtension = file.ContentType;
            string pathToSave = "";
            string FileFolder = "~/Files/Temp/ProductionDashboard/ComputerSettingsDetail/" + SetttingDetailID + "/";
            string dirFullPath = System.Web.HttpContext.Current.Server.MapPath(FileFolder);
            try
            {
                if (!System.IO.Directory.Exists(dirFullPath))
                    System.IO.Directory.CreateDirectory(dirFullPath);

                if (!string.IsNullOrEmpty(fileName))
                {
                    pathToSave = dirFullPath + fileName;
                    file.SaveAs(pathToSave);

                    result = ChartService.Charts_ComputerSettingsDetail_QuickUpdate(SetttingDetailID, "OptionValue", fileName, BaseGenericRequest);
                }
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
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                fileName
            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetChartImage(int Chart_SettingID)
        {
            GenericReturn result = new GenericReturn();
            string ImagePath = "";
            string FileFolder = "~/Files/Temp/ProductionDashboard/ComputerSettingsDetail/";
            try
            {
                var Settings = ChartService.ComputerSettingsDetail_List(Chart_SettingID, BaseGenericRequest);
                var ImageSetting = Settings.Where(w => w.OptionType == "FilePath").FirstOrDefault();
                if (ImageSetting != null)
                {
                    ImagePath = FileFolder + ImageSetting.Chart_SetttingDetailID + "/" + ImageSetting.OptionValue;
                    string dirFullPath = System.Web.HttpContext.Current.Server.MapPath(ImagePath);
                    if (!System.IO.File.Exists(dirFullPath))
                    {
                        result.ErrorCode = 1; //no file found
                    }
                }

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
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                ImagePath
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetChartUrl(int Chart_SettingID)
        {
            GenericReturn result = new GenericReturn();
            string UrlPath = "";

            try
            {
                var Settings = ChartService.ComputerSettingsDetail_List(Chart_SettingID, BaseGenericRequest);
                var UrlSetting = Settings.Where(w => w.OptionType == "Url").FirstOrDefault();
                if (UrlSetting != null)
                {
                    UrlPath = UrlSetting.OptionValue;
                }
                else
                {
                    result.ErrorCode = 1; //no setting
                }
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
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString(),
                UrlPath
            }, JsonRequestBehavior.AllowGet);
        }
    }
}