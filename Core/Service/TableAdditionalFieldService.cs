using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{

    #region namespaces
    using Core.Entities.Utilities;
    using Data;
    using Entities;
    using System;
    using System.Collections.Generic;
    using System.Data;

    #endregion
    public class TableAdditionalFieldService
    {
        private static TableAdditionalFieldsRepository _rep;

        static TableAdditionalFieldService()
        {
            _rep = new TableAdditionalFieldsRepository();
        }

        #region Methods

        public static List<TableAdditionalFields> List(int? ReferenceID, string ModuleName, GenericRequest request)
        {
            using (DataTable dt = _rep.List(ReferenceID, ModuleName, null, request))
            {
                List<TableAdditionalFields> _list = dt.ConvertToList<TableAdditionalFields>();
                return _list;
            }
        }            
        public static List<TableAdditionalFields> ListConfiguration(int? ReferenceID,int FormatID, string ModuleName, GenericRequest request)
        {
            using (DataTable dt = _rep.ListConfiguration(ReferenceID, FormatID, ModuleName, request))
            {
                List<TableAdditionalFields> _list = dt.ConvertToList<TableAdditionalFields>();
                return _list;
            }
        }    
        public static List<TableAdditionalFields> ListDetail(int RequestID, int FormatID, GenericRequest request)
        {
            using (DataTable dt = _rep.ListDetail(RequestID, FormatID, request))
            {
                List<TableAdditionalFields> _list = dt.ConvertToList<TableAdditionalFields>();
                return _list;
            }
        } 
        public static List<TableAdditionalFields> List4Column(int? ReferenceID, string ModuleName, string ColumnName, GenericRequest request)
        {
            using (DataTable dt = _rep.List4Column(ReferenceID, ModuleName, ColumnName, request))
            {
                List<TableAdditionalFields> _list = dt.ConvertToList<TableAdditionalFields>();
                return _list;
            }
        }
        #endregion
    }
}
