using Core.Entities.QA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.QA
{
    public class  FilmetricFilmLimits
    {
        public int FilmID { get; set; }
        public string FilmName { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal MaxValue { get; set; }
        public decimal MinValue { get; set; }

        
    }
}
