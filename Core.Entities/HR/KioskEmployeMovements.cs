using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class KioskEmployeMovements : TableMaintenance
    {
        public int SuggestionsCount { get; set; }
        public int RequestsCount { get; set; }
        public int VacanciesAvailable { get; set; }
        public int PaymentReceipts { get; set; }
        public int CoursesAvailable { get; set; }
    }
}
