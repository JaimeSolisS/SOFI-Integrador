using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.Administration.Models.ViewModels.Users
{
    public class EditViewModel
    {
        #region Properties

        public IEnumerable<SelectListItem> OrganizationsList;
        public IEnumerable<SelectListItem> StorageLocationsList;
        public User _User;
        public IEnumerable<SelectListItem> _UserWarehouseAccess;
        public IEnumerable<int> SelectedProfiles { get; set; }
        public IEnumerable<SelectListItem> ProfilesList { get; set; }

        public IEnumerable<SelectListItem> CompaniesList;
        public List<UserFacility> CompaniesPlantsTable;
        public List<ProductionLine> ProductionLinesList;
        public List<UsersProcessLine> ProductionLinesUserTable;



        #endregion

        public EditViewModel()
        {
            OrganizationsList = new SelectList(new List<Organization>());
            _User = new User();
            SelectedProfiles = new List<int>();
            ProfilesList = new SelectList(new List<Profile>());

            CompaniesList = new SelectList(new List<Company>());
            CompaniesPlantsTable = new List<UserFacility>();
            ProductionLinesList = new List<ProductionLine>();
            ProductionLinesUserTable = new List<UsersProcessLine>();
        }

    }
}