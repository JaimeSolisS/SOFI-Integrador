using Core.Entities.Utilities;
using Core.Data;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;


namespace Core.Service
{
    public class CatalogDetailService
    {
        private static CatalogDetailRepository _rep;

        static CatalogDetailService()
        {
            _rep = new CatalogDetailRepository();
        }

        #region CRUD

        public static GenericReturn Add(CatalogDetail _CatalogDetail, GenericRequest request)
        {
            // TODO: Metodo para convertir 0 a null

            return _rep.Add(_CatalogDetail, request);
        }

        public static GenericReturn QuickUpdate(int CatalogDetailID, string ColumnName, string Value, int FacilityID, int UserID, string CultureID)
        {
            return _rep.QuickUpdate(CatalogDetailID, ColumnName, Value, FacilityID, UserID, CultureID);
        }

        public static List<CatalogDetail> List(int CatalogID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(CatalogID, request.FacilityID, request.UserID, request.CultureID))
            {
                List<CatalogDetail> _list = dt.ConvertToList<CatalogDetail>();

                return _list;
            }
        }    
        public static List<CatalogDetailTemp> List4FormatPhase(int CatalogID, string FormatID, Guid TransactionID, GenericRequest request)
        {
            using (DataTable dt = _rep.List4FormatPhase(CatalogID, FormatID, TransactionID, request.FacilityID, request.UserID, request.CultureID))
            {
                List<CatalogDetailTemp> _list = dt.ConvertToList<CatalogDetailTemp>();

                return _list;
            }
        }  public static List<CatalogDetailTemp> List4FormatPhaseEdit(int? FormatLoopRuleTempID, int CatalogID, string FormatID, Guid TransactionID, GenericRequest request)
        {
            using (DataTable dt = _rep.List4FormatPhaseEdit(FormatLoopRuleTempID, CatalogID, FormatID, TransactionID, request.FacilityID, request.UserID, request.CultureID))
            {
                List<CatalogDetailTemp> _list = dt.ConvertToList<CatalogDetailTemp>();

                return _list;
            }
        }

        public static GenericReturn Delete(int CatalogDetailID, GenericRequest request)
        {
            return _rep.Delete(CatalogDetailID, request.FacilityID, request.UserID, request.CultureID);
        }

        public static CatalogDetail Get(int? CatalogDetailID, GenericRequest request)
        {
            using (DataTable dt = _rep.Get(CatalogDetailID, request))
            {
                List<CatalogDetail> _list = dt.ConvertToList<CatalogDetail>();
                if (_list == null)
                    return new CatalogDetail();
                else
                    return _list[0];
            }
        }

        public static GenericReturn Update(int? CatalogDetailID, string ValueID, string Param1, string Param2, string Param3, GenericRequest request)
        {
            return _rep.Update(CatalogDetailID, null, ValueID, Param1, Param2, Param3, "", request);
        }

        #endregion
    }
}
