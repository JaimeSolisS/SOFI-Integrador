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
    public static class  TranslationDetailService
    {
        private static TranslationDetailRepository _rep;

        static TranslationDetailService()
        {
            _rep = new TranslationDetailRepository();
        }

        #region CRUD

        public static GenericReturn Add(TranslationDetail _TranslationDetail, GenericRequest request)
        {
            return _rep.Add(_TranslationDetail, request);
        }

        public static List<TranslationDetail> List(int CatalogDetailID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(CatalogDetailID, request.FacilityID, request.UserID, request.CultureID))
            {
                List<TranslationDetail> _list = dt.ConvertToList<TranslationDetail>();

                return _list;
            }
        }

        public static TranslationDetail GetTranslation4Tag(string CatalogTag, string CultureID, GenericRequest request)
        {
            using (DataTable dt = _rep.GetTranslation4Tag(CatalogTag, request.FacilityID, request.UserID, CultureID))
            {
                TranslationDetail _list = dt.ConvertToList<TranslationDetail>().FirstOrDefault();

                return _list;
            }
        }

        public static GenericReturn QuickUpdate(int CatalogDetailID, string CultureID, string ColumnName, string Value, int FacilityID, int UserID, string CurrentCultureID)
        {
            return _rep.QuickUpdate(CatalogDetailID, CultureID, ColumnName, Value, FacilityID, UserID, CurrentCultureID);
        }

        #endregion
    }
}
