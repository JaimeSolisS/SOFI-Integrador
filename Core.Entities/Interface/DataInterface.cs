using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class DataInterface : TableMaintenance
    {
        public int DataInterfaceID { get; set; }
        public string InterfaceName { get; set; }
        public string DataInterfaceValue { get; set; }
        public int StartRow { get; set; }
        public string Path { get; set; }
        public string RegexName { get; set; }
        public int FrequencyID { get; set; }
        public int FileTypeID { get; set; }
        public string ValidFileExtensions { get; set; }
        public string SQLCommand { get; set; }
        public int Seq { get; set; }
        public bool Enabled { get; set; }
        public DateTime CurrentFullDate { get; set; }
        public DateTime NextRun { get; set; }
        public List<DataInterfacesField> InterfacesFields { get; set; }
        public string Reference { get; set; } 
        public bool DeleteFilesOnFinish { get; set; }
    }
}
