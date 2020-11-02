using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class TableAdditionalFieldsRepository : GenericRepository
    {
        #region Methods
        public DataTable List(int? ReferenceID, string ModuleName, int? OrganizationID, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("TableAdditionalFields_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iReferenceID", DbType.Int32, ReferenceID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iModuleName", DbType.String, ModuleName);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }
        public DataTable ListConfiguration(int? ReferenceID, int FormatID, string ModuleName, GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("[TableAdditionalFields_FieldsConfiguration]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFormatID", DbType.Int32, FormatID);
                db.AddInParameter(dbCommand, "@iReferenceID", DbType.Int32, ReferenceID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iModuleName", DbType.String, ModuleName);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);

                using (DataTable dt = new DataTable())
                {
                    // Execute Query
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            finally
            { dbCommand.Dispose(); }

        }
        public DataTable ListDetail(int RequestID, int FormatID, GenericRequest request)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("[eReq].[TableDescriptionFields_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iRequestID", DbType.Int32, RequestID);
                db.AddInParameter(dbCommand, "@iFormatID", DbType.Int32, FormatID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);

                // Execute Query
                using (DataTable dt = new DataTable())
                {
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            finally
            { dbCommand.Dispose(); }

        }

        public DataTable List4Column(int? ReferenceID, string ModuleName, string ColumnName, GenericRequest request)
        {

            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[TableAdditionalFields_4ColumnName]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iReferenceID", DbType.Int32, ReferenceID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iModuleName", DbType.String, ModuleName);
                db.AddInParameter(dbCommand, "@iColumnName", DbType.String, ColumnName);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                // Execute Query
                using (DataTable dt = new DataTable())
                {
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }

            }
            finally
            { dbCommand.Dispose(); }

        }
        #endregion
    }
}
