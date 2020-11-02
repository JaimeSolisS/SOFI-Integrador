using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.Administration.Models.ViewModels.Users
{
    public class CreateViewModel
    {
        #region Properties

        public IEnumerable<SelectListItem> ProfilesList;
        public IEnumerable<SelectListItem> DepartmentsList;
        public IEnumerable<SelectListItem> ShiftsList;
        public IEnumerable<SelectListItem> CulturesList;
        public bool IsModal;
        public string Search;

        #endregion

        public CreateViewModel()
        {
            ProfilesList = new SelectList(new List<Profile>());
            DepartmentsList = new SelectList(new List<Department>());
            ShiftsList = new SelectList(new List<ShiftsMaster>());
            CulturesList = new SelectList(new List<Catalog>());
            IsModal = false;
            Search = "";
        }
    }
}