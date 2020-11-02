using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class PDFFilesDetail
    {
        public int NumDocto { get; set; }
        public string FieldValue { get; set; }
        public string FieldName { get; set; }
        public string FontName { get; set; }
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public float FontSize { get; set; }
        public string FontColor { get; set; }
    }
}
