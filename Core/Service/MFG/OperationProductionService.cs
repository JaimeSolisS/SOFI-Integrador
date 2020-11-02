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
    public class OperationProductionService
    {
        private static OperationProductionRepository _rep;
        static OperationProductionService()
        {
            _rep = new OperationProductionRepository();
        }

        #region Methods
        public static OperationProduction Get(int? OperationRecordID,  GenericRequest req)
        {
            using (DataTable dt = _rep.List(null, OperationRecordID, null, null, null, null, null, req))
            {
                List<OperationProduction> _list = dt.ConvertToList<OperationProduction>();
                return _list.FirstOrDefault();
            }
        }
        public static OperationProduction GetbyID(int? OperationProductionID, GenericRequest req)
        {
            using (DataTable dt = _rep.List(OperationProductionID, null, null, null, null, null, null, req))
            {
                List<OperationProduction> _list = dt.ConvertToList<OperationProduction>();
                return _list.FirstOrDefault();
            }
        }
        public static List<OperationProduction> List(int? OperationProductionID, int? OperationRecordID, decimal? CycleTime, int? CavitiesNumber, int? ProducedQty, int? InitialShotNumber, int? FinalShotNumber, GenericRequest req)
        {
            using (DataTable dt = _rep.List(OperationProductionID, OperationRecordID, CycleTime, CavitiesNumber, ProducedQty, InitialShotNumber, FinalShotNumber, req))
            {
                List<OperationProduction> _list = dt.ConvertToList<OperationProduction>();
                return _list;
            }
        }
        public static GenericReturn Upsert(int? OperationProductionID, int? OperationRecordID, decimal? CycleTime, int? CavitiesNumber, int? ProducedQty, int? InitialShotNumber, int? FinalShotNumber, GenericRequest req)
        {
            return _rep.Upsert(OperationProductionID.SelectedValue(), OperationRecordID, CycleTime, CavitiesNumber, ProducedQty, InitialShotNumber, FinalShotNumber, req);
        }

        #endregion

        #region DetailsMethods
        public static OperationProductionDetail Details_Get(int? OperationProductionDetailID, GenericRequest req)
        {
            using (DataTable dt = _rep.Details_List(OperationProductionDetailID, null, null, null, null, req))
            {
                List<OperationProductionDetail> _list = dt.ConvertToList<OperationProductionDetail>();
                return _list.FirstOrDefault();
            }
        }
        public static List<OperationProductionDetail> Details_List(int? OperationProductionID, GenericRequest req)
        {
            using (DataTable dt = _rep.Details_List(null, OperationProductionID, null, null, null, req))
            {
                List<OperationProductionDetail> _list = dt.ConvertToList<OperationProductionDetail>();
                return _list;
            }
        }
        public static List<OperationProductionDetail> Details_List(int? OperationProductionDetailID, int? OperationProductionID, int? Hour, int? ShotNumber, int? ProducedQty, GenericRequest req)
        {
            using (DataTable dt = _rep.Details_List(OperationProductionDetailID, OperationProductionID, Hour, ShotNumber, ProducedQty, req))
            {
                List<OperationProductionDetail> _list = dt.ConvertToList<OperationProductionDetail>();
                return _list;
            }
        }
        public static GenericReturn Details_Upsert(int? OperationProductionDetailID, int? OperationProductionID, int? Hour, int? ShotNumber, int? ProducedQty, GenericRequest request)
        {
            return _rep.Details_Upsert(OperationProductionDetailID, OperationProductionID, Hour, ShotNumber, ProducedQty, request);
        }

        #endregion

        public static List<OperationProductionDetailsLog> DetailsLogs_List(int? OperationProductionDetailLogID, int? OperationProductionID, int? CurrentShotNumber, GenericRequest request)
        {
            using (DataTable dt = _rep.DetailsLogs_List(OperationProductionDetailLogID, OperationProductionID, CurrentShotNumber, request))
            {
                List<OperationProductionDetailsLog> _list = dt.ConvertToList<OperationProductionDetailsLog>();
                return _list;
            }
        }
        public static GenericReturn DetailsLogs_Insert(int? OperationProductionID, int? CurrentShotNumber,bool RecalculateProductionHours, GenericRequest request)
        {
            return _rep.DetailsLogs_Insert(OperationProductionID, CurrentShotNumber, RecalculateProductionHours, request);
        }


        public static List<t_ChartData> ShiftHours(int? OperationProductionID, int? OperationRecordID, GenericRequest request)
        {
            using (DataTable dt = _rep.ShiftHours(OperationProductionID, OperationRecordID, request))
            {
                return dt.ConvertToList<t_ChartData>();
            }
        }
        public static int GetHour(int OperationProductionID, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                return _rep.GetHour(OperationProductionID);
            }
            catch (Exception ex)
            {
                ErrorMessage = "ERROR: " + ex.Message;
                return 0;
            }
        }
        public static bool IsCapturedHour(int OperationProductionID,int Hour, out string ErrorMessage)
        {
            ErrorMessage = "";
            try
            {
                return _rep.IsCapturedHour(OperationProductionID, Hour);
            }
            catch (Exception ex)
            {
                ErrorMessage = "ERROR: " + ex.Message;
                return false;
            }
        }


    }
}
