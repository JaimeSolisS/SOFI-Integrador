using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;

namespace Core.Service
{
    public class KioskSuggestionsAdministratorService
    {
        private static KioskSuggestionsAdministratorRepository _rep;

        static KioskSuggestionsAdministratorService()
        {
            _rep = new KioskSuggestionsAdministratorRepository();
        }
        public static List<KioskEmployeeSuggestion> List(int? KioskEmployeeSuggestionID, string EmployeeID, int? CategoryID, string FacilityIDs, DateTime? StartDate, DateTime? EndDate, GenericRequest request)
        {
            using (DataTable dt = _rep.List(KioskEmployeeSuggestionID, EmployeeID, CategoryID, FacilityIDs, StartDate, EndDate, request))
            {
                List<KioskEmployeeSuggestion> _list = dt.ConvertToList<KioskEmployeeSuggestion>();
                return _list;
            }
        }

        public static List<KioskEmployeeSuggestion> List(string EmployeeID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, EmployeeID, null, null, null, null, request))
            {
                List<KioskEmployeeSuggestion> _list = dt.ConvertToList<KioskEmployeeSuggestion>();
                return _list;
            }
        }

        public static List<KioskEmployeeSuggestion> List(GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, null, null, null, null, request))
            {
                List<KioskEmployeeSuggestion> _list = dt.ConvertToList<KioskEmployeeSuggestion>();
                return _list;
            }
        }

        public static DataSet ListDataSet(int? CategoryID, int[] ddl_Facilities, DateTime? StartDate, DateTime? EndDate, GenericRequest request)
        {
            string Facilities = ddl_Facilities == null ? null : string.Join(",", ddl_Facilities);

            using (DataSet ds = _rep.ListDataSet(CategoryID, Facilities, StartDate, EndDate, request))
            {
                return ds;
            }
        }

        public static GenericReturn Delete(int? KioskEmployeeSuggestionID, GenericRequest request)
        {
            return _rep.Delete(KioskEmployeeSuggestionID, request);
        }

    }
}
