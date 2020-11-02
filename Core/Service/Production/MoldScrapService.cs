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
    public class MoldScrapService
    {
        private static MoldScrapRepository _rep;

        static MoldScrapService()
        {
            _rep = new MoldScrapRepository();
        }

        public static List<MoldScraps> List(MoldScraps entity , GenericRequest request)
        {
            using (DataTable dt = _rep.List(entity, request))
            {
                List<MoldScraps> _list = dt.ConvertToList<MoldScraps>();
                return _list;
            }
        }

        public static GenericReturn Upsert(MoldScraps entity, GenericRequest request)
        {
            return _rep.Upsert(entity, request);
        }

        public static GenericReturn BulkUpsert(List<Entities.SQL_DataType.t_MoldScrap> list, MoldScraps entity, GenericRequest request)
        {
            using (DataTable dt = list.ConvertToDataTable())
            {
                return _rep.BulkUpsert(dt, entity, request);
            }
        }

        //public static GenericReturn BulkUpsert(List<GenericReturn> MoldScraps entity, GenericRequest request)
        //{
        //    return _rep.BulkUpsert(entity, request);
        //}
    }
}
