using Core.Data;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities.Utilities;

namespace Core.Service
{
    public class DataInterfacesFieldService
    {
        private static DataInterfacesFieldRepository _rep;

        static DataInterfacesFieldService()
        {
            _rep = new DataInterfacesFieldRepository();
        }

        #region CRUD
        public static List<DataInterfacesField> List(int DataInterfaceID)
        {
            using (DataTable dt = _rep.List(DataInterfaceID))
            {
                List<DataInterfacesField> result = dt.ConvertToList<DataInterfacesField>();
                return result;
            }

        }
        #endregion
    }
}
