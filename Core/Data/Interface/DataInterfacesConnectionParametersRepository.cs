using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class DataInterfacesConnectionParametersRepository : GenericRepository
    {
        public DataTable List(int? DataInterfaceConnectionParameterID, int? DataInterfaceID, string ParameterName, string ParameterValue, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[DataInterfacesConnectionParameters_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDataInterfaceConnectionParameterID", DbType.Int32, DataInterfaceConnectionParameterID);
                db.AddInParameter(dbCommand, "@iDataInterfaceID", DbType.Int32, DataInterfaceID);
                db.AddInParameter(dbCommand, "@iParameterName", DbType.String, ParameterName);
                db.AddInParameter(dbCommand, "@iParameterValue", DbType.String, ParameterValue);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }
    }
}