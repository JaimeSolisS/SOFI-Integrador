using Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace Core.Data
{
    public class UserRepository : GenericRepository
    {
        public GenericReturn Login(string UserName, string Password, string CultureID, out bool NeedsPasswordChange, out string DefaultCultureID, out int BaseFacilityId, out int CompanyID, out int UserID, out string Firstname, out string Lastname)
        {
            GenericReturn result = new GenericReturn();
            NeedsPasswordChange = false;
            DefaultCultureID = string.Empty;
            Firstname = string.Empty;
            Lastname = string.Empty;
            BaseFacilityId = 0;
            UserID = 0;
            CompanyID = 0;
            dbCommand = db.GetStoredProcCommand("Login");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUserName", DbType.String, UserName);
                db.AddInParameter(dbCommand, "@iPassword", DbType.String, Password);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                db.AddOutParameter(dbCommand, "@oNeedsPasswordChange", DbType.Boolean, 1);
                db.AddOutParameter(dbCommand, "@oDefaultCultureID", DbType.String, 5);
                db.AddOutParameter(dbCommand, "@oBaseFacilityId", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oCompanyId", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oUserID", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oFirstname", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oLastname", DbType.String, 255);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);

                // Output parameters
                NeedsPasswordChange = (bool)db.GetParameterValue(dbCommand, "@oNeedsPasswordChange");
                DefaultCultureID = (string)db.GetParameterValue(dbCommand, "@oDefaultCultureID");
                BaseFacilityId = (int)db.GetParameterValue(dbCommand, "@oBaseFacilityId");
                CompanyID = (int)db.GetParameterValue(dbCommand, "@oCompanyId");
                UserID = (int)db.GetParameterValue(dbCommand, "@oUserID");
                Firstname = (string)db.GetParameterValue(dbCommand, "@oFirstname");
                Lastname = (string)db.GetParameterValue(dbCommand, "@oLastname");
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

        public DataTable AddAccountGuest(User entity, GenericRequest request)
        {
            try
            { // Get DbCommand to Execute the Insert Procedure
                using (dbCommand = db.GetStoredProcCommand("Users_AddAccountGuest"))
                {
                    // Parameters
                    db.AddInParameter(dbCommand, "@iUserAccountID", DbType.String, entity.UserAccountID);
                    db.AddInParameter(dbCommand, "@iFirstName", DbType.String, entity.FirstName);
                    db.AddInParameter(dbCommand, "@iLastName", DbType.String, entity.LastName);
                    db.AddInParameter(dbCommand, "@ieMail", DbType.String, entity.eMail);
                    db.AddInParameter(dbCommand, "@iEmployeeNumber", DbType.String, entity.EmployeeNumber);
                    db.AddInParameter(dbCommand, "@iChangedBy", DbType.String, entity.ChangedBy);
                    db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                    db.AddOutParameter(dbCommand, "@oUserID", DbType.Int32, 0);
                    db.AddOutParameter(dbCommand, "@oFacilityID", DbType.Int32, 0);
                    db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                    db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);

                    // Execute Query
                    db.ExecuteNonQuery(dbCommand);

                    // Output parameters
                    if ((int)db.GetParameterValue(dbCommand, "@oErrorCode") == 0)
                    {
                        var UserID = (int)db.GetParameterValue(dbCommand, "@oUserID");
                        var FacilityID = (int)db.GetParameterValue(dbCommand, "@oFacilityID");

                        using (DataTable result = Get(UserID, FacilityID, request.CultureID))
                        {
                            // Execute Query
                            return result;
                        }
                    }
                    else
                    {
                        throw new Exception((string)db.GetParameterValue(dbCommand, "@oErrorMessage"));
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public GenericReturn AddNewUserAccount(User entity, GenericRequest request)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("Users_AddAccountGuest");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUserAccountID", DbType.String, entity.UserAccountID);
                db.AddInParameter(dbCommand, "@iFirstName", DbType.String, entity.FirstName);
                db.AddInParameter(dbCommand, "@iLastName", DbType.String, entity.LastName);
                db.AddInParameter(dbCommand, "@ieMail", DbType.String, entity.eMail);
                db.AddInParameter(dbCommand, "@iEmployeeNumber", DbType.String, entity.EmployeeNumber);
                db.AddInParameter(dbCommand, "@iChangedBy", DbType.String, entity.ChangedBy);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, request.CultureID);
                db.AddOutParameter(dbCommand, "@oUserID", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oFacilityID", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);

                // Execute Query
                db.ExecuteNonQuery(dbCommand);

                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                result.ID = (int)db.GetParameterValue(dbCommand, "@oUserID");
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }
            finally
            {
                dbCommand.Dispose();
            }

            return result;
        }

        public DataTable Get(int UserID, int FacilityID, string CultureID)
        {
            dbCommand = db.GetStoredProcCommand("Users_Get");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);

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

        public DataTable GetInfo(string UserAccountID, int FacilityID, string CultureID)
        {
            dbCommand = db.GetStoredProcCommand("Users_GetInfo");
            try
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iUserAccountID", DbType.String, UserAccountID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);

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

        public DataTable UserMenus(int UserID, string CultureID)
        {
            // Get DbCommand to Execute the List Procedure
            dbCommand = db.GetStoredProcCommand("UserMenu");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUserID", DbType.String, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);

                // Execute Query                
                using (DataTable dt = new DataTable())
                {
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
            finally
            {
                dbCommand.Dispose();
            }
        }

        public DataTable UserFacilities(int UserID, int FacilityID, string CultureID)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("UserFacilities_Get");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                // Execute Query                
                dt.Load(db.ExecuteReader(dbCommand));
            }
            finally
            { dbCommand.Dispose(); }
            return dt;
        }

        public GenericReturn ProfilesUpdate(int? UserID, string SelectedProfilesID, int? FacilityID, int? ChangedBy, string CultureID)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("UsersProfiles_Update");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iSelectedProfilesID", DbType.String, SelectedProfilesID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iChangedBy", DbType.Int32, ChangedBy);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
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


        public int GetUserID(string UserName)
        {
            int UserID = 0;

            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetSqlStringCommand("SELECT dbo.Users_GetUserID(@iUserAccountID)");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUserAccountID", DbType.String, UserName);

                // Execute Query
                UserID = Convert.ToInt32(db.ExecuteScalar(dbCommand).ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
            }

            return UserID;
        }

        #region MISC Methods
        public GenericReturn Add(User _User)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("dbo.Users_Insert");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUserAccountID", DbType.String, _User.UserAccountID);
                db.AddInParameter(dbCommand, "@iFirstName", DbType.String, _User.FirstName);
                db.AddInParameter(dbCommand, "@iLastName", DbType.String, _User.LastName);
                db.AddInParameter(dbCommand, "@iProfileArrayID", DbType.String, _User.ProfileArrayIds);
                db.AddInParameter(dbCommand, "@ieMail", DbType.String, _User.eMail);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, _User.Enabled);
                db.AddInParameter(dbCommand, "@iDepartmentID", DbType.Int32, _User.DepartmentID);
                db.AddInParameter(dbCommand, "@iEmployeeNumber", DbType.String, _User.EmployeeNumber);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, _User.ShiftID);
                db.AddInParameter(dbCommand, "@iBaseFacilityID", DbType.Int32, _User.BaseFacilityID);
                db.AddInParameter(dbCommand, "@iDefaultCultureID", DbType.String, _User.DefaultCultureID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, _User.FacilityID);
                db.AddInParameter(dbCommand, "@iChangedBy", DbType.Int32, _User.ChangedBy);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, _User.CultureID);
                db.AddOutParameter(dbCommand, "@oUserID", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);

                // Execute Query
                db.ExecuteNonQuery(dbCommand);

                // Output parameters
                _User.ID = result.ID = (int)db.GetParameterValue(dbCommand, "@oUserID");
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
            }
            catch (Exception ex)
            {
                result.ErrorCode = 99;
                result.ErrorMessage = ex.Message;
            }
            finally
            {
                dbCommand.Dispose();
            }

            return result;
        }

        public GenericReturn Update(User _User)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("dbo.Users_Update");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, _User.ID);
                db.AddInParameter(dbCommand, "@iUserAccountID", DbType.String, _User.UserAccountID);
                db.AddInParameter(dbCommand, "@iFirstName", DbType.String, _User.FirstName);
                db.AddInParameter(dbCommand, "@iLastName", DbType.String, _User.LastName);
                db.AddInParameter(dbCommand, "@ieMail", DbType.String, _User.eMail);
                db.AddInParameter(dbCommand, "@iEnabled", DbType.Boolean, _User.Enabled);
                db.AddInParameter(dbCommand, "@iDepartmentID", DbType.Int32, _User.DepartmentID);
                db.AddInParameter(dbCommand, "@iEmployeeNumber", DbType.String, _User.EmployeeNumber);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, _User.ShiftID);
                db.AddInParameter(dbCommand, "@iBaseFacilityID", DbType.Int32, _User.BaseFacilityID);
                db.AddInParameter(dbCommand, "@iDefaultCultureID", DbType.String, _User.DefaultCultureID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, _User.FacilityID);
                db.AddInParameter(dbCommand, "@iChangedBy", DbType.Int32, _User.ChangedBy);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, _User.CultureID);
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
            {
                dbCommand.Dispose();
            }

            return result;
        }

        public GenericReturn QuickUpdate(int UserID, string ColumnName, string Value, int FacilityID, int ChangedBy, string CultureID)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Insert Procedure
            dbCommand = db.GetStoredProcCommand("dbo.Users_QuickUpdate");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iColumnName", DbType.String, ColumnName);
                db.AddInParameter(dbCommand, "@iValue", DbType.String, Value);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iChangedBy", DbType.Int32, ChangedBy);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
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
            {
                dbCommand.Dispose();
            }

            return result;
        }

        public GenericReturn Delete(int UserID, int FacilityID, int ChangedBy, string CultureID)
        {
            GenericReturn result = new GenericReturn();

            // Get DbCommand to Execute the Delete Procedure
            dbCommand = db.GetStoredProcCommand("dbo.Users_Delete");

            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iChangedBy", DbType.Int32, ChangedBy);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
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
            {
                dbCommand.Dispose();
            }

            return result;
        }

        public DataTable List(int? ProfileID, string UserAccountID, string eMail, int? DepartmentID, string EmployeeNumber, int? ShiftID, int? BaseFacilityID, int FacilityID, int UserID, string CultureID)
        {
            using (dbCommand = db.GetStoredProcCommand("Users_List"))
            {
                // Parameters    
                db.AddInParameter(dbCommand, "@iProfileID", DbType.Int32, ProfileID);
                db.AddInParameter(dbCommand, "@iUserAccountID", DbType.String, UserAccountID);
                db.AddInParameter(dbCommand, "@ieMail", DbType.String, eMail);
                db.AddInParameter(dbCommand, "@iDepartmentID", DbType.Int32, DepartmentID);
                db.AddInParameter(dbCommand, "@iEmployeeNumber", DbType.String, EmployeeNumber);
                db.AddInParameter(dbCommand, "@iShiftID", DbType.Int32, ShiftID);
                db.AddInParameter(dbCommand, "@iBaseFacilityID", DbType.Int32, BaseFacilityID);
                db.AddInParameter(dbCommand, "@iFacilityID", DbType.Int32, FacilityID);
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, UserID);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);

                using (DataTable dt = new DataTable())
                {
                    dt.Load(db.ExecuteReader(dbCommand));
                    return dt;
                }
            }
        }

        public GenericReturn IsValidPageAccess(int UserID, string NavigateTo, string CultureID)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("IsValidPageAccess");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUserID", DbType.String, UserID);
                db.AddInParameter(dbCommand, "@iNavigateTo", DbType.String, NavigateTo);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oMenuID", DbType.Int32, 0);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                result.ID = (int)db.GetParameterValue(dbCommand, "@oMenuID");
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

        public GenericReturn IsValidEventAccess(int UserID, string NavigateTo, string EventDescription, string CultureID)
        {
            GenericReturn result = new GenericReturn();
            // Get DbCommand to Execute the Update Procedure
            dbCommand = db.GetStoredProcCommand("IsValidEventAccess");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUserID", DbType.String, UserID);
                db.AddInParameter(dbCommand, "@iNavigateTo", DbType.String, NavigateTo);
                db.AddInParameter(dbCommand, "@iCultureID", DbType.String, CultureID);
                db.AddInParameter(dbCommand, "@iEventDescription", DbType.String, EventDescription);
                db.AddOutParameter(dbCommand, "@oErrorCode", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@oErrorMessage", DbType.String, 255);
                db.AddOutParameter(dbCommand, "@oEventID", DbType.Int32, 0);
                // Execute Query
                db.ExecuteNonQuery(dbCommand);
                // Output parameters
                result.ErrorCode = (int)db.GetParameterValue(dbCommand, "@oErrorCode");
                result.ErrorMessage = (string)db.GetParameterValue(dbCommand, "@oErrorMessage");
                result.ID = (int)db.GetParameterValue(dbCommand, "@oEventID");
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

        public DataTable UserFacilityAccess(int UserID, int FacilityID, string CultureID)
        {
            DataTable dt = new DataTable();
            // Get DbCommand to Execute the Procedure
            dbCommand = db.GetSqlStringCommand("SELECT F.OrganizationID,F.OrganizationName,F.CompanyID,F.CompanyName,F.FacilityID,F.FacilityName FROM [dbo].[t_UserFacilityAccess](@iUserID) F ORDER BY F.[FacilityName]");
            try
            {
                // Parameters
                db.AddInParameter(dbCommand, "@iUserID", DbType.Int32, FacilityID);
                dt.Load(db.ExecuteReader(dbCommand));
                // Execute Query
                //ParamValue = db.ExecuteScalar(dbCommand).ToString();
            }

            finally
            {
                dbCommand.Dispose();
            }

            return dt;
        }

        #endregion
    }
}
