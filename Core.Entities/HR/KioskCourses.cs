using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class KioskCourses
    {
        public int ID { get; set; }
        public int? Clave { get; set; }
        public string Nombre { get; set; }
        public DateTime IniciaCurso { get; set; }
        public DateTime TerminaCurso { get; set; }
        public DateTime IniciaRegistro { get; set; }
        public DateTime TerminaRegistro { get; set; }
        public int? Grado { get; set; }
        public string Estado { get; set; }
        public bool? InscripcionAbierta { get; set; }

    }
}
