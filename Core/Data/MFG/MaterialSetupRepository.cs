﻿using Core.Entities;
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
					    db.AddInParameter(dbCommand, "@iMaterialSetupID", DbType.Int32,MaterialSetupID); 
					    // Execute Query
					    dt.Load(db.ExecuteReader(dbCommand));
				    }
				    finally
				    {dbCommand.Dispose();}
				    return dt;
	    }
    }
}