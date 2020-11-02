using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Utilities
{
    public class Security
    {
        public string GetCurrentUser()
        {
            string CurrentUser = String.Empty;
            CurrentUser = HttpContext.Current.User.Identity.Name;
            if (String.IsNullOrEmpty(CurrentUser))
            { CurrentUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name; }

            return CurrentUser;
        }
    }
}