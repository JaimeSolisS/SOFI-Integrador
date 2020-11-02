using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class KioskPrePayroll : TableMaintenance
    {
        public DateTime Fecha { get; set; }
        public string Ent1 { get; set; }
        public string Sal1 { get; set; }
        public string Ent2 { get; set; }
        public string Sal2 { get; set; }
        public string Ent3 { get; set; }
        public string Sal3 { get; set; }
        public string Ent4 { get; set; }
        public string Sal4 { get; set; }
        public string SalMax
        {
            get
            {
                if (Sal4 != null && Sal4 != "") return Sal4;
                else if (Sal3 != null && Sal3 != "") return Sal3;
                else if (Sal2 != null && Sal2 != "") return Sal2;
                else return Sal1;
            }
        }
        public string Observaciones { get; set; }
        public Decimal? HorasLaborales { get; set; }
        public Decimal? HorasExtras { get; set; }
    }
}
