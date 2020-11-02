using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class CheckListRepository : GenericRepository
    {
        public DataTable Templates_List(int? CheckListTemplateID, string CheckListName, bool? Enabled, int? OrganizationID, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[CheckListTemplates_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCheckListTemplateID", DbType.Int32, CheckListTemplateID);
                db.AddInParameter(dbCommand, "@iCheckListName", DbType.String, CheckListName);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
                db.AddInParameter(dbCommand, "@iOrganizationID", DbType.Int32, OrganizationID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
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
        public DataTable TemplatesDetail_List(int? CheckListTemplateDetailID, int? CheckListTemplateID, string CheckListName, int? Seq, string Question, GenericRequest request)
        {
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[dbo].[CheckListTemplatesDetail_List]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iCheckListTemplateDetailID", DbType.Int32, CheckListTemplateDetailID);
                db.AddInParameter(dbCommand, "@iCheckListTemplateID", DbType.Int32, CheckListTemplateID);
                db.AddInParameter(dbCommand, "@iCheckListName", DbType.String, CheckListName);
                db.AddInParameter(dbCommand, "@iSeq", DbType.Int32, Seq);
                db.AddInParameter(dbCommand, "@iQuestion", DbType.String, Question);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
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
    }
}
