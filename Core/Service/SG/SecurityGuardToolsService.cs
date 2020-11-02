using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class SecurityGuardToolsService
    {
        private static SecurityGuardToolsRepository _rep;

        static SecurityGuardToolsService()
        {
            _rep = new SecurityGuardToolsRepository();
        }

        public static List<SecurityGuardTool> List(string AccessCode, int? SecurityGuardLogID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(AccessCode, SecurityGuardLogID, request))
            {
                List<SecurityGuardTool> _list = dt.ConvertToList<SecurityGuardTool>();
                return _list;
            }
        }

        public static List<SecurityGuardTool> OldNewToolsList(int? TempAttachmentID, GenericRequest request)
        {
            using (DataTable dt = _rep.OldNewToolsList(TempAttachmentID, request))
            {
                List<SecurityGuardTool> _list = dt.ConvertToList<SecurityGuardTool>();
                return _list;
            }
        }

        public static GenericReturn Insert(SecurityGuardTool SecurityTools, GenericRequest request)
        {
            return _rep.Insert(SecurityTools, request);
            //using (DataTable dt = SecurityToolsList.Select(x => new
            //{
            //    x.SecurityGuardLogID,
            //    x.ToolName,
            //    x.ToolImgPath
            //}).ToList().ConvertToDataTable())
            //{
            //    return _rep.Insert(dt, request);
            //}
        }

        public static GenericReturn UpdateImgPath(int? SecurityGuardToolID, string ToolImgPath, GenericRequest request)
        {
            return _rep.UpdateImgPath(SecurityGuardToolID, ToolImgPath, request);
        }

        public static List<SecurityGuardTool> ListByCheckIn(string AccessCode, int? TempAttachmentID, GenericRequest request)
        {
            using (DataTable dt = _rep.ListByCheckIn(AccessCode, TempAttachmentID, request))
            {
                List<SecurityGuardTool> _list = dt.ConvertToList<SecurityGuardTool>();
                return _list;
            }
        }

        public static GenericReturn UpdateTempData(int? OldSecurityGuardLogID, int? NewSecurityGuardLogID, string ToolsToDisable, List<SecurityGuardTool> ToolsToAble, GenericRequest request)
        {
            if (ToolsToAble != null)
            {
                using (DataTable dt = ToolsToAble.Select(x => new
                {
                    x.SecurityGuardLogID,
                    x.ToolName,
                    x.ToolImgPath
                }).ToList().ConvertToDataTable())

                {
                    return _rep.UpdateTempData(OldSecurityGuardLogID, NewSecurityGuardLogID, ToolsToDisable, dt, request);
                }
            }
            else
            {
                var dt = new List<SecurityGuardTool>().Select(x => new { x.SecurityGuardLogID, x.ToolName, x.ToolImgPath }).ToList().ConvertToDataTable();
                return _rep.UpdateTempData(OldSecurityGuardLogID, NewSecurityGuardLogID, ToolsToDisable, dt, request);
            }
        }

        public static List<SecurityGuardTool> GetAvailableToolsByUser(int? TempAttachmentID, string AccessCode, GenericRequest request)
        {
            using (DataTable dt = _rep.GetAvailableToolsByUser(TempAttachmentID, AccessCode, request))
            {
                List<SecurityGuardTool> _list = dt.ConvertToList<SecurityGuardTool>();
                return _list;
            }
        }

    }
}
