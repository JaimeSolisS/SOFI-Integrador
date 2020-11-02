using Core.Entities;
using Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.SG.Models.SecurityGuardReport
{
    public class IndexViewModel
    {
        public List<SecurityGuardLog> SecurityGuardLogList;
        public IEnumerable<SelectListItem> CheckInTypesList;
        public IEnumerable<SelectListItem> CheckInPersonTypesList;
        public IEnumerable<SelectListItem> CompaniesList;
        public DateTime CurrentDate;

        public IndexViewModel()
        {
            SecurityGuardLogList = new List<SecurityGuardLog>();
            CheckInTypesList = new SelectList(new List<SelectListItem>());
            CheckInPersonTypesList = new SelectList(new List<SelectListItem>());
            CompaniesList = new SelectList(new List<SelectListItem>());
            CurrentDate = DateTime.Now;
        }
    }
}