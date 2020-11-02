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
    public class DataInterfaceService
    {
        private static DataInterfacesRepository _rep;

        static DataInterfaceService()
        {
            _rep = new DataInterfacesRepository();
        }

        #region CRUD
        public static List<DataInterface> List()
        {
            using (DataTable dt = _rep.List(null))
            {
                List<DataInterface> result = dt.ConvertToList<DataInterface>();
                return result;
            }

        }

        public static GenericReturn SendDataToDB(int DataInterfaceID, string FileName, string Reference, int? UserID, string CultureID, List<t_MasterTemplate> list)
        {
            return _rep.SendDataToDB(DataInterfaceID, FileName, Reference, UserID, CultureID, list.ConvertToDataTable());
        }

        public static List<DataInterfacelog> ListLog(string FileName, GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, FileName, null, null, null, null, request))
            {
                List<DataInterfacelog> _list = dt.ConvertToList<DataInterfacelog>();
                return _list;
            }
        }

        #endregion
    }
}
