using Core.Entities;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebSite.Areas.HR.Models.ViewModels.KioskSuggestionsAdministrator
{
    public class IndexViewModel
    {
        public IEnumerable<SelectListItem> CategoriesList;
        public IEnumerable<SelectListItem> FacilitiesList;
        public List<KioskEmployeeSuggestion> SuggestionsList;
        public int SugestionsHistoryDays;

        public IndexViewModel()
        {
            CategoriesList = new SelectList(new List<SelectListItem>());
            FacilitiesList = new SelectList(new List<SelectListItem>());
            SuggestionsList = new List<KioskEmployeeSuggestion>();
            SugestionsHistoryDays = 30;
        }
    }
}