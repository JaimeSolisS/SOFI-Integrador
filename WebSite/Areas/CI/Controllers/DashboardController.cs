using Core.Entities;
using Core.Entities.Utilities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using WebSite.Areas.CI.Models.ViewModels.Dashboard;

namespace WebSite.Areas.CI.Controllers
{
    public class DashboardController : BaseController
    {
        // GET: CI/Dashboard
        public ActionResult Index()
        {
            var model = new IndexViewModel();
            GenericReturn result = new GenericReturn();

            try
            {
                var MonthNumber = MiscellaneousService.Catalog_GetDetailID(VARG_FacilityID, "Months", DateTime.Now.Month.ToString(), out string Message);
                var Month = vw_CatalogService.Get(MonthNumber, BaseGenericRequest);
                model.MonthName = Month.DisplayText;

                var ScreenSaverVideoPath = MiscellaneousService.Param_GetValue(VARG_FacilityID, "CI_Dashboard_ScreenSaverVideo", "");
                var BackgroundImage = MiscellaneousService.Param_GetValue(VARG_FacilityID, "CI_Dashboard_BackgroundImage", "");
                model.ScreenSaverInterval = MiscellaneousService.Param_GetValue(VARG_FacilityID, "CI_Dashboard_ScreenSaverInterval", "0").ToInt();
                model.ClosedWindowAfter = MiscellaneousService.Param_GetValue(VARG_FacilityID, "CI_Dashboard_ClosedWindowAfter", "0").ToInt();
                model.TotalVisits = CI_DashboardVisitService.Counter(null);

                // G.Sánchez (14 Sept 2019). Se filtra por nodos parent
                //model._ListAreas = CI_DashboardAreaService.List(null, true, null, BaseGenericRequest);
                model._CarouselVideos = CI_DashboardCarouselVideosService.List(null, BaseGenericRequest);

                // Ajustar la rutas de los videos
                model.BackgroundImage = string.Format("{0}/{1}", VARG_HostName, BackgroundImage);

                // G.Sánchez (14 Sept 2019). Usar el carusel de videos para el Screen Saver
                if (model._CarouselVideos != null && model._CarouselVideos.Count > 0)
                { ScreenSaverVideoPath = model._CarouselVideos[0].Path; }
                model.ScreenSaverVideoPath = string.Format("{0}/{1}", VARG_HostName, ScreenSaverVideoPath);

                foreach (var item in model._CarouselVideos)
                { item.Path = string.Format("{0}/{1}", VARG_HostName, item.Path); }

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
            var model = CI_DashboardAreaService.List4Parent(null, ParentID, true, BaseGenericRequest);
            return PartialView("~/Areas/CI/Views/Dashboard/_DashboardAreas.cshtml", model);
        }

        public ActionResult GetAreaDetail(int DashboardAreaID,int? ParentID)
        {
            var model = new IndexDetailViewModel();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/CI/Views/Dashboard/_AreaDetail.cshtml";

            try
            {
                var hostname = Request.RequestContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);
                model._List = CI_DashboardAreaDetailService.List(null, ParentID, DashboardAreaID, true, null, hostname, BaseGenericRequest);
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }

            return PartialView(ViewPath, model);
        }

        public ActionResult GetGallery(int DashboardAreaID, string DataEffectClass, string SourcePath)
        {
            var model = new IndexGalleryViewModel();
            GenericReturn result = new GenericReturn();
            string ViewPath = "~/Areas/CI/Views/Dashboard/_GalleryDetail.cshtml";

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
            string ViewPath = "~/Areas/CI/Views/Dashboard/_GalleryFullDetail.cshtml";
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
            catch (Exception )
            {
                string imagen = "/Content/img/not_found.png";
                model.Add(new GenericItem()
                {
                    Key = "notfound",
                   Name = "http://" +Request.Url.Authority + imagen
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

    }
}