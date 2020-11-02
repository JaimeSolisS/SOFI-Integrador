using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.QA
{
    public class FilmetricInspectionDetail 
    {
        public int? FilmetricInspectionDetailID { get; set; }
        public int? FilmetricInspectionID { get; set; }
        public int? FilmID { get; set; }
        public string FilmName { get; set; }
        public decimal? Value { get; set; }
        public DateTime DateAdded { get; set; }

        }
}
