using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public static class UserService
    {
        private static UserRepository _rep;

        static UserService()
        {
            _rep = new UserRepository();
        }

        public static GenericReturn Login(string UserName, string Password, string CultureID, out bool NeedsPasswordChange, out string DefaultCultureID, out int BaseFacilityId, out int CompanyId, out int UserID, out string Firstname, out string Lastname)
        {
            return _rep.Login(UserName, Password, CultureID, out NeedsPasswordChange, out DefaultCultureID, out BaseFacilityId, out CompanyId, out UserID, out Firstname, out Lastname);
        }

        public static User Get(int UserID, GenericRequest req)
        {
            using (DataTable dt = _rep.Get(UserID, req.FacilityID, req.CultureID))
            {
                List<User> _list = dt.ConvertToList<User>();
                return _list.FirstOrDefault();
            }
        }

        public static User GetInfo(string UserAccountID, GenericRequest req)
        {
            using (DataTable dt = _rep.GetInfo(UserAccountID, req.FacilityID, req.CultureID))
            {
                List<User> _list = dt.ConvertToList<User>();
                if (_list != null && _list.Count > 0)
                {
                    return _list.FirstOrDefault();
                }

                return null;
            }
        }

        public static GenericReturn AddNewUSerAccount(User entity, GenericRequest req)
        {
            return _rep.AddNewUserAccount(entity, req);
        }

        public static User AddAccountGuest(User entity, GenericRequest req)
        {
            using (DataTable dt = _rep.AddAccountGuest(entity, req))
            {
                List<User> _list = dt.ConvertToList<User>();
                if (_list != null && _list.Count > 0)
                {
                    return _list.FirstOrDefault();
                }

                return null;
            }
        }

        public static List<AppMenu> UserMenus(int UserID, string CultureID)
        {
            List<AppMenu> EntitiesList = new List<AppMenu>();
            foreach (DataRow r in _rep.UserMenus(UserID, CultureID).Rows)
            {
                EntitiesList.Add(new AppMenu
                {
                    MenuID = Int32.Parse(r["MenuID"].ToString()),
                    ParentMenuID = Int32.Parse(r["ParentMenuID"].ToString()),
                    Description = r["Description"].ToString(),
                    NavigateTo = r["NavigateTo"].ToString(),
                    Sequence = Int32.Parse(r["Sequence"].ToString()),
                    Icon = r["Icon"] != null ? r["Icon"].ToString() : "/Content/img/Menu/menu.png"
                });
            }
            return EntitiesList;
        }

        public static List<Facility> GetFacilities(int UserID, int FacilityID, string CultureID)
        {
            DataTable dt = _rep.UserFacilities(UserID, FacilityID, CultureID);
            List<Facility> EntitiesList = dt.ConvertToList<Facility>();
            return EntitiesList;
        }

        public static GenericReturn ProfilesUpdate(int? UserID, string SelectedProfilesID, GenericRequest request)
        {
            return _rep.ProfilesUpdate(UserID, SelectedProfilesID, request.FacilityID, request.UserID, request.CultureID);
        }

        public static int GetUserID(string UserName)
        {
            try
            {
                return _rep.GetUserID(UserName);
            }
            catch
            {
                return 0;
            }
        }

        #region CRUD
        public static GenericReturn Add(User _User)
        {
            // Manejo de nulos
            _User.DepartmentID = _User.DepartmentID.SelectedValue();
            _User.ShiftID = _User.ShiftID.SelectedValue();
            _User.BaseFacilityID = _User.BaseFacilityID.SelectedValue();

            return _rep.Add(_User);
        }

        public static GenericReturn Update(User _User)
        {
            // Manejo de nulos
            _User.DepartmentID = _User.DepartmentID.SelectedValue();
            _User.ShiftID = _User.ShiftID.SelectedValue();
            _User.BaseFacilityID = _User.BaseFacilityID.SelectedValue();

            return _rep.Update(_User);
        }

        public static GenericReturn QuickUpdate(int UserID, string ColumnName, string Value, int FacilityID, int ChangedBy, string CultureID)
        {
            return _rep.QuickUpdate(UserID, ColumnName, Value, FacilityID, ChangedBy, CultureID);
        }

        public static GenericReturn Delete(int UserID, int FacilityID, int ChangedBy, string CultureID)
        {
            return _rep.Delete(UserID, FacilityID, ChangedBy, CultureID);
        }

        public static List<User> List(int? ProfileID, string UserAccountID, string eMail, int? DepartmentID, string EmployeeNumber, int? ShiftID, int? BaseFacilityID, int FacilityID, int UserID, string CultureID)
        {
            using (DataTable dt = _rep.List(ProfileID.SelectedValue(), UserAccountID, eMail, DepartmentID.SelectedValue(), EmployeeNumber,
                                         ShiftID.SelectedValue(), BaseFacilityID.SelectedValue(), FacilityID, UserID, CultureID))
            {
                List<User> _List = dt.ConvertToList<User>(); ;

                if (_List == null) { _List = new List<User>(); }

                return _List;
            }
        }
        #endregion

        #region Methods

        public static List<Facility> ListFacilities4Select(GenericRequest req, bool EmptyFirst = true)
        {
            using (DataTable dt = _rep.UserFacilityAccess(req.CompanyID, req.UserID, req.CultureID))
            {
                List<Facility> _list = dt.ConvertToList<Facility>();
                if (EmptyFirst)
                {
                    // Anexar fila vacia al principio
                    _list.Insert(0, new Facility() { FacilityID = 0, FacilityName = "" });
                }
                return _list;
            }
        }

        public static bool IsValidPageAccess(int UserID, string NavigateTo, string CultureID)
        {
            if (_rep.IsValidPageAccess(UserID, NavigateTo, CultureID).ErrorCode == 0)
            {
                return true;
            }
            return false;
        }

        public static bool IsValidEventAccess(int UserID, string NavigateTo, string CultureID, string Event)
        {
            if (_rep.IsValidEventAccess(UserID, NavigateTo, Event, CultureID).ErrorCode == 0)
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
