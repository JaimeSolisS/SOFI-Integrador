using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class TableMaintenance
    {
        public int UserID { get; set; }
        public string UserFullName { get; set; }
        public DateTime DateLastMaint { get; set; }
        public DateTime DateAdded { get; set; }

        public string DateLastMaintFormat
        {
            get { return DateLastMaint.ToString("yyyy-MM-dd HH:mm"); }
        }

        public string DateAddedFormat
        {
            get { return DateAdded.ToString("yyyy-MM-dd HH:mm"); }
        }
    }
}
