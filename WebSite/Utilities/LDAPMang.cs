using Core.Service;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;

namespace WebSite.Utilities
{
    public class LDAPMang
    {
        public static bool TryAuthenticated(int facilityID, string domain, string username, string pwd)
        {
            String domainAndUsername = domain + @"\" + username;

            try
            {
                //Bind to the native AdsObject to force authentication.
                string LDAPDirectory = System.Configuration.ConfigurationManager.AppSettings["LDAPDirectory"].ToString();
                DirectoryEntry entry = new DirectoryEntry(MiscellaneousService.Param_GetValue(facilityID,"LDAPDirectory", LDAPDirectory), domainAndUsername, pwd);
                Object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry);

                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();

                if (null == result)
                {
                    return false;
                }
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception("Error authenticating user. " + ex.Message);
            }

            //return true;
        }
    }
}