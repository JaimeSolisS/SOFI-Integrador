namespace Core.Data
{
    #region Namespaces

    using Entities;
    using System;
    using System.Data;

    #endregion

    public class DepartmentRepository : GenericRepository
    {
        #region CRUD Methods

        public DataTable List(int? OrganizationID, int? DepartmentID, bool? Enabled, int FacilityID, int UserID, string CultureID)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("Departments_List");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iOrganizationID", DbType.Int32, OrganizationID);
                db.AddInParameter(dbCommand, "@iDepartmentID", DbType.Int32, DepartmentID);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);

                // Execute Query
                using (DataTable dt = new DataTable())
                {
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
            }
        }
        public DataTable List4Facility(int FacilityID, GenericRequest req)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[Departments_List4Facility]");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, req.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, req.CultureID);

                // Execute Query
                using (DataTable dt = new DataTable())
                {
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
            }
        }
        #endregion

    }
}
