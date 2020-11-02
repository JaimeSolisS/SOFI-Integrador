using Core.Entities;
using Core.Service;
using System;
using System.Web.Mvc;
using WebSite.Models;

namespace WebSite.Areas.Administration.Controllers
{
    public class GenericChartsAxisController : BaseController
    {

        [HttpPost]
        public JsonResult SaveNewAxis(GenericChartsAxis GenericChartsAxisEntity)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = GenericChartsAxisService.Insert(
                    GenericChartsAxisEntity.GenericChartID
                    , GenericChartsAxisEntity.AxisName
                    , GenericChartsAxisEntity.AxisTypeID
                    , GenericChartsAxisEntity.AxisChartTypeID
                    , GenericChartsAxisEntity.AxisDatatypeID
                    , GenericChartsAxisEntity.AxisColor
                    , GenericChartsAxisEntity.AxisFormat
                    , BaseGenericRequest);
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
        public JsonResult SaveEditedAxis(GenericChartsAxis GenericChartsAxisEntity)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = GenericChartsAxisService.Update(
                    GenericChartsAxisEntity.GenericChartAxisID
                    , GenericChartsAxisEntity.GenericChartID
                    , GenericChartsAxisEntity.AxisName
                    , GenericChartsAxisEntity.AxisTypeID
                    , GenericChartsAxisEntity.AxisChartTypeID
                    , GenericChartsAxisEntity.AxisDatatypeID
                    , GenericChartsAxisEntity.AxisColor
                    , GenericChartsAxisEntity.AxisFormat
                    , BaseGenericRequest);
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
        public JsonResult DeleteAxis(int GenericChartAxisID)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = GenericChartsAxisService.Delete(GenericChartAxisID, BaseGenericRequest);
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
        public JsonResult UpdateAxisName(int GenericChartAxisID, string AxisName)
        {
            GenericReturn result = new GenericReturn();

            try
            {
                result = GenericChartsAxisService.UpdateAxisName(GenericChartAxisID, AxisName, BaseGenericRequest);
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

    }
}