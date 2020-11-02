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
    public static class MiscellaneousService
    {
        private static MiscellaneousRepository _rep;

        static MiscellaneousService()
        {
            _rep = new MiscellaneousRepository();
        }

        #region Methods

        public static string Param_GetValue(int CatalogDetailID, int ParamIndex, string DefaultValue)
        {
            return _rep.Param_GetValue(CatalogDetailID, ParamIndex, DefaultValue);
        }

        public static GenericReturn Params_SetValue(string ParamName, string ParamValue, GenericRequest request)
        {
            return _rep.Params_SetValue(ParamName, ParamValue, request);
        }

        public static bool ProductionProcess_ApplyLines(int CatalogDetailID, bool DefaultValue)
        {
            return _rep.ProductionProcess_ApplyLines(CatalogDetailID, DefaultValue);
        }

        public static string Param_GetValue(int FacilityID, string ParamName, string DefaultValue)
        {
            return _rep.Param_GetValue(FacilityID, ParamName, DefaultValue);
        }

        public static int Catalog_GetDetailID(int FacilityID, string CatalogTag, string ValueID, out string ErrorMessage)
        {
            ErrorMessage = "";

            try
            {
                return _rep.Catalog_GetDetailID(FacilityID, CatalogTag, ValueID);
            }
            catch (Exception ex)
            {
                ErrorMessage = "ERROR: " + ex.Message;
                return 0;
            }
        }

        public static int Catalog_GetID(int FacilityID, string CatalogTag, out string ErrorMessage)
        {
            ErrorMessage = "";

            try
            {
                // NOTA: La cultura no la requiere la funcion, se dejo por las dependencias de la misma
                return _rep.Catalog_GetID(FacilityID, CatalogTag, "");
            }
            catch (Exception ex)
            {
                ErrorMessage = "ERROR: " + ex.Message;
                return 0;
            }
        }       
      

        public static bool Event_ValidUser(int EventID, int UserID)
        {
            return _rep.Event_ValidUser(EventID, UserID);
        }

        public static bool Event_ValidUser_HasAccess(int EventID, int UserID)
        {
            return _rep.Event_ValidUser_HasAccess(EventID, UserID);
        }

        public static DateTime Facility_GetDate(int FacilityID)
        {
            return _rep.Facility_GetDate(FacilityID);
        }

        public static List<EntityField> EntityFieldsConfiguration(int? FacilityID, int? EntityID, int? EntityIndicator, string SystemModuleTag, string CultureID)
        {
            DataTable dt = _rep.EntityFieldsConfiguration(FacilityID, EntityID, EntityIndicator, SystemModuleTag, CultureID);
            List<EntityField> EntitiesList = dt.ConvertToList<EntityField>();
            return EntitiesList;
        }

        public static DataTable eReq_GetReportData(string SQL_StoredProcedure, int? DocumentID, string FileID, int? FacilityID, int? UserID, string CultureID, string Parameters, byte[] Logo = null)
        {
            return _rep.eReq_GetReportData(SQL_StoredProcedure, DocumentID, Int32.TryParse(FileID, out int i) ? i : 0, FacilityID, UserID, CultureID, Parameters, Logo);
        }

        public static DataTable eReq_GetReportDataDetail(string SQL_StoredProcedure, int? DocumentID, int? FacilityID, int? UserID, string CultureID, string Parameters, byte[] Logo = null)
        {
            return _rep.eReq_GetReportDataDetail(SQL_StoredProcedure, DocumentID, FacilityID, UserID, CultureID, Parameters, Logo);
        }

        #endregion
    }
}
