using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Core.Services
{
    public static class AttachmentService
    {
        private static AttachmentRepository _rep;
        static AttachmentService()
        {
            _rep = new AttachmentRepository();
        }

        #region Methods

        public static List<IAttachment> List(int? FileID, string AttachmentType, int? ReferenceID, int? ReferenceType, int? CompanyID, int? FileType, string FilePathName, GenericRequest req)
        {
            DataTable dt = new DataTable();
            IEnumerable<IAttachment> EntitiesList = Enumerable.Empty<IAttachment>();
            switch (AttachmentType.ToUpper())
            {
                case "PARTS":
                    {
                        dt = _rep.PartsMasterAttachments_List(FileID, ReferenceID, CompanyID, FileType, FilePathName, req.UserID, req.CultureID);
                        EntitiesList = dt.ConvertToList<PartsMasterAttachment>();
                        break;
                    }
                default:
                    {
                        dt = _rep.Attachments_List(FileID, ReferenceID, ReferenceType, FileType, FilePathName, req.UserID, req.FacilityID, req.CultureID);
                        EntitiesList = dt.ConvertToList<WMS_Attachment>();
                        break;
                    }
            }

            return EntitiesList.ToList();
        }

        public static GenericReturn Insert(int? ReferenceID, string AttachmentType, int? ReferenceType, int? CompanyID, int? FileType, string FilePathName, GenericRequest req)
        {
            GenericReturn result = new GenericReturn();
            switch (AttachmentType.ToUpper())
            {
                case "PARTS":
                    {
                        result = _rep.PartsMasterAttachments_Insert(ReferenceID, CompanyID, FileType, FilePathName, req.UserID, req.CultureID);
                        break;
                    }
                default:
                    {
                        result = _rep.Attachments_Insert(ReferenceID, ReferenceType, FileType, FilePathName, req.UserID, req.CultureID);
                        break;
                    }
            }
            return result;

        }

        public static GenericReturn QuickUpdate(int? FileID, string AttachmentType, int? ReferenceID, int? ReferenceType, int? CompanyID, int? FileType, string FilePathName, GenericRequest req)
        {
            GenericReturn result = new GenericReturn();
            switch (AttachmentType.ToUpper())
            {
                case "PARTS":
                    {
                        result = _rep.PartsMasterAttachments_QuickUpdate(FileID, ReferenceID, CompanyID, FileType, FilePathName, req.UserID, req.CultureID);
                        break;
                    }
                default:
                    {
                        result = _rep.Attachments_QuickUpdate(FileID, ReferenceID, ReferenceType, FileType, FilePathName, req.UserID, req.CultureID);
                        break;
                    }
            }
            return result;

        }

        public static GenericReturn Delete(int? FileID, string AttachmentType, GenericRequest req)
        {
            GenericReturn result = new GenericReturn();
            switch (AttachmentType.ToUpper())
            {
                case "PARTS":
                    {
                        result = _rep.PartsMasterAttachments_Delete(FileID, req.UserID, req.CultureID);
                        break;
                    }
                default:
                    {
                        result = _rep.Attachments_Delete(FileID, req.UserID, req.CultureID);
                        break;
                    }
            }
            return result;
        }

        public static GenericReturn DA_InsertPOD(int OrderID, string FilePath, GenericRequest request)
        {
            return _rep.DA_InsertPOD(OrderID, FilePath, request);
        }

        public static GenericReturn Properties_QuickUpdate(int? FileId, string PropertyName, string PropertyValue, int? PropertyTypeID, GenericRequest request)
        {
            return _rep.Properties_QuickUpdate(FileId, PropertyName, PropertyValue, PropertyTypeID, request);
        }

        #endregion
    }
}
