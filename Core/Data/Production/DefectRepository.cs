using Core.Entities;
using Core.Entities.Production;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class DefectRepository : GenericRepository
    {
        public DataTable Defects_SelectList(int? DefectID, string DefectName, int? ProductionProcessID, int? FacilityID, int? UserID, string CultureID, bool Enabled)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.[Defects_SelectList]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDefectID", DbType.Int32, DefectID);
                db.AddInParameter(dbCommand, "@iDefectName", DbType.String, DefectName);
                db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, ProductionProcessID);
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

        public DataTable Defects_List(Defect defect, DefectProcess defectProcess, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.Defects_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iDefectID", DbType.Int32, defect.DefectID);
                db.AddInParameter(dbCommand, "@iDefectName", DbType.String, defect.DefectName);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, defect.Enabled);
                db.AddInParameter(dbCommand, "@iProductionProcessID", DbType.Int32, defectProcess.ProductionProcessID);
                db.AddInParameter(dbCommand, "@iProductionLineID", DbType.Int32, defectProcess.ProductionLineID);
                db.AddInParameter(dbCommand, "@iVAID", DbType.Int32, defectProcess.VAID);
                db.AddInParameter(dbCommand, "@iDesignID", DbType.Int32, defectProcess.DesignID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.String, defectProcess.ShiftID);
                // Execute Query
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }
    }
}
