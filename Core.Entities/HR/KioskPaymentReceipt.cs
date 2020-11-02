using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class KioskPaymentReceipt
    {
        public string Clave { get; set; }
        public string Recibo { get; set; }
        public string Receptor { get; set; }
        public string FolioSobre { get; set; }
        public int aFiscal { get; set; }
        public int sFiscal { get; set; }
        public byte[] ByteXML { get; set; }
        public byte[] BytePDF { get; set; }
        public string FilePDF
        {
            get
            {
                //return "/Files/HR/Kiosk/Receipts/{0}/" + Recibo.Replace(" | ", " ") + ".pdf";
                return "~/Files/Temp/HR/Kiosk/Receipts/{0}/" + FolioSobre + ".pdf";
            }
        }
        public string FileXML
        {
            get
            {
                //return "/Files/HR/Kiosk/Receipts/{0}/" + Recibo.Replace(" | ", " ") + ".xml";
                return "~/Files/Temp/HR/Kiosk/Receipts/{0}/" + FolioSobre + ".xml";
            }
        }
    }
}
