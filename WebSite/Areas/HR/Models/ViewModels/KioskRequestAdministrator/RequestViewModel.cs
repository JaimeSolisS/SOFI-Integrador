using Core.Entities;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WebSite.Areas.HR.Models.ViewModels.KioskRequestAdministrator
{
    public class RequestViewModel
    {

        public string Employee;
        public string Department;
        public string Type;
        public DateTime? OpenDate;
        public string Description;
        public string Shift;
        public string Status;
        public string OpenDateFormatted {
            get {
                if (OpenDate != null)
                {
                    return OpenDate.Value.ToString("yyyy-MM-dd HH:mm");
                }
                else
                {
                    return "";
                }

            }
        }

        public RequestViewModel()
        {
            Employee = "";
            Department = "";
            Type = "";
            OpenDate = DateTime.Today;
            Description = "";
            Shift = "";
            Status = "";
        }
    }
}