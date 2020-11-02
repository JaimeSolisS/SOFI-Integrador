using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class FormatsMedia : TableMaintenance
    {
        public int FormatMediaID { get; set; }
        public string Path { get; set; }
        public string FileName { get { return System.IO.Path.GetFileName(Path); } set { } }
        public int? Seq { get; set; }
        public string MediaID { get; set; }
        public int Fileid { get; set; }
        public bool ViewReadOnly { get; set; }
    }
}
