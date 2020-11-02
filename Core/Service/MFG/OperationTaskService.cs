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
    public class OperationTaskService
    {
        private static OperationTaskRepository _rep;
        static OperationTaskService()
        {
            _rep = new OperationTaskRepository();
        }
        public static List<OperationTask> List(int? OperationRecordID, int? MachineID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, OperationRecordID, null, MachineID, null, null, null, null, null, request))
            {
                List<OperationTask> _list = dt.ConvertToList<OperationTask>();
                return _list;
            }
        }
        public static List<OperationTask> List(int? DateType, DateTime? StartDate, DateTime? EndDate, GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, null, null, null, null, DateType, StartDate, EndDate, null, request))
            {
                List<OperationTask> _list = dt.ConvertToList<OperationTask>();
                return _list;
            }
        }
        public static List<OperationTask> List(int? OperationTaskID, int? OperationSetupParameterID, int? OperationRecordID, string ResponsibleName, int? MachineID, int? ShiftID, int? DateType, DateTime? StartDate, DateTime? EndDate, int? StatusID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(OperationTaskID, OperationSetupParameterID, OperationRecordID, ResponsibleName, MachineID, ShiftID.SelectedValue(), DateType.SelectedValue(), StartDate, EndDate, StatusID.SelectedValue(), request))
            {
                List<OperationTask> _list = dt.ConvertToList<OperationTask>();
                return _list;
            }
        }

        public static GenericReturn Update(int? OperationTaskID, int? OperationSetupParameterID, string ProblemDescription, int? ResponsibleID, string SuggestedAction, string AttendantUserName, DateTime? TargetDate, DateTime? ClosedDate, int? StatusID, int? ChangedBy, GenericRequest request)
        {
            return _rep.Update(OperationTaskID, OperationSetupParameterID, ProblemDescription, ResponsibleID, SuggestedAction, AttendantUserName, TargetDate, ClosedDate, StatusID, ChangedBy, request);
        }

        public static DataSet Tasks_Report(int[] MachinesIDs, int[] MachineSetupIDs, int[] MaterialIDs, int[] ProcessIDs, int[] ShiftIDs,
            int[] StatusIDs, int[] ResponsibleIDs, string Attendant, int? DateType, DateTime? StartDate, DateTime? EndDate, GenericRequest request)
        {
            string Machines = null;
            string MachineSetups = null;
            string Materials = null;
            string Process = null;
            string Shifts = null;
            string Status = null;
            string Responsibles = null;
            if (MachinesIDs != null)
            {
                Machines = string.Join<int>(",", MachinesIDs);
            }
            if (MachineSetupIDs != null)
            {
                MachineSetups = string.Join<int>(",", MachineSetupIDs);
            }
            if (MaterialIDs != null)
            {
                Materials = string.Join<int>(",", MaterialIDs);
            }
            if (ProcessIDs != null)
            {
                Process = string.Join<int>(",", ProcessIDs);
            }
            if (ShiftIDs != null)
            {
                Shifts = string.Join<int>(",", ShiftIDs);
            }
            if (StatusIDs != null)
            {
                Status = string.Join<int>(",", StatusIDs);
            }
            if (ResponsibleIDs != null)
            {
                Responsibles = string.Join<int>(",", ResponsibleIDs);
            }
            using (DataSet ds = _rep.OperationTasks_Report(Machines, MachineSetups,
                Materials, Process, Shifts, Status, Responsibles, Attendant,
                DateType, StartDate, EndDate, request))
            {
                return ds;
            }
        }
    }
}
