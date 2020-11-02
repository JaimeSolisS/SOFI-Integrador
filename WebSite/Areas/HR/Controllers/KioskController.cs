using Core.Entities;
using Core.Entities.Utilities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Areas.HR.Models.ViewModels.Kiosk;
using static WebSite.Models.StaticModels;

namespace WebSite.Areas.HR.Controllers
{
    public class KioskController : BaseController
    {
        public ActionResult Index()
        {
            var model = new IndexViewModel();
            GenericReturn result = new GenericReturn();

            try
            {
                var ScreenSaverVideoPath = MiscellaneousService.Param_GetValue(VARG_FacilityID, "HR_Kiosk_ScreenSaverVideo", "");
                var BackgroundImage = MiscellaneousService.Param_GetValue(VARG_FacilityID, "HR_Kiosk_BackgroundImage", "");
                model.BackgroundImage = string.Format("{0}/{1}", VARG_HostName, BackgroundImage);
                model.SessionTime = MiscellaneousService.Param_GetValue(VARG_FacilityID, "HR_Kiosk_ClosedWindowAfter", "0").ToInt();
                model.CarouselMedia = KioskMediaService.List(null, BaseGenericRequest);
                model.ScreenSaverInterval = MiscellaneousService.Param_GetValue(VARG_FacilityID, "HR_Kiosk_ScreenSaverInterval", "0").ToInt();
                model.TransitionTime = MiscellaneousService.Param_GetValue(VARG_FacilityID, "HR_KIOSK_CAROUSELTRANSITIONTIME", "5000");
                model.FacilityName = FacilityService.List4Select(BaseGenericRequest, false).Where(x => x.FacilityID == VARG_FacilityID).FirstOrDefault().FacilityName;

                if (model.CarouselMedia != null && model.CarouselMedia.Count > 0)
                    ScreenSaverVideoPath = model.CarouselMedia[0].Path;

                model.ScreenSaverVideoPath = string.Format("{0}/{1}", VARG_HostName, ScreenSaverVideoPath);

                foreach (var item in model.CarouselMedia)
                    item.Path = string.Format("{0}/{1}", VARG_HostName, item.Path);


            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }

            return View(model);
        }

        public ActionResult GetAreas(int ParentID)
        {
            var model = new KioskAreaViewModel();
            GenericReturn result = new GenericReturn();

            try
            {
                var current = KioskAreaDetailService.Get(ParentID, BaseGenericRequest);

                model.KioskAreasList = KioskAreaService.List4Parent(null, ParentID, BaseGenericRequest);
                model.ParentID = ParentID;

                if (current != null)
                {
                    if (current.ParentKioskAreaDetailID == null)
                        current.ParentKioskAreaDetailID = 0;

                    model.BackParentID = current.ParentKioskAreaDetailID;
                    model.BackKioskAreaID = current.KioskAreaID;

                }
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }

            return PartialView("~/Areas/HR/Views/Kiosk/_KioskAreas.cshtml", model);
        }

        public ActionResult GetAreaDetail(int KioskAreaID, int? ParentID)
        {
            var model = new KioskAreaDetailsViewModel();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/HR/Views/Kiosk/_KioskAreaDetails.cshtml";

            try
            {
                var hostname = Request.RequestContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);
                model.KioskAreaDetailList = KioskAreaDetailService.List(null, ParentID, KioskAreaID, true, null, hostname, BaseGenericRequest);

            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }

            return PartialView(ViewPath, model);
        }


        public ActionResult GetGallery(int KioskAreaID, string DataEffectClass, string SourcePath)
        {
            var model = new CI.Models.ViewModels.Dashboard.IndexGalleryViewModel();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/HR/Views/Kiosk/_GalleryDetail.cshtml";

            try
            {
                model.DataEffectClass = DataEffectClass;
                var hostname = Request.RequestContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);

                var path = Server.MapPath(SourcePath.Replace(hostname, ""));

                foreach (var item in Directory.GetFiles(path))
                {
                    model._List.Add(new GenericItem()
                    {
                        Key = item,
                        Name = string.Format("{0}/{1}", SourcePath, Path.GetFileName(item))
                    });
                }
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }

            return PartialView(ViewPath, model);
        }

        public ActionResult GetGalleryDetail(string SourcePath)
        {
            var model = new List<GenericItem>();
            string ViewPath = "~/Areas/HR/Views/Kiosk/_GalleryFullDetail.cshtml";
            try
            {
                var hostname = Request.RequestContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);
                var path = Server.MapPath(SourcePath.Replace(hostname, ""));

                if (Directory.GetFiles(path) != null)
                {
                    foreach (var item in Directory.GetFiles(path))
                    {
                        model.Add(new GenericItem()
                        {
                            Key = item,
                            Name = string.Format("{0}/{1}", SourcePath, Path.GetFileName(item))
                        });
                    }
                }
            }
            catch (Exception)
            {
                string imagen = "/Content/img/not_found.png";
                model.Add(new GenericItem()
                {
                    Key = "notfound",
                    Name = "http://" + Request.Url.Authority + imagen
                });
                //throw;
            }

            return PartialView(ViewPath, model);
        }

        [HttpPost]
        public ActionResult CounterAdd()
        {
            var TotalVisits = CI_DashboardVisitService.Add(BaseGenericRequest.UserID);
            return Json(new
            {
                TotalVisits
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetPathVideo(int Seq)
        {
            var video = CI_DashboardCarouselVideosService.GetBySeq(Seq, BaseGenericRequest);
            return Json(new
            {
                VideoPath = video.Path
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetModalLoginOptions()
        {
            string ViewPath = "~/Areas/HR/Views/Kiosk/_Mo_LoginOptions.cshtml";
            IEnumerable<SelectListItem> model = new SelectList(new List<Catalog>());
            try
            {
                model = new SelectList(vw_CatalogService.List4Select("SOFIDomains", BaseGenericRequest, false), "CatalogDetailID", "DisplayText");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.ToString();
            }

            return PartialView(ViewPath, model);
        }
    }
}