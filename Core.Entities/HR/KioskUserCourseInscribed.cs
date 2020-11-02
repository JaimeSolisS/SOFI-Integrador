using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class KioskUserCourseInscribed
    {
        public int idRegistro { get; set; }
        public int idCurso { get; set; }
        public string claveEmp { get; set; }
        public int division { get; set; }
    }
}