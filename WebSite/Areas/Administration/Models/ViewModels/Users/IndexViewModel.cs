using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.Administration.Models.ViewModels.Users
{
    public class IndexViewModel
    {
        #region Properties

        public IEnumerable<SelectListItem> ProfilesList;
        public IEnumerable<SelectListItem> DepartmentsList;
        public IEnumerable<SelectListItem> ShiftsList;
        public IEnumerable<SelectListItem> FacilitiesList;
        //public IEnumerable<SelectListItem> CompaniesList;

        public List<User> _List_Users;

        #endregion

        public IndexViewModel()
        {
            ProfilesList = new SelectList(new List<Profile>());
            DepartmentsList = new SelectList(new List<Department>());
            ShiftsList = new SelectList(new List<ShiftsMaster>());
            FacilitiesList = new SelectList(new List<Facility>());

            _List_Users = new List<User>();
        }
    }
}