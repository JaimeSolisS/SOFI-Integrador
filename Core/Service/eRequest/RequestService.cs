using Core.Data;
using Core.Entities;
using Core.Entities.SQL_DataType;
using Core.Entities.Utilities;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class RequestService
    {
        private static RequestRepository _rep;

        static RequestService()
        {
            _rep = new RequestRepository();
        }

        public static List<Facility> GetFacility(GenericRequest request, bool AddEmptyRecord = false, string EmptyText = "")
        {
            using (DataTable dt = _rep.GetFacility(request))
            {
                List<Facility> _list = dt.ConvertToList<Facility>();
                if (_list != null && _list.Count == 1)
                {
                    AddEmptyRecord = false;
                }
                if (AddEmptyRecord)
                {
                    // Anexar fila vacia al principio
                    _list.Insert(0, new Facility() { FacilityID = 0, FacilityName = EmptyText });
                }

                return _list;
            }
        }

        public static List<FormatsEntity> GetFormat4User(GenericRequest request, bool AddEmptyRecord = false)
        {
            using (DataTable dt = _rep.GetFormat4User(request))
            {
                List<FormatsEntity> _list = dt.ConvertToList<FormatsEntity>();
                if (AddEmptyRecord && _list.Count > 1)
                {
                    _list.Insert(0, new FormatsEntity { FormatID = 0, FormatName = "" });
                }
                return _list;
            }
        }
        public static List<Request> RequestList(int? RequestID, string FormatIDs, int? Folio, string DepartmentIDs, string StatusIDs, string FacilityIDs, int? DateTypeID, DateTime? StartDate, DateTime? EndDate, GenericRequest request)
        {
            using (DataTable dt = _rep.RequestList(RequestID, FormatIDs, Folio, DepartmentIDs, StatusIDs, FacilityIDs, DateTypeID, StartDate, EndDate, request))
            {
                List<Request> _list = dt.ConvertToList<Request>();

                return _list;
            }
        }
        public static List<Request> RequestList(int? RequestID, GenericRequest request)
        {
            using (DataTable dt = _rep.RequestList(RequestID, request))
            {
                List<Request> _list = dt.ConvertToList<Request>();

                return _list;
            }
        }
        public static List<RequestLoopFlow> RequestLoopsFlowList(int RequestID, GenericRequest request)
        {
            using (DataTable dt = _rep.RequestLoopsFlowList(RequestID, request))
            {
                List<RequestLoopFlow> _list = dt.ConvertToList<RequestLoopFlow>();

                return _list;
            }
        }
        public static List<RequestsGenericDetail> RequestsGenericDetailList(int RequestID, GenericRequest request)
        {
            using (DataTable dt = _rep.RequestsGenericDetailList(RequestID, request))
            {
                List<RequestsGenericDetail> _list = dt.ConvertToList<RequestsGenericDetail>();

                return _list;
            }
        }
        public static List<RequestGenericDetail_tb> RequestsGenericDetailListTable(int RequestID, GenericRequest request)
        {
            using (DataTable dt = _rep.RequestsGenericDetailListTable(RequestID, request))
            {
                List<RequestGenericDetail_tb> _list = dt.ConvertToList<RequestGenericDetail_tb>();

                return _list;
            }
        }

        public static List<RequestApprovalLog> RequestsApprovalLogList(int RequestID, GenericRequest request)
        {
            using (DataTable dt = _rep.RequestsApprovalLogList(RequestID, request))
            {
                List<RequestApprovalLog> _list = dt.ConvertToList<RequestApprovalLog>();

                return _list;
            }
        }

        public static FormatsEntity GetFormat4ID(int FormatID, GenericRequest request)
        {
            using (DataTable dt = _rep.GetFormat4ID(FormatID, request))
            {
                List<FormatsEntity> _list = dt.ConvertToList<FormatsEntity>();

                return _list.FirstOrDefault();
            }
        }

        public static GenericReturn Create(List<t_RequestGenericDetail> GenericDetail, List<t_AdditionalFields> AdditionalFields, int FacilityID, string RequestDate, int DepeartmentID, int FormatID, string Concept, string Specification, GenericRequest request)
        {
            DataTable dtAdditionalFields = new DataTable();
            if (AdditionalFields != null)
            {
                dtAdditionalFields = AdditionalFields.ConvertToDataTable();
            }
            if (GenericDetail != null)
            {
                using (DataTable dtGenericDetail = GenericDetail.ConvertToDataTable())
                {
                    if (dtGenericDetail != null && dtGenericDetail.Rows.Count > 0)
                    {
                        return _rep.Create(dtGenericDetail, dtAdditionalFields, FacilityID, RequestDate, DepeartmentID, FormatID, Concept, Specification, request);
                    }
                    else
                    {
                        return _rep.Create(null, dtAdditionalFields, FacilityID, RequestDate, DepeartmentID, FormatID, Concept, Specification, request);
                    }
                }
            }
            else
            {
                return _rep.Create(null, dtAdditionalFields, FacilityID, RequestDate, DepeartmentID, FormatID, Concept, Specification, request);
            }


        }
        public static GenericReturn Edit(int RequestID, List<t_RequestGenericDetail> GenericDetail, List<t_AdditionalFields> AdditionalFields, int FacilityID, string RequestDate, int DepeartmentID, int FormatID, string Concept, string Specification, string FilesToDelete, GenericRequest request)
        {
            DataTable dtAdditionalFields = new DataTable();
            if (AdditionalFields != null)
            {
                dtAdditionalFields = AdditionalFields.ConvertToDataTable();
            }
            if (GenericDetail != null)
            {
                using (DataTable dtGenericDetail = GenericDetail.ConvertToDataTable())
                {
                    if (dtGenericDetail != null && dtGenericDetail.Rows.Count > 0)
                    {
                        return _rep.Edit(RequestID, dtGenericDetail, dtAdditionalFields, FacilityID, RequestDate, DepeartmentID, FormatID, Concept, Specification, FilesToDelete, request);
                    }
                    else
                    {
                        return _rep.Edit(RequestID, null, dtAdditionalFields, FacilityID, RequestDate, DepeartmentID, FormatID, Concept, Specification, FilesToDelete, request);
                    }
                }
            }
            else
            {
                return _rep.Edit(RequestID, null, dtAdditionalFields, FacilityID, RequestDate, DepeartmentID, FormatID, Concept, Specification, FilesToDelete, request);
            }


        }

        public static GenericReturn SeqValidate(int RequestLoopFlowID, GenericRequest request)
        {
            return _rep.SeqValidate(RequestLoopFlowID, request);
        }
        public static GenericReturn ApprovalFlowUser(int RequestLoopFlowID, int? UserID, GenericRequest request)
        {
            if (UserID == null)
            {
                UserID = request.UserID;
            }
            return _rep.ApprovalFlowUser(RequestLoopFlowID, UserID.ToInt(), request);
        }
        public static GenericReturn SendCodeValidation(string UserID, GenericRequest request)
        {
            if (UserID == "")
            {
                UserID = request.UserID.ToString();
            }
            return _rep.SendCodeValidation(UserID, request);
        }
        public static GenericReturn CheckCodeValidation(int UserID, string FAToken, GenericRequest request)
        {
            return _rep.CheckCodeValidation(UserID, FAToken, request);
        }
        public static GenericReturn CheckSignature(int UserID, GenericRequest request)
        {
            return _rep.CheckSignature(UserID, request);
        }
        public static bool ViewReadOnly(int? RequestID, GenericRequest request)
        {
            return _rep.ViewReadOnly(RequestID, request);
        }
        public static GenericReturn UpdateStatus(int RequestID, int StatusID, GenericRequest request)
        {
            return _rep.UpdateStatus(RequestID, StatusID, request);
        }
        public static GenericReturn RequestsLoopsFlowStatusUpdate(int RequestLoopFlowID, string Status, string Comments, int UserID, GenericRequest request)
        {
            return _rep.RequestsLoopsFlowStatusUpdate(RequestLoopFlowID, Status, Comments, UserID, request);
        }
        public static User GetSignatureIMG(GenericRequest request)
        {

            using (DataTable dt = _rep.GetSignatureIMG(request))
            {
                User _list = dt.ConvertToList<User>().FirstOrDefault();

                return _list;
            }

        }
        public static GenericReturn SaveSignature(Byte[] IMG, int? UserID, GenericRequest request)
        {
            if (UserID == null)
            {
                UserID = request.UserID;
            }
            return _rep.SaveSignature(IMG, UserID.ToInt(), request);

        }
        public static List<FormatsMedia> MediaList(int RequestID, GenericRequest request)
        {
            using (DataTable dt = _rep.MediaList(RequestID, request))
            {
                List<FormatsMedia> _list = dt.ConvertToList<FormatsMedia>();

                return _list.OrderBy(o => o.Seq).ToList();
            }
        }

        public static List<eReqUserAccess> GetUserAccessToeRequest(GenericRequest request)
        {
            using (DataTable dt = _rep.GetUserAccessToeRequest(request))
            {
                List<eReqUserAccess> _list = dt.ConvertToList<eReqUserAccess>();
                return _list;
            }
        }

        public static List<EntityField> FieldsConfigurationList(int EntityID, int EntityIndicator, string SystemModuleTag, GenericRequest request)
        {
            using (DataTable dt = _rep.FieldsConfigurationList(EntityID, EntityIndicator, SystemModuleTag, request))
            {
                List<EntityField> _list = dt.ConvertToList<EntityField>();
                return _list;
            }
        }
    }
}
