using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class DownTime : TableMaintenance
    {
        public int DownTimeID { get; set; }
        public int ReferenceID { get; set; }
        public int ReferenceTypeID { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDateFormat {
            get { return StartDate.ToString("HH:mm"); }
        }
        public DateTime EndDate { get; set; }
        public string EndDateFormat {
            get { return EndDate.ToString("HH:mm"); }
        }
        public string TimeFormat { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int ReasonID { get; set; }
        public string ReasonName { get; set; }
        public string Comments { get; set; }
        public int DownTimeTypeID { get; set; }
        public int StatusID { get; set; }
        public string StatusValue { get; set; }
        public int InsertsQuantity { get; set; }
    }
}
