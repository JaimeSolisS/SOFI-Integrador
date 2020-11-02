using Core.Entities.Utilities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using WebSite.Controllers;
using System.DirectoryServices;
using Core.Entities;

namespace WebSite
{
    public class MvcApplication : System.Web.HttpApplication
    {
        #region Properties

        public const string SESSION_KEY_CULTURE = "CultureID";
        public const string SOFI_COOKIE = "SOFI_UserInfo";
        public const string USER_ID = "UserID";
        public const string USER_NAME = "UserName";
        public const string FIRST_NAME = "Firstname";
        public const string LAST_NAME = "Lastname";
        public const string USER_FULLNAME = "UserFullName";
        public const string FACILITY_ID = "BaseFacilityId";
        public const string COMPANY_ID = "CompanyId";

        public enum NotifyType
        {
            warning, // orage
            info,   // blue
            success,// green
            error   // red
        }

        public static string DefaultCulture;

        #endregion

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            // FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = new System.Globalization.CultureInfo("en-US");
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // Si no se ha seleccionado el idioma
            var DefaultCultureID = MiscellaneousService.Param_GetValue(0, "DefaultCultureID", "EN-US");
            if (Session[SESSION_KEY_CULTURE] == null)
                Session[SESSION_KEY_CULTURE] = DefaultCultureID;

            // De entrada no habra usuario logeado
            if (Session[USER_NAME] == null)
                Session[USER_NAME] = "";

            if (Session[USER_ID] == null)
                Session[USER_ID] = 0;

            // Obtener el usuario de Windows
            string FullUserInfo;
            string[] UserMachineInfo;
            string MachineDomainName;
            string MachineUserName;
            Utilities.Security security = new Utilities.Security();
            FullUserInfo = security.GetCurrentUser();
            //FullUserInfo = "ESSILOR-MX\\Cemsoft02";
            UserMachineInfo = FullUserInfo.Split('\\');
            MachineDomainName = UserMachineInfo[0];
            MachineUserName = UserMachineInfo[1];


            // Si aun no hay una base de información, crearla
            HttpCookie cookie = Request.Cookies[SOFI_COOKIE];

