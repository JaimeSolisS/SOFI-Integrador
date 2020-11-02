using Core.Entities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebSite.Areas.Administration.Models.ViewModels.Profiles;
using WebSite.Models;
using static WebSite.Models.StaticModels;

namespace WebSite.Areas.Administration.Controllers
{
    public class ProfilesController : BaseController
    {
        public ActionResult Index()
        {
            var model = new IndexViewModel();
            try
            {
                model.Profiles = ProfileService.List(new Profile(), BaseGenericRequest);
                var profileList = new List<Profile>();
                profileList.AddRange(model.Profiles);
                profileList.Insert(0, new Profile { ProfileID = 0, ProfileName = "" });
                model.ProfileList = new SelectList(profileList, "ProfileID", "ProfileName"); ;
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

        public ActionResult Create()
        {
            EditViewModel model = new EditViewModel();
            try
            {
                model.OrganizationsList = new SelectList(OrganizationService.List(true, BaseGenericRequest), "OrganizationID", "OrganizationName");
            }
            catch (Exception e)
            {
                MessageBoxOK(NotifyType.error, "error", e.Message);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView();
            }
            return View();
        }
        [HttpPost]
        public ActionResult Create([Bind(Include = "ProfileName")]Profile newProfile)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = ProfileService.Upsert(newProfile, null, BaseGenericRequest);
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
                result.ID
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int ProfileID)
        {
            EditViewModel model = new EditViewModel();
            try
            {
                model.EditProfile = ProfileService.Get(ProfileID, BaseGenericRequest);
            }
            catch (Exception e)
            {
                MessageBoxOK(NotifyType.error, "error", e.Message);
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView(model);
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ProfileId,ProfileName,OrganizationID")] Profile editProfile, string SelectedMenusId)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = ProfileService.Upsert(editProfile, SelectedMenusId, BaseGenericRequest);
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
                result.ID,
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int ProfileID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = ProfileService.Delete(ProfileID, BaseGenericRequest);
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
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AppMenus(string ProfileArrayID)
        {
            var nodes = new List<JsTreeModel>();
            JavaScriptSerializer js = new JavaScriptSerializer();
            var menus = ProfileService.GetProfilesMenus(ProfileArrayID, VARG_UserID, VARG_CultureID);
            foreach (var p in menus.OrderBy(i => i.Sequence))
            {
                nodes.Add(new JsTreeModel()
                {
                    id = p.MenuID.ToString(),
                    parent = p.ParentMenuID.ToString() == "0" ? "#" : p.ParentMenuID.ToString(),
                    text = p.Description.ToString(),
                    state = new JsTreeModelState { opened = true, selected = p.Enabled }
                });
            }
            var str = js.Serialize(nodes);
            return Json(nodes, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ShowEvents(int ProfileID, int MenuID)
        {
            GenericReturn result = new GenericReturn();
            List<AppEvents> model = new List<AppEvents>();
            var ViewPath = "~/Areas/Administration/Views/Profiles/_ShowEvents.cshtml";
            try
            {
                model = ProfileService.GetProfilesEvents(ProfileID, MenuID, VARG_UserID, VARG_CultureID);
                if (model.Any())
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
                View = RenderRazorViewToString(ViewPath, model)
            }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult UpdateEvents(int ProfileID, int MenuID, string SelectedEventsId)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = ProfileService.ProfilesEventsUpdate(ProfileID, MenuID, SelectedEventsId, VARG_UserID, VARG_CultureID);
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
                Title = "",
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateEventAccess(int ProfileID, int EventID, bool Access, int? AccessTypeID)
        {
            GenericReturn result = new GenericReturn();
            try
            {
                result = AppEventProfileService.UpdateAccess(EventID, ProfileID, Access, AccessTypeID, BaseGenericRequest);
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
                Title = "",
                notifyType = result.ErrorCode == 0 ? NotifyType.success.ToString() : NotifyType.error.ToString()
            }, JsonRequestBehavior.AllowGet);
        }

        #region CRUD
        public ActionResult Profiles_List(int ProfileID)
        {
            var model = new List<Profile>();
            Profile profile = new Profile();
            if (ProfileID != 0)
            {
                profile.ProfileID = ProfileID;
            }
            model = ProfileService.List(profile, BaseGenericRequest);
            return PartialView(@"~/Areas/Administration/Views/Profiles/_Tbl_Profiles.cshtml", model);
        }
        #endregion
    }
}