//using Core.Entities;
using Core.Entities;
using System;
using System.Data;


namespace Core.Data
{
     class DashboardRepository :GenericRepository
    {
        public DataTable Dashboard_ASNChart(DateTime? ChartDate, int ProductionProcessID,int LineID, int ShiftID, int? FacilityID, int? UserID, string CultureID)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].Dashboard_ASNChart");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iChartDate", DbType.DateTime, ChartDate);
                db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, ProductionProcessID);
                db.AddInParameter(dbCommand, "@iLineID", DbType.Int32, LineID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }

        public DataTable Dashboard_ProductionGoalsHours(DateTime? Date, int? GoalNameID, int? ProductionProcessID, int? LineID, int? VAID, int? FacilityID, int? UserID, string CultureID)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].Dashboard_ProductionGoalsHours");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDate", DbType.DateTime, Date);
                db.AddInParameter(dbCommand, "@iGoalNameID", DbType.Int32, GoalNameID);
                db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, ProductionProcessID);
                db.AddInParameter(dbCommand, "@iLineID", DbType.Int32, LineID);
                db.AddInParameter(dbCommand, "@iVAID", DbType.Int32, VAID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }

        public DataTable Dashboard_ProductionChart(DateTime? ChartDate, int  ProductionProcessID,int LineID,int? VAID,int? DesignID, int? ShiftID, int? FacilityID, int? UserID, string CultureID)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].Dashboard_ProductionChart");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iChartDate", DbType.DateTime, ChartDate);
                db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, ProductionProcessID);
                db.AddInParameter(dbCommand, "@iLineID", DbType.Int32, LineID);
                db.AddInParameter(dbCommand, "@iVAID", DbType.Int32, VAID);
                db.AddInParameter(dbCommand, "@iDesignID", DbType.Int32, DesignID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }

        public DataSet Dashboard_YieldDefectsChart(DateTime? ChartDate, int ProductionProcessID, int? LineID, int? VA, int? DesignID, int? ShiftID, int? Top, int? FacilityID, int? UserID, string CultureID)
        {
            DataSet ds = new DataSet();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].Dashboard_YieldDefectsChart");
            dbCommand.CommandTimeout = 120;
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iChartDate", DbType.DateTime, ChartDate);
                db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, ProductionProcessID);
                db.AddInParameter(dbCommand, "@iLineID", DbType.Int32, LineID);
                db.AddInParameter(dbCommand, "@iVA", DbType.Int32, VA);
                db.AddInParameter(dbCommand, "@iDesignID", DbType.Int32, DesignID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iTop", DbType.Int32, Top);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                // Execute Query
                ds = db.ExecuteDataSet(dbCommand);
            }
            finally
            { dbCommand.Dispose(); }
            return ds;
        }

        public DataTable Dashboard_TopDefectsChart(DateTime? ChartDate, int ProductionProcessID, int? LineID, int? VA, int? DesignID, string Defects, int? ShiftID, int? FacilityID, int? UserID, string CultureID)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].Dashboard_TopDefectsChart");
            dbCommand.CommandTimeout = 60;
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iChartDate", DbType.DateTime, ChartDate);
                db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, ProductionProcessID);
                db.AddInParameter(dbCommand, "@iLineID", DbType.Int32, LineID);
                db.AddInParameter(dbCommand, "@iVAID", DbType.Int32, VA);
                db.AddInParameter(dbCommand, "@iDesignID", DbType.Int32, DesignID);
                db.AddInParameter(dbCommand, "@iDefects", DbType.String, Defects);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }

        public DataTable Dashboard_DefectChart(DateTime? ChartDate, int ProductionProcessID, int? LineID, int? VA, int? DesignID, int? ShiftID, string Defect, int? FacilityID, int? UserID, string CultureID)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].Dashboard_DefectChart");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iChartDate", DbType.DateTime, ChartDate);
                db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, ProductionProcessID);
                db.AddInParameter(dbCommand, "@iLineID", DbType.Int32, LineID);
                db.AddInParameter(dbCommand, "@iVAID", DbType.Int32, VA);
                db.AddInParameter(dbCommand, "@iDesignID", DbType.Int32, DesignID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iDefect", DbType.String, Defect);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }

        public DataTable Dashboard_MoldScrapChart(DateTime? ChartDate, int ProductionProcessID, int? LineID,int? DesignID,int ShiftID, int? FacilityID, int? UserID, string CultureID)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[Dashboard_MoldScrapChart]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iChartDate", DbType.DateTime, ChartDate);
                db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, ProductionProcessID);
                db.AddInParameter(dbCommand, "@iLineID", DbType.Int32, LineID);
                db.AddInParameter(dbCommand, "@iDesignID", DbType.Int32, DesignID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }

        public DataTable Dashboard_GetShiftHours(int? ShiftID, int? FacilityID, int? UserID, string CultureID)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("Dashboard_GetShiftHours");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }

        public DataTable Dashboard_GetYieldGoal(DateTime? ChartDate, int? ProductionProcessID, int? LineID, int? VA, int? DesignID, int? ShiftID, int? Top, int? FacilityID, int? UserID, string CultureID)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[Dashboard_GetYieldGoal]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iChartDate", DbType.DateTime, ChartDate);
                db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, ProductionProcessID);
                db.AddInParameter(dbCommand, "@iLineID", DbType.Int32, LineID);
                db.AddInParameter(dbCommand, "@iVA", DbType.Int32, VA);
                db.AddInParameter(dbCommand, "@iDesignID", DbType.Int32, DesignID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iTop", DbType.Int32, Top);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                // Execute Query
                //db.ExecuteNonQuery(dbCommand);
                dt.Load(db.ExecuteReader(dbCommand));
                // Output parameters
                //result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                //result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                //result.ID = (int)db.GetParameterValue(dbCommand, "@oID");
            }
            //catch (Exception ex)
            //{
            //    result.ErrorCode = 99;
            //    result.ErrorMessage = ex.Message;
            //}
            finally
            { dbCommand.Dispose(); }
            return dt;
        }

        public int GetCurrentShiftID(int FacilityID, int UserID, string CultureID,int DefaultValue)
        {
            int ParamValue = DefaultValue;
            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetSqlStringCommand("SELECT [MFG].[fn_GetCurrentShiftID] (@iFacilityID,@iUserID,@iCultureID)");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                // Execute Query
                ParamValue = Int32.Parse(db.ExecuteScalar(dbCommand).ToString());
            }
            catch
            {
                ParamValue = DefaultValue;
            }
            finally
            {
                dbCommand.Dispose();
            }
            return ParamValue;
        }
        public string GetYieldChartTitle(DateTime? ChartDate,int ProductionProcessID, int? LineID, int? VAID, int? DesignID, int ShiftID,int FacilityID, int UserID, string CultureID, string DefaultValue)
        {
            string ParamValue = DefaultValue;
            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetSqlStringCommand("SELECT [MFG].[fn_Dashboard_GetYieldChartTitle](@iChartDate,@iProductionProcessID,@iLineID,@iVAID,@iDesignID,@iShiftID,@iFacilityID,@iUserID,@iCultureID)");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iChartDate", DbType.Date, ChartDate);
                db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, ProductionProcessID);
                db.AddInParameter(dbCommand, "@iLineID", DbType.Int32, LineID);
                db.AddInParameter(dbCommand, "@iVAID", DbType.Int32, VAID);
                db.AddInParameter(dbCommand, "@iDesignID", DbType.Int32, DesignID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                // Execute Query
                ParamValue = db.ExecuteScalar(dbCommand).ToString();
            }
            catch
            {
                ParamValue = DefaultValue;
            }
            finally
            {
                dbCommand.Dispose();
            }
            return ParamValue;
        }


        public DataSet Dashboard_TicketsChart(int? Year, int? FacilityID, int? UserID, string CultureID)
        {
            DataSet ds = new DataSet();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[IT].Dashboard_TicketsChart");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iYear", DbType.Int32, Year);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                // Execute Query
                ds = db.ExecuteDataSet(dbCommand);
            }
            finally
            { dbCommand.Dispose(); }
            return ds;
        }
        public DataTable Dashboard_ProjectsChart(int? Year,int? StatusID, int? FacilityID, int? UserID, string CultureID)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[IT].[Dashboard_ProjectsChart]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iYear", DbType.Int32, Year);
                db.AddInParameter(dbCommand, "@iStatusID", DbType.Int32, StatusID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));

            }

            finally
            { dbCommand.Dispose(); }
            return dt;
        }
        public DataSet Dashboard_ServicesChart(int? Year, int? FacilityID, int? UserID, string CultureID)
        {
            DataSet ds = new DataSet();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[IT].[Dashboard_ServicesChart]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iYear", DbType.Int32, Year);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                // Execute Query
                ds = db.ExecuteDataSet(dbCommand);

            }

            finally
            { dbCommand.Dispose(); }
            return ds;
        }
        public DataTable Dashboard_GetYearMonths(int? Year, int? FacilityID, int? UserID, string CultureID)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("Dashboard_GetYearMonths");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iYear", DbType.Int32, Year);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }
        public DataTable Dashboard_TopTicketsChart(int? Year, int? Month, int? Top,int? FacilityID, int? UserID, string CultureID)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[IT].[Dashboard_TopTicketsChart]");
            dbCommand.CommandTimeout = 60;
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iYear", DbType.Int32, Year);
                db.AddInParameter(dbCommand, "@iMonth", DbType.Int32, Month);
                db.AddInParameter(dbCommand, "@iTop", DbType.Int32, Top);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }
    }
}
