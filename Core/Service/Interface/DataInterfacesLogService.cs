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
    public class DataInterfacesLogService
    {
        private static DataInterfacesLogRepository _rep;
        //private object dbCommand;

        static DataInterfacesLogService()
        {
            _rep = new DataInterfacesLogRepository();
        }

        #region CRUD
        public static List<t_Files> GetFilesToProcess(int DataInterfaceID, List<t_Files> files)
        {
            using (DataTable dt = _rep.FilesToProcess(DataInterfaceID, files.ConvertToDataTable()))
            {
                List<t_Files> _list = dt.ConvertToList<t_Files>();
                return _list;
            }

        }

        public static GenericReturn Upsert(DataInterfacelog entity)
        {
            return _rep.Upsert(entity);
        }


        #endregion
    }
}
