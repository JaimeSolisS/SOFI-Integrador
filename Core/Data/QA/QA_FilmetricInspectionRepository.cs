using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    class QA_FilmetricInspectionRepository : GenericRepository
    {
        public DataTable List (int? FilmetricInspectionID, int? ProductID, int? SubstractID, int? BaseID, decimal? AdditionID, int? LineID, DateTime? StartDate, DateTime? EndDate, int? FacilityID, int? UserID, string CultureID){
	 DataTable dt = new DataTable();
        // Get DbCommand to Execute the Update Procedure
        dbCommand = db.GetStoredProcCommand("[QA].[FilmetricInspections_List]");
				try
				{
					// Parameters
					db.AddInParameter(dbCommand, "@iFilmetricInspectionID", DbType.Int32,FilmetricInspectionID); 
				db.AddInParameter(dbCommand, "@iProductID", DbType.Int32,ProductID); 
				db.AddInParameter(dbCommand, "@iSubstractID", DbType.Int32,SubstractID); 
				db.AddInParameter(dbCommand, "@iBaseID", DbType.Int32,BaseID); 
				db.AddInParameter(dbCommand, "@iAdditionID", DbType.Decimal,AdditionID); 
				db.AddInParameter(dbCommand, "@iLineID", DbType.Int32,LineID);
                db.AddInParameter(dbCommand, "@iStartDate", DbType.Date, StartDate);
                db.AddInParameter(dbCommand, "@iEndDate", DbType.Date, EndDate);
				db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32,FacilityID); 
				db.AddInParameter(dbCommand, "@iUserID", DbType.Int32,UserID); 
				db.AddInParameter(dbCommand, "@iCultureID", DbType.String,CultureID);
					// Execute Query
					dt.Load(db.ExecuteReader(dbCommand));
				}
				finally
				{dbCommand.Dispose();}
				return dt;
	}
        public GenericReturn Insert(int? ProductID, int? MaterialID, int? SubstractID, int? BaseID, int? AdditionID, int? LineID, int? UserID, decimal? HcValue, decimal? BcValue, decimal? PuValue, decimal? EuValue, GenericRequest req)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("QA.FilmetricInspections_Insert");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iProductID", DbType.Int32, ProductID);
                db.AddInParameter(dbCommand, "@iSubstractID", DbType.Int32, SubstractID);
                db.AddInParameter(dbCommand, "@iBaseID", DbType.Int32, BaseID);
                db.AddInParameter(dbCommand, "@iAdditionID", DbType.Int32, AdditionID);
                db.AddInParameter(dbCommand, "@iMaterialID", DbType.Int32, MaterialID);
                db.AddInParameter(dbCommand, "@iLineID", DbType.Int32, LineID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, req.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, req.UserID);
                db.AddInParameter(dbCommand, "@iHcvalue", DbType.Decimal, HcValue);
                db.AddInParameter(dbCommand, "@iEuvalue", DbType.Decimal, EuValue);
                db.AddInParameter(dbCommand, "@iPuvalue", DbType.Decimal, PuValue);
                db.AddInParameter(dbCommand, "@iBcvalue", DbType.Decimal, BcValue);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, req.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");

            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }
            finally
            { dbCommand.Dispose(); }
            return result;
        }

        public DataTable Details_List (int? FilmetricInspectionDetailID, int? FilmetricInspectionID, int? FilmID,int? ProductID, decimal? Value, GenericRequest request){
	 DataTable dt = new DataTable();
        // Get DbCommand to Execute the Update Procedure
        dbCommand = db.GetStoredProcCommand("[QA].[FilmetricInspectionDetails_List]");
				try
				{
					// Parameters
				db.AddInParameter(dbCommand, "@iFilmetricInspectionDetailID", DbType.Int32,FilmetricInspectionDetailID); 
				db.AddInParameter(dbCommand, "@iFilmetricInspectionID", DbType.Int32,FilmetricInspectionID); 
				db.AddInParameter(dbCommand, "@iFilmID", DbType.Int32,FilmID);
                db.AddInParameter(dbCommand, "@iProductID", DbType.Int32,ProductID);
                db.AddInParameter(dbCommand, "@iValue", DbType.Decimal,Value); 
				db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32,request.FacilityID); 
				db.AddInParameter(dbCommand, "@iUserID", DbType.Int32,request.UserID); 
				db.AddInParameter(dbCommand, "@iCultureID", DbType.String,request.CultureID);

					// Execute Query
					dt.Load(db.ExecuteReader(dbCommand));
				}
				finally
				{dbCommand.Dispose();}
				return dt;
	}



    }
}
