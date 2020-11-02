using Core.Data;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public static class ErrorLogsService
    {
        private static ErrorLogsRepository _rep;

        static ErrorLogsService()
        {
            _rep = new ErrorLogsRepository();
        }

        public static GenericReturn Insert(string ApplicationName, int ErrorLogId, string ErrorMessage, int FacilityID, string ProcessName,
         string Reference1, string Reference2, string Reference3, string Reference4, string Reference5,
         int UserID, string CultureID)
        {
            return _rep.Insert(ApplicationName, ErrorLogId, ErrorMessage, FacilityID, ProcessName,
          Reference1, Reference2, Reference3, Reference4, Reference5,
          UserID, CultureID);
        }
    }
}
