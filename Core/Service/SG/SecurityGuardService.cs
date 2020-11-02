using Core.Data;
using Core.Entities;
using Core.Entities.SQL_DataType;
using Core.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class SecurityGuardService
    {
        private static SecurityGuardRepository _rep;
        static SecurityGuardService()
        {
            _rep = new SecurityGuardRepository();
        }

        public static List<SecurityGuardLog> List(GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, null, null, null, null, null, request))
            {
                List<SecurityGuardLog> _list = dt.ConvertToList<SecurityGuardLog>();
                return _list;
            }
        }
        public static List<SecurityGuardLog> List(string CheckInPersonTypes, string EmployeeNumber, string VehiclePlates, string CheckTypeID, string PersonName, DateTime? StartDate, DateTime? EndDate, GenericRequest request)
        {
            using (DataTable dt = _rep.List(CheckInPersonTypes, EmployeeNumber, VehiclePlates, CheckTypeID, PersonName, StartDate, EndDate, request))
            {
                List<SecurityGuardLog> _list = dt.ConvertToList<SecurityGuardLog>();
                return _list;
            }
        }

        public static GenericReturn Insert(SecurityGuardLog SGL, GenericRequest request)
        {
            return _rep.Insert(SGL, request);
        }

        public static GenericReturn Upsert(SecurityGuardLog SGL, GenericRequest request)
        {
            return _rep.Upsert(SGL, request);
        }

        public static GenericReturn SetCheckOut(int SecurityGuardLogID, DateTime? CheckOut, GenericRequest request)
        {
            return _rep.Update(SecurityGuardLogID, null, null, null, null, null, null, null, CheckOut, null, null, null, null, null, request);
        }

        public static List<CheckInPersonInfo> GetPersonInfoByCode(string AccessCode, string CheckType, GenericRequest request)
        {
            using (DataTable dt = _rep.GetPersonInfoByCode(AccessCode, CheckType, request))
            {
                List<CheckInPersonInfo> _list = dt.ConvertToList<CheckInPersonInfo>();
                return _list;
            }
        }

        public static DataSet ExportToExcel(int[] ddl_CheckInPersonTypes, int? ddl_Facilities, string PersonName, string CheckTypeID,
            DateTime? StartDate, DateTime? EndDate, string EmployeeNumber, string VehiclePlates, GenericRequest request)
        {

            string CheckInPersonTypes = null;

            if (ddl_CheckInPersonTypes != null)
            {
                CheckInPersonTypes = string.Join<int>(",", ddl_CheckInPersonTypes);
            }

            using (DataSet ds = _rep.ExportToExcel(CheckInPersonTypes, EmployeeNumber, VehiclePlates, CheckTypeID, PersonName, StartDate, EndDate, request))
            {
                return ds;
            }
        }

        public static List<SecurityGuardLog> GetSecurityGuardCheckOutInfo(string AccessCode, GenericRequest request)
        {
            using (DataTable dt = _rep.GetSecurityGuardCheckOutInfo(AccessCode, request))
            {
                List<SecurityGuardLog> _list = dt.ConvertToList<SecurityGuardLog>();
                return _list;
            }
        }

        public static bool CheckSessionState(string AccessCode, string CheckType, GenericRequest request)
        {
            using (DataTable dt = _rep.CheckSessionState(AccessCode, CheckType, request))
            {
                List<t_GenericItem> _list = dt.ConvertToList<t_GenericItem>();
                if (_list.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        public static SecurityGuardLog GetPersonInfoHistory(string AccessCode, int? PersonTypeID, GenericRequest request)
        {
            using (DataTable dt = _rep.GetPersonInfoHistory(AccessCode, PersonTypeID, request))
            {
                if (dt != null)
                {
                    return dt.ConvertToList<SecurityGuardLog>().FirstOrDefault();
                }
                else
                {
                    return new SecurityGuardLog();
                }
            }
        }

        public static List<Badge> GetAvailableBadges(int VendorTypeID, GenericRequest request, bool EmptyFirst = true, string EmptyText = "")
        {
            using (DataTable dt = _rep.GetAvailableBadges(VendorTypeID, request))
            {
                List<Badge> _list = dt.ConvertToList<Badge>();
                if (_list != null && _list.Count == 1)
                {
                    EmptyFirst = false;
                }
                if (EmptyFirst)
                {
                    // Anexar fila vacia al principio
                    _list.Insert(0, new Badge() { BadgeID = 0, BadgeNumber = EmptyText });
                }
                return _list;
            }
        }

    }
}
