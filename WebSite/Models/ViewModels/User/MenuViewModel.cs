using Core.Entities;
using System.Collections.Generic;

namespace WebSite.Models.ViewModels.User
{
    public class MenuViewModel
    {
        #region Properties

        public Core.Entities.User CurrentUser { get; set; }

        public IEnumerable<AppMenu> Menus { get; set; }

        #endregion

        public MenuViewModel()
        {
            CurrentUser = new Core.Entities.User();
        }
    }
}