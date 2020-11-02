namespace Core.Service
{
    #region namespaces

    using Core.Data;
    using Core.Entities;
    using Core.Entities.SQL_DataType;
    using Core.Entities.Utilities;
    using System.Collections.Generic;
    using System.Data;

    #endregion

    public class UsersProcessesLinesService
    {
        private static UsersProcessesLinesRepository _rep;

        static UsersProcessesLinesService()
        {
            _rep = new UsersProcessesLinesRepository();
        }

        #region Methods

        public static List<UsersProcessLine> AccessList(int? ProductionProcessID, GenericRequest request, bool EmptyFirst)
        {
            using (DataTable dt = _rep.AccessList(ProductionProcessID, request))
            {
                List<UsersProcessLine> _list = dt.ConvertToList<UsersProcessLine>();
                if (_list != null && _list.Count == 1)
                {
                    EmptyFirst = false;
                }
                if (EmptyFirst)
                {
                    // Anexar fila vacia al principio
                    _list.Insert(0, new UsersProcessLine() { ProductionLineID = 0, ProductionLineName = "" });
                }
                return _list;
            }
        }
        public static List<UsersProcessLine> LinesAccessList(int? EntityUserID, GenericRequest request)
        {
            using (DataTable dt = _rep.UserAccessList(EntityUserID, request))
            {
                List<UsersProcessLine> _list = dt.ConvertToList<UsersProcessLine>();
                return _list;
            }
        }

        public static GenericReturn Update(List<t_GenericItem> SelectedLinesID, int? EntityUserID, GenericRequest request)
        {
            using (DataTable dt = SelectedLinesID.ConvertToDataTable())
            {
                return _rep.Update(dt, EntityUserID, request);
            }
        }

        public static GenericReturn UsersProcessesLines_Insert(int? ProductionProcessID, int? ProductionLineID, int? ChangedBy, GenericRequest request)
        {
            return _rep.Insert(null, ProductionProcessID, ProductionLineID, ChangedBy, request);
        }
        #endregion

        public static List<UsersProcessLine> List(int? ProductionProcessID, int? ProductionLineID, int? EntityUserId, int? ChangedBy, GenericRequest request)
        {
            using (DataTable dt = _rep.List(ProductionProcessID, ProductionLineID, EntityUserId, ChangedBy, request))
            {
                List<UsersProcessLine> _list = dt.ConvertToList<UsersProcessLine>();
                return _list;
            }
        }

        public static GenericReturn Insert(int? UserID, int? ProductionProcessID, int? ProductionLineID, int? ChangedBy, GenericRequest request)
        {
            return _rep.Insert(UserID, ProductionProcessID, ProductionLineID, null, request);
        }

        public static GenericReturn Delete(int? EntityUserID, int? ProductionLineID, GenericRequest request)
        {
            return _rep.Delete(EntityUserID, ProductionLineID, request);
        }
    }
}
