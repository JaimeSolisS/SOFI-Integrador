using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class PDFFileService
    {
        private static PDFFileRepository _rep;

        static PDFFileService()
        {
            _rep = new PDFFileRepository();
        }

        public static PDFFile PDFFiles_GetPDFDetail(int FormatID, GenericRequest request)
        {
            using (DataTable dt = _rep.PDFFiles_GetPDFDetail(FormatID, request))
            {
                PDFFile _list = dt.ConvertToList<PDFFile>().FirstOrDefault();
                return _list;
            }
        }
        public static GenericReturn PDFFilesDetail_TEMP_Delete(int FileDetailTempID, GenericRequest request)
        {
            return _rep.PDFFilesDetail_TEMP_Delete(FileDetailTempID, request);
        }
        public static GenericReturn PDFFilesDetail_TEMP_Insert(Guid TransactionID, int FileID, string FieldName, string FieldType, GenericRequest request)
        {
            return _rep.PDFFilesDetail_TEMP_Insert(TransactionID, FileID, FieldName, FieldType, request);
        }
        public static GenericReturn PDFFilesDetail_TEMP_QuickUpdate(int FileDetailTempID, string ColumnName, string Value, GenericRequest request)
        {
            return _rep.PDFFilesDetail_TEMP_QuickUpdate(FileDetailTempID, ColumnName, Value, request);
        }
    }
}
