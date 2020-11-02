using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class KioskUserPointsMovement
    {
        public string Clave { get; set; }
        public string Concepto { get; set; }
        public int Puntos { get; set; }
        public DateTime Fecha { get; set; }
        public int Tipo { get; set; }
    }
}
