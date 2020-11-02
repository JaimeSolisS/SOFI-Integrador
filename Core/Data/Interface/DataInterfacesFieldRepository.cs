using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class DataInterfacesFieldRepository : GenericRepository
    {

        #region CRUD
        public DataTable List(int DataInterfaceID)
        {
            try
            {
                using (dbCommand = db.GetStoredProcCommand("DataInterfacesFields_List"))
                {
                    // Parameters
                    db.AddInParameter(dbCommand, "@iDataInterfaceID", DbType.Int32, DataInterfaceID);

                    // Execute Query
                    using (DataTable dt = new DataTable())
                    {
                        dt.Load(db.ExecuteReader(dbCommand));
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
