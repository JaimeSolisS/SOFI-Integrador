using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class KioskUserPendingPointsExchange
    {
        public int ID { get; set; }
        public int Clave { get; set; }
        public string RFC { get; set; }
        public int Puntos { get; set; }
        public string Solicitud { get; set; }
    }
}
