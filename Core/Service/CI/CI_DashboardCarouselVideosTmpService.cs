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
    public class CI_DashboardCarouselVideosTmpService
    {
        private static CI_DashboardCarouselVideosTmpRepository _rep;

        static CI_DashboardCarouselVideosTmpService()
        {
            _rep = new CI_DashboardCarouselVideosTmpRepository();
        }

        #region Methods

        public static List<DashboardCarouselVideosTmp> List(string TransactionID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(TransactionID, request))
            {
                List<DashboardCarouselVideosTmp> _list = dt.ConvertToList<DashboardCarouselVideosTmp>();

                return _list;
            }
        }

        public static GenericReturn Delete(int FileID, GenericRequest request)
        {
            return _rep.Delete(FileID, request);
        }

        public static GenericReturn CreateSettingsOnTmp(string TransactionID, GenericRequest request)
        {
            return _rep.CreateSettingsOnTmp(TransactionID, request);
        }


        public static GenericReturn Insert(string TransactionID, List<t_Files> list, GenericRequest request)
        {

            using (DataTable dt = list.ConvertToDataTable())
            {
                return _rep.Insert(TransactionID, dt, request);
            }
        }

        public static GenericReturn SetSort(string TransactionID, List<t_GenericItem> list, GenericRequest request)
        {

            using (DataTable dt = list.ConvertToDataTable())
            {
                return _rep.SetSort(TransactionID, dt, request);
            }
        }
        #endregion
    }
}
