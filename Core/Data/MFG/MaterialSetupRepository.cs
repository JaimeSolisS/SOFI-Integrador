using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    class MaterialSetupRepository : GenericRepository
    {
        public DataTable List (int? MaterialSetupID, int? MachineID, int? MaterialID, int? MachineSetupID, GenericRequest request){
	     DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[MaterialSetups_List]");
				    try
				    {
					    // Parameters
					    db.AddInParameter(dbCommand, "@iMaterialSetupID", DbType.Int32,MaterialSetupID); 				        db.AddInParameter(dbCommand, "@iMachineID", DbType.Int32,MachineID); 				        db.AddInParameter(dbCommand, "@iMaterialID", DbType.Int32,MaterialID); 				        db.AddInParameter(dbCommand, "@iMachineSetupID", DbType.Int32,MachineSetupID); 				        db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32,request.FacilityID); 				        db.AddInParameter(dbCommand, "@iUserID", DbType.Int32,request.UserID); 				        db.AddInParameter(dbCommand, "@iCultureID", DbType.String,request.CultureID);
					    // Execute Query
					    dt.Load(db.ExecuteReader(dbCommand));
				    }
				    finally
				    {dbCommand.Dispose();}
				    return dt;
	    }
    }
}