            if (cookie == null)
            {
                if (!string.IsNullOrEmpty(MachineUserName))
                {
                    // Obtener los datos del usuario
                    var UserInfoDB = UserService.GetInfo(MachineUserName, new Core.Entities.GenericRequest() { FacilityID = 0, CultureID = DefaultCultureID });

                    if (UserInfoDB == null)
                    {
                        try
                        {
                            string URI = string.Format("{0}://{1}", "LDAP", MachineDomainName);
                            string ldapFilter = "(samAccountName=" + MachineUserName + ")";
                            DirectoryEntry Entry = new DirectoryEntry(URI);
                            Entry.RefreshCache();
                            DirectorySearcher Searcher = new DirectorySearcher(Entry, ldapFilter);
                            //SearchResult Result = Searcher.FindOne();

                            foreach (SearchResult result in Searcher.FindAll())
                            {
                                DirectoryEntry de = result.GetDirectoryEntry();

                                UserInfoDB = new User()
                                {
                                    UserAccountID = MachineUserName,
                                    FirstName = de.Properties["name"].Value.ToString(),
                                    eMail = de.Properties["userprincipalname"].Value.ToString(),
                                    ChangedBy = 0,
                                    CultureID = DefaultCultureID
                                };

                                var UserGuest = UserService.AddAccountGuest(UserInfoDB, new Core.Entities.GenericRequest() { FacilityID = 0, CultureID = DefaultCultureID });
                                if (UserGuest != null)
                                {
                                    UserInfoDB = UserGuest;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorLogsService.Insert("SOFI.WebSite", 0, ex.Message.ToString(), 0, "Get AD Info", Context.Request.LogonUserIdentity.Name.ToString(),
                                                    Context.Request.UserHostAddress.ToString(), Context.Request.Browser.Type.ToString(), "", "", 0, "");

                            // En caso de no poder obtener datos de AD, usar el nombre del usuario obtenido para dar de alta
                            UserInfoDB = new User()
                            {
                                UserAccountID = MachineUserName,
                                FirstName = MachineUserName,
                                eMail = MachineUserName,
                                ChangedBy = 0,
                                CultureID = DefaultCultureID
                            };

                            var UserGuest = UserService.AddAccountGuest(UserInfoDB, new Core.Entities.GenericRequest() { FacilityID = 0, CultureID = DefaultCultureID });
                            if (UserGuest != null)
                            {
                                UserInfoDB = UserGuest;
                            }
                        }
                    }

                    if (UserInfoDB != null)
                    {
                        cookie = new HttpCookie(SOFI_COOKIE);
                        cookie[SESSION_KEY_CULTURE] = DefaultCultureID;
                        cookie[USER_ID] = UserInfoDB.UserID.ToString();
                        cookie[USER_NAME] = MachineUserName;
                        cookie[FIRST_NAME] = UserInfoDB != null ? UserInfoDB.FirstName : "";
                        cookie[LAST_NAME] = UserInfoDB != null ? UserInfoDB.LastName : "";
                        cookie[USER_FULLNAME] = UserInfoDB != null ? UserInfoDB.FullName : "";
                        cookie[FACILITY_ID] = UserInfoDB != null ? UserInfoDB.BaseFacilityID.ToString() : "0";
                        cookie[COMPANY_ID] = UserInfoDB != null ? UserInfoDB.CompanyID.ToString() : "0";
                        cookie[SESSION_KEY_CULTURE] = UserInfoDB != null ? UserInfoDB.DefaultCultureID.ToString() : "0";
                        cookie.Expires = DateTime.Now.AddYears(1);
                        Response.Cookies.Add(cookie);
                        HttpCookie langCookie = new HttpCookie("culture", UserInfoDB.DefaultCultureID.ToString());
                        langCookie.Expires = DateTime.Now.AddYears(1);
                        Response.Cookies.Add(langCookie);
                    }
                }
            }

        }

        public static string GetPropertyLDAP(SearchResult searchResult, string PropertyName)
        {
            if (searchResult.Properties.Contains(PropertyName))
            {
                return searchResult.Properties[PropertyName][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public void Application_Error(Object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            // TODO: Implementar logica para guardar el error
            int VARG_UserID = Session["UserID"] != null ? Session["UserID"].ToInt() : 0;
            string VARG_CultureID = Session["CultureID"] != null ? Session["CultureID"].ToString() : "";
            int FACILITY_ID = Session["BaseFacilityId"] != null ? Session["BaseFacilityId"].ToInt() : 0;

            ErrorLogsService.Insert("SOFI.WebSite", 0, exception.ToString(), FACILITY_ID, Context.Request.RawUrl.ToString(),
                Context.Request.LogonUserIdentity.Name.ToString(), Context.Request.UserHostAddress.ToString(), Context.Request.Browser.Type.ToString(),
                "", "", VARG_UserID, VARG_CultureID);

            

            Server.ClearError();
            bool isAjaxCall = string.Equals("XMLHttpRequest", Context.Request.Headers["x-requested-with"], StringComparison.OrdinalIgnoreCase);
            var routeData = new RouteData();
            routeData.Values.Add("controller", "ErrorPage");
            routeData.Values.Add("action", "Error");
            routeData.Values.Add("exception", exception);

            if (exception.GetType() == typeof(HttpException))
            { routeData.Values.Add("statusCode", ((HttpException)exception).GetHttpCode()); }
            else
            { routeData.Values.Add("statusCode", 500); }

            if (isAjaxCall)
            {
                if (routeData.Values["statusCode"].ToString() == "404" || routeData.Values["statusCode"].ToString() == "500")
                { Response.End(); }
                else
                {
                    Response.ContentType = "application/json";
                    Response.Write(
                        new JavaScriptSerializer().Serialize(
                            new { ErrorMessage = exception.Message.ToString() }
                        )
                    );
                }
            }
            else
            {
                Response.TrySkipIisCustomErrors = true;
                Response.StatusCode = 500;
                IController controller = new ErrorPageController();
                controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
                Response.End();
            }

        }
    }


}
