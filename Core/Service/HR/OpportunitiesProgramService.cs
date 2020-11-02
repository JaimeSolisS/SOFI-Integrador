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
    public class OpportunitiesProgramService
    {
        private static OpportunitiesProgramRepository _rep;

        static OpportunitiesProgramService()
        {
            _rep = new OpportunitiesProgramRepository();
        }

        public static List<OpportunitiesProgram> List(GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, null, null, null, null, null, null, request))
            {
                List<OpportunitiesProgram> _list = dt.ConvertToList<OpportunitiesProgram>();
                return _list;
            }
        }

        public static List<OpportunitiesProgram> SimpleList(string OpportunityNumber, string EmployeeNumber, GenericRequest request)
        {
            using (DataTable dt = _rep.SimpleList(OpportunityNumber, EmployeeNumber, request))
            {
                List<OpportunitiesProgram> _list = dt.ConvertToList<OpportunitiesProgram>();
                return _list;
            }
        }

        public static List<OpportunitiesProgram> List(string OpportunityNumber, GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, OpportunityNumber, null, null, null, null, null, null, request))
            {
                List<OpportunitiesProgram> _list = dt.ConvertToList<OpportunitiesProgram>();
                return _list;
            }
        }
        public static List<OpportunitiesProgram> List(int OpportunityProgramID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(OpportunityProgramID, null, null, null, null, null, null, null, request))
            {
                List<OpportunitiesProgram> _list = dt.ConvertToList<OpportunitiesProgram>();
                return _list;
            }
        }

        public static List<OpportunitiesProgram> List(int? OpportunityProgramID, string OpportunityNumber, int? DateTypeID, int? DepartmentID, string ShiftIDs, string Grades, DateTime? StartDate, DateTime? EndDate, GenericRequest request)
        {
            using (DataTable dt = _rep.List(OpportunityProgramID, OpportunityNumber, DateTypeID, DepartmentID, ShiftIDs, Grades, StartDate, EndDate, request))
            {
                List<OpportunitiesProgram> _list = dt.ConvertToList<OpportunitiesProgram>();
                return _list;
            }
        }

        public static GenericReturn Insert(string Name, string Description, List<User> Responsibles, int? DescriptionTypeID, int? DepartmentID, int? ShiftID, int? GradeID, int? DdlFacilityID, DateTime? ExpirationDate, bool? Enabled, int? CreatedBy, GenericRequest request)
        {
            if (Responsibles == null)
            {
                Responsibles = new List<User>();
            }
            using (DataTable dt = Responsibles.Select(x => new
            {
                x.ID
            }).ToList().ConvertToDataTable())
            {
                return _rep.Insert(Name, Description, dt, DescriptionTypeID, DepartmentID, ShiftID, GradeID, DdlFacilityID, ExpirationDate, Enabled, CreatedBy, request);
            }
        }

        public static GenericReturn Update(int? OpportunityProgramID, string OpportunityNumber, string Name, string Description, List<User> Responsibles, int? DescriptionTypeID, int? DepartmentID, int? ShiftID, int? GradeID, int? DdlFacilityID, DateTime? ExpirationDate, bool? Enabled, int NotificationTypeID, string FilesToDelete, int? CreatedBy, GenericRequest request)
        {
            if (Responsibles == null)
            {
                Responsibles = new List<User>();
            }
            using (DataTable dt = Responsibles.Select(x => new
            {
                x.ID
            }).ToList().ConvertToDataTable())
            {
                return _rep.Update(OpportunityProgramID, OpportunityNumber, Name, Description, dt, DescriptionTypeID, DepartmentID, ShiftID, GradeID, DdlFacilityID, ExpirationDate, Enabled, NotificationTypeID, FilesToDelete, CreatedBy, request);
            }
        }

        public static GenericReturn SetEnableDisable(int? OpportunityProgramID, bool? Enabled, int NotificationTypeID, string StatusName, GenericRequest request)
        {
            return _rep.SetEnableDisable(OpportunityProgramID, Enabled, NotificationTypeID, StatusName, request);
        }


        public static DataSet ListFilteredDataset(string txt_NumVacant, int DateTypeID, string[] ddl_Departments,
            DateTime? StartDate, DateTime? EndDate, string[] ddl_Shifts, string[] ddl_Grades, GenericRequest request)
        {

            var Departments = ddl_Departments != null ? string.Join<string>(",", ddl_Departments) : null;
            var Shifts = ddl_Shifts != null ? string.Join<string>(",", ddl_Shifts) : "";
            var Grades = ddl_Grades != null ? string.Join<string>(",", ddl_Grades) : "";

            using (DataSet ds = _rep.ListDataSet(txt_NumVacant, DateTypeID, Departments, Shifts, Grades, StartDate, EndDate, request))
            {
                return ds;
            }
        }

        public static GenericReturn SendNotifications(int? OpportunityProgramID, string Comment, string CandidateID, GenericRequest request)
        {
            return _rep.SendNotifications(OpportunityProgramID, Comment, CandidateID, request);
        }

        public static List<HRUserAccess> GetUserAccessToOProgramOptions(GenericRequest request)
        {
            using (DataTable dt = _rep.GetUserAccessToOProgramOptions(request))
            {
                List<HRUserAccess> _list = dt.ConvertToList<HRUserAccess>();
                return _list;
            }
        }

        public static List<OpportunitiesProgramMedia> MediaList(int OpportunityProgramID, GenericRequest request)
        {
            using (DataTable dt = _rep.MediaList(OpportunityProgramID, request))
            {
                List<OpportunitiesProgramMedia> _list = dt.ConvertToList<OpportunitiesProgramMedia>();

                return _list.OrderBy(o => o.Seq).ToList();
            }
        }

    }
}
