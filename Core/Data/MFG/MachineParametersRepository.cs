using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Data
{
    public class MachineParametersRepository : GenericRepository
    {
        public DataTable List(int? MachineParameterID, string ParameterName, int? ParameterTypeID, int? ParameterLength, int? ParameterPrecision, int? ParameterListID, bool? UseReference, string ReferenceName, bool? IsCavity, bool? Enabled, GenericRequest request)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.MachineParameters_List");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iMachineParameterID", DbType.Int32, MachineParameterID);
                db.AddInParameter(dbCommand, "@iParameterName", DbType.String, ParameterName);
                db.AddInParameter(dbCommand, "@iParameterTypeID", DbType.Int32, ParameterTypeID);
                db.AddInParameter(dbCommand, "@iParameterLength", DbType.Int32, ParameterLength);
                db.AddInParameter(dbCommand, "@iParameterPrecision", DbType.Int32, ParameterPrecision);
                db.AddInParameter(dbCommand, "@iParameterListID", DbType.Int32, ParameterListID);
                db.AddInParameter(dbCommand, "@iUseReference", DbType.Boolean, UseReference);
                db.AddInParameter(dbCommand, "@iReferenceName", DbType.String, ReferenceName);
                db.AddInParameter(dbCommand, "@iIsCavity", DbType.Boolean, IsCavity);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
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

        public GenericReturn Insert(string ParameterName, int? ParameterTypeID, int? ParameterLength, int? ParameterPrecision, string ParameterTag, bool? UseReference, string Reference, int? ReferenceTypeID, int? ReferenceListID, bool? IsCavity, bool? Enabled, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.MachineParameters_Insert");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iParameterName", DbType.String, ParameterName);
                db.AddInParameter(dbCommand, "@iParameterTypeID", DbType.Int32, ParameterTypeID);
                db.AddInParameter(dbCommand, "@iParameterLength", DbType.Int32, ParameterLength);
                db.AddInParameter(dbCommand, "@iParameterPrecision", DbType.Int32, ParameterPrecision);
                //db.AddInParameter(dbCommand, "@iParameterListID", DbType.Int32, ParameterListID);
                db.AddInParameter(dbCommand, "@iCatalogTag", DbType.String, ParameterTag);
                db.AddInParameter(dbCommand, "@iUseReference", DbType.Boolean, UseReference);
                db.AddInParameter(dbCommand, "@iReferenceName", DbType.String, Reference);
                db.AddInParameter(dbCommand, "@iReferenceTypeID", DbType.Int32, ReferenceTypeID);
                db.AddInParameter(dbCommand, "@iReferenceListID", DbType.Int32, ReferenceListID);
                db.AddInParameter(dbCommand, "@iIsCavity", DbType.Boolean, IsCavity);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oMachineParameterID", DbType.Int32, 0);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                result.ID = (int)db.GetParameterValue(dbCommand, "@oMachineParameterID");
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

        public GenericReturn Delete(int? MachineParameterID, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("[MFG].[MachineParameters_Delete]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iMachineParameterID", DbType.Int32, MachineParameterID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                //result.ID = (int)db.GetParameterValue(dbCommand, "@oID");
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

        public GenericReturn Update(int? MachineParameterID, string ParameterName, int? ParameterTypeID, int? ParameterLength, int? ParameterPrecision, int? ParameterListID, bool? UseReference, string ReferenceName, int? ReferenceTypeID, int? ReferenceListID, string FunctionValue, bool? IsCavity, bool? Enabled, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("MFG.MachineParameters_Update");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iMachineParameterID", DbType.Int32, MachineParameterID);
                db.AddInParameter(dbCommand, "@iParameterName", DbType.String, ParameterName);
                db.AddInParameter(dbCommand, "@iParameterTypeID", DbType.Int32, ParameterTypeID);
                db.AddInParameter(dbCommand, "@iParameterLength", DbType.Int32, ParameterLength);
                db.AddInParameter(dbCommand, "@iParameterPrecision", DbType.Int32, ParameterPrecision);
                db.AddInParameter(dbCommand, "@iParameterListID", DbType.Int32, ParameterListID);
                db.AddInParameter(dbCommand, "@iUseReference", DbType.Boolean, UseReference);
                db.AddInParameter(dbCommand, "@iReferenceName", DbType.String, ReferenceName);
                db.AddInParameter(dbCommand, "@iReferenceTypeID", DbType.Int32, ReferenceTypeID);
                db.AddInParameter(dbCommand, "@iReferenceListID", DbType.Int32, ReferenceListID);
                db.AddInParameter(dbCommand, "@iFunctionValue", DbType.String, FunctionValue);
                db.AddInParameter(dbCommand, "@iIsCavity", DbType.Boolean, IsCavity);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, Enabled);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, request.FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, request.UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                //result.ID = (int)db.GetParameterValue(dbCommand, "@oID");
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
    }
}
