using Core.Data;
using Core.Entities;
using Core.Entities.SQL_DataType;
using Core.Entities.Utilities;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class FormatsService
    {
        private static FormatsRepository _rep;

        static FormatsService()
        {
            _rep = new FormatsRepository();
        }
        public static GenericReturn EntityFieldsConfiguration_Insert(int EntityID, int EntityIndicator, int SystemModule, int IsVisible, int IsMandatory, string ValueID,
            string Param1, string Param2, string Param3, string CatalogTagID, int DataTypeID, int FieldLength, int FieldPrecission, GenericRequest request)
        {
            return _rep.EntityFieldsConfiguration_Insert(EntityID, EntityIndicator, SystemModule, IsVisible, IsMandatory, ValueID, Param1, Param2, Param3, CatalogTagID, DataTypeID, FieldLength, FieldPrecission, request);
        }

        public static GenericReturn Upsert(int? FormatID, string FormatName, int Use2FA, int DirectApproval, int HasDetail, int HasAttachment, Guid TransactionID, string FormatPath, int PositionYInitial, int MaxDetail, int Interline, List<EntityField> FormatFieldsList, GenericRequest request)
        {
            using (DataTable dtFormatFields = FormatFieldsList.ConvertToDataTable())
            {
                // Borrar columnas que no aplican
                dtFormatFields.Columns.Remove("ID");
                dtFormatFields.Columns.Remove("ValueID");
                dtFormatFields.Columns.Remove("FieldDescription");
                dtFormatFields.Columns.Remove("DefaultTranslation");
                dtFormatFields.Columns.Remove("IsEditable");
                dtFormatFields.Columns.Remove("SystemModuleTag");
                dtFormatFields.Columns.Remove("AllowAcces");
                dtFormatFields.Columns.Remove("FieldName");
                dtFormatFields.Columns.Remove("ReadOnly");

                if (dtFormatFields != null && dtFormatFields.Rows.Count > 0)
                {
                    return _rep.Upsert(FormatID, FormatName, Use2FA, DirectApproval, HasDetail, HasAttachment, TransactionID, FormatPath, PositionYInitial, MaxDetail, Interline, dtFormatFields, request);
                }
                else
                {
                    return _rep.Upsert(FormatID, FormatName, Use2FA, DirectApproval, HasDetail, HasAttachment, TransactionID, FormatPath, PositionYInitial, MaxDetail, Interline, dtFormatFields, request);
                }
            }
            
        }
        public static GenericReturn EntityFieldUpsert(EntityField _EntityField, GenericRequest request)
        {
            // TODO: Metodo para convertir 0 a null
            if (_EntityField.ID == 0) { _EntityField.ID = null; }
            return _rep.EntityFieldUpsert(_EntityField, request);
        }

        public static GenericReturn FormatGenericDetailUpsert(int? FormatGenericDetailTempID, int FormatID, string NameES, string NameEN, string Description, int DataTypeID, int FieldLength, int FieldPrecision,
            string CatalogTag, int IsMandatory, int Enabled, Guid TransactionID, GenericRequest request)
        {
            return _rep.FormatGenericDetailUpsert(FormatGenericDetailTempID, FormatID, NameES, NameEN, Description, DataTypeID, FieldLength, FieldPrecision, CatalogTag, IsMandatory, Enabled, TransactionID, request);
        }
        public static GenericReturn FormatAccessTempInsert(int FormatID, Guid TransactionID, string FacilityID, string UserListID, GenericRequest request)
        {
            return _rep.FormatAccessTempInsert(FormatID, TransactionID, FacilityID, UserListID, request);
        }

        public static GenericReturn FormatGenericDetailTempInsert(int FormatID, Guid TransactionID, GenericRequest request)
        {
            return _rep.FormatGenericDetailTempInsert(FormatID, TransactionID, request);
        }

        public static List<FormatAccessTemp> FormatAccessTemp_List4Transaction(int FormatID, Guid TransactionID, GenericRequest request)
        {
            using (DataTable dt = _rep.FormatAccessTemp_List4Transaction(FormatID, TransactionID, request))
            {
                List<FormatAccessTemp> _list = dt.ConvertToList<FormatAccessTemp>();
                return _list;
            }
        }
        public static List<FormatAccessTemp> GetFormatAccessTemp_List(Guid TransactionID, GenericRequest request)
        {
            using (DataTable dt = _rep.GetFormatAccessTemp_List(TransactionID, request))
            {
                List<FormatAccessTemp> _list = dt.ConvertToList<FormatAccessTemp>();
                return _list;
            }
        }
        public static List<FormatsLoopsRulesTemp> FormatsLoopsRulesTemp_List(Guid TransactionID, int FacilitySelectID, GenericRequest request)
        {
            using (DataTable dt = _rep.FormatsLoopsRulesTemp_List(TransactionID, FacilitySelectID, request))
            {
                List<FormatsLoopsRulesTemp> _list = dt.ConvertToList<FormatsLoopsRulesTemp>();
                return _list;
            }
        }        
        public static List<PDFFilesDetail_TEMP> PDFFilesDetail_TEMP_List4Detail (Guid TransactionID, int FileID, GenericRequest request)
        {
            using (DataTable dt = _rep.PDFFilesDetail_TEMP_List4Detail(TransactionID, FileID, request))
            {
                List<PDFFilesDetail_TEMP> _list = dt.ConvertToList<PDFFilesDetail_TEMP>();
                return _list;
            }
        }        
        public static List<PDFFilesDetail_TEMP> PDFFilesDetail_TEMP_List4Signatures(Guid TransactionID, int FileID, GenericRequest request)
        {
            using (DataTable dt = _rep.PDFFilesDetail_TEMP_List4Signatures(TransactionID, FileID, request))
            {
                List<PDFFilesDetail_TEMP> _list = dt.ConvertToList<PDFFilesDetail_TEMP>();
                return _list;
            }
        }
        public static List<PDFFilesDetail_TEMP> PDFFilesDetail_TEMP_List4Header (Guid TransactionID, int FileID, GenericRequest request)
        {
            using (DataTable dt = _rep.PDFFilesDetail_TEMP_List4Header(TransactionID, FileID, request))
            {
                List<PDFFilesDetail_TEMP> _list = dt.ConvertToList<PDFFilesDetail_TEMP>();
                return _list;
            }
        }
        public static List<User> UsersFacilitiesPermission(string FacilitiesList, GenericRequest request)
        {
            using (DataTable dt = _rep.UsersFacilitiesPermission(FacilitiesList, request))
            {
                List<User> _list = dt.ConvertToList<User>();
                return _list;
            }
        }        
        public static List<FormatsEntity> FormatsList(string FormatName, string Approver, GenericRequest request)
        {
            using (DataTable dt = _rep.FormatsList(FormatName, Approver, request))
            {
                List<FormatsEntity> _list = dt.ConvertToList<FormatsEntity>();
                return _list;
            }
        }     
        public static List<FormatGenericDetailTemp> FormatGenericDetailTemp_List4PDFConfiguration(Guid TransactionID, GenericRequest request)
        {
            using (DataTable dt = _rep.FormatGenericDetailTemp_List4PDFConfiguration(TransactionID, request))
            {
                List<FormatGenericDetailTemp> _list = dt.ConvertToList<FormatGenericDetailTemp>();
                return _list;
            }
        }        
        public static List<CatalogDetailTemp> CatalogsDetailTemp_List4ConfigurationPDF(Guid TransactionID, GenericRequest request)
        {
            using (DataTable dt = _rep.CatalogsDetailTemp_List4ConfigurationPDF(TransactionID, request))
            {
                List<CatalogDetailTemp> _list = dt.ConvertToList<CatalogDetailTemp>();
                return _list;
            }
        }

        public static GenericReturn AddNewFlowPhase(CatalogDetail _CatalogDetail, Guid Transaction, GenericRequest request)
        {
            return _rep.AddNewFlowPhase(_CatalogDetail, Transaction, request);
        }
        public static GenericReturn CreateNewFormat(string FormatName, int Use2FA, int DirectApproval, int HasDetail, int HasAttachment, int? FileID, int Enabled, GenericRequest request)
        {
            return _rep.CreateNewFormat(FormatName, Use2FA, DirectApproval, HasDetail, HasAttachment, FileID, Enabled, request);
        }
        public static GenericReturn FormatAccess_TEMP_Delete(int FormatAccessTempID, GenericRequest request)
        {
            return _rep.FormatAccess_TEMP_Delete(FormatAccessTempID, request);
        }
        public static GenericReturn FormatsLoopsRulesDetail_TEMP_Delete4Edit(int FormatLoopRuleTempID, int Seq, Guid TransactionID, GenericRequest request)
        {
            return _rep.FormatsLoopsRulesDetail_TEMP_Delete4Edit(FormatLoopRuleTempID, Seq, TransactionID, request);
        }
        public static GenericReturn FormatPhase_CatalogDetail_TEMP_Delete(int CatalogDetailTempID, GenericRequest request)
        {
            return _rep.FormatPhase_CatalogDetail_TEMP_Delete(CatalogDetailTempID, request);
        }
        public static GenericReturn FormatsLoopsApprovers_TEMP_Delete(int FormatLoopApproverTempID, GenericRequest request)
        {
            return _rep.FormatsLoopsApprovers_TEMP_Delete(FormatLoopApproverTempID, request);
        }
        public static GenericReturn FormatsLoopsRules_TEMP_Delete(int FormatLoopRuleTempID, GenericRequest request)
        {
            return _rep.FormatsLoopsRules_TEMP_Delete(FormatLoopRuleTempID, request);
        }
        public static GenericReturn TranslationDetaulUpdate(string Tag, string DescriptionEN, string DescriptionES, GenericRequest request)
        {
            return _rep.TranslationDetaulUpdate(Tag, DescriptionEN, DescriptionES, request);
        }

        public static List<TranslationDetail> GetTranslationList(int CatalogDetailTempID, int CatalogDetailID, GenericRequest request)
        {
            using (DataTable dt = _rep.GetTranslationList(CatalogDetailTempID, CatalogDetailID, request.FacilityID, request.UserID, request.CultureID))
            {
                List<TranslationDetail> _list = dt.ConvertToList<TranslationDetail>();

                return _list;
            }
        }
        public static List<FormatsLoopsRulesTemp> FormatsLoopsRules_TEMP_List(int FormatLoopRuleTempID, GenericRequest request)
        {
            using (DataTable dt = _rep.FormatsLoopsRules_TEMP_List(FormatLoopRuleTempID, request))
            {
                List<FormatsLoopsRulesTemp> _list = dt.ConvertToList<FormatsLoopsRulesTemp>();

                return _list;
            }
        }
        public static GenericReturn FormatGenericDetailTempDelete(int FormatID, string ColumnName, Guid TransactionID, GenericRequest request)
        {
            return _rep.FormatGenericDetailTempDelete(FormatID, ColumnName, TransactionID, request);
        }
        public static GenericReturn FormatsLoopsFlow_TEMP_Delete(int FormatLoopFlowTempID, GenericRequest request)
        {
            return _rep.FormatsLoopsFlow_TEMP_Delete(FormatLoopFlowTempID, request);
        }

        public static List<FormatGenericDetailTemp> FormatGenericDetailList(int FormatID, Guid TransactionID, GenericRequest request)
        {
            using (DataTable dt = _rep.FormatGenericDetailList(FormatID, TransactionID, request))
            {
                List<FormatGenericDetailTemp> _list = dt.ConvertToList<FormatGenericDetailTemp>();

                return _list;
            }
        }
        public static List<EntityField> FormatsListGeneralFields(int FormatID, GenericRequest request)
        {
            using (DataTable dt = _rep.FormatsListGeneralFields(FormatID, request))
            {
                List<EntityField> _list = dt.ConvertToList<EntityField>();

                return _list;
            }
        }        
        public static List<FormatGenericDetailTemp> FormatGenericDetailField_List(int FormatID, GenericRequest request)
        {
            using (DataTable dt = _rep.FormatGenericDetailField_List(FormatID, request))
            {
                List<FormatGenericDetailTemp> _list = dt.ConvertToList<FormatGenericDetailTemp>();

                return _list;
            }
        } 
        public static List<CatalogDetailTemp> Formats_ListPhases(int FormatID, GenericRequest request)
        {
            using (DataTable dt = _rep.Formats_ListPhases(FormatID, request))
            {
                List<CatalogDetailTemp> _list = dt.ConvertToList<CatalogDetailTemp>();

                return _list;
            }
        }
        public static List<FormatAccessTemp> FormatAccess_List(int FormatID, GenericRequest request)
        {
            using (DataTable dt = _rep.FormatAccess_List(FormatID, request))
            {
                List<FormatAccessTemp> _list = dt.ConvertToList<FormatAccessTemp>();

                return _list;
            }
        }
        public static FormatGenericDetailTemp FormatGenericDetailList4FormatGenericDetail(int FormatGenericDetailTempID, GenericRequest request)
        {
            using (DataTable dt = _rep.FormatGenericDetailList4FormatGenericDetail(FormatGenericDetailTempID, request))
            {
                FormatGenericDetailTemp _list = dt.ConvertToList<FormatGenericDetailTemp>().FirstOrDefault();

                return _list;
            }
        }

        public static DataTable FormatsPreviewFileData(int FormatID, int FileID, Guid TransactionID, int PositionYInitial, int MaxDetail, int Interline, GenericRequest request)
        {
            return _rep.FormatsPreviewFileData(FormatID, FileID, TransactionID, PositionYInitial, MaxDetail, Interline, request);
        }
        public static List<FormatsLoopsFlow_TEMP> FormatsLoopsFlow_TEMP_List(int FormatLoopRuleTempID, GenericRequest request)
        {
            using (DataTable dt = _rep.FormatsLoopsFlow_TEMP_List(FormatLoopRuleTempID, request))
            {
                List<FormatsLoopsFlow_TEMP> _list = dt.ConvertToList<FormatsLoopsFlow_TEMP>();

                return _list;
            }
        }
        public static List<FormatsLoopsRulesDetail_TEMP> FormatsLoopsRulesDetail_TEMP_List4Edit(int FormatLoopRuleTempID, Guid TransactionID, int FormatID, GenericRequest request)
        {
            using (DataTable dt = _rep.FormatsLoopsRulesDetail_TEMP_List4Edit(FormatLoopRuleTempID, TransactionID, FormatID, request))
            {
                List<FormatsLoopsRulesDetail_TEMP> _list = dt.ConvertToList<FormatsLoopsRulesDetail_TEMP>();

                return _list;
            }
        }
        public static List<FormatsLoopsRulesDetail_TEMP> FormatsLoopsRulesDetail_TEMP_List4EditExceptions(int FormatLoopRuleTempID, Guid TransactionID, int FormatID, Decimal Seq, GenericRequest request)
        {
            using (DataTable dt = _rep.FormatsLoopsRulesDetail_TEMP_List4EditExceptions(FormatLoopRuleTempID, TransactionID, FormatID, Seq, request))
            {
                List<FormatsLoopsRulesDetail_TEMP> _list = dt.ConvertToList<FormatsLoopsRulesDetail_TEMP>();

                return _list;
            }
        }
        public static List<FormatsLoopsApprovers_TEMP> FormatsLoopsAprovers_TEMP_List(int FormatLoopFlowTempID, GenericRequest request)
        {
            using (DataTable dt = _rep.FormatsLoopsAprovers_TEMP_List(FormatLoopFlowTempID, request))
            {
                List<FormatsLoopsApprovers_TEMP> _list = dt.ConvertToList<FormatsLoopsApprovers_TEMP>();

                return _list;
            }
        }
        public static FormatsLoopsApprovers_TEMP FormatsLoopsAprovers_TEMP_List4ID(int FormatLoopApproverTempID, GenericRequest request)
        {
            using (DataTable dt = _rep.FormatsLoopsAprovers_TEMP_List4ID(FormatLoopApproverTempID, request))
            {
                FormatsLoopsApprovers_TEMP _list = dt.ConvertToList<FormatsLoopsApprovers_TEMP>().FirstOrDefault();
                return _list;
            }
        }

        public static GenericReturn CreateRule(List<t_FormatsLoopsFlow> FormatsLoopsFlow, int FacilitySelectedID, Guid TransactionID, int FormatID, string Description, GenericRequest request)
        {
            if (FormatsLoopsFlow != null)
            {
                using (DataTable dtFormatsLoopsFlow = FormatsLoopsFlow.ConvertToDataTable())
                {
                    if (dtFormatsLoopsFlow != null && dtFormatsLoopsFlow.Rows.Count > 0)
                    {
                        return _rep.CreateRule(dtFormatsLoopsFlow, FacilitySelectedID, TransactionID, FormatID, Description, request);
                    }
                    else
                    {
                        return _rep.CreateRule(null, FacilitySelectedID, TransactionID, FormatID, Description, request);
                    }
                }
            }
            else
            {
                return _rep.CreateRule(null, FacilitySelectedID, TransactionID, FormatID, Description, request);
            }

        }
        public static GenericReturn EditRules(int FormatLoopRuleTempID, List<t_FormatsLoopsFlow> FormatsLoopsFlow, int FacilitySelectedID, Guid TransactionID, int FormatID, string Description, GenericRequest request)
        {
            if (FormatsLoopsFlow != null)
            {
                using (DataTable dtFormatsLoopsFlow = FormatsLoopsFlow.ConvertToDataTable())
                {
                    if (dtFormatsLoopsFlow != null && dtFormatsLoopsFlow.Rows.Count > 0)
                    {
                        return _rep.EditRules(FormatLoopRuleTempID, dtFormatsLoopsFlow, FacilitySelectedID, TransactionID, FormatID, Description, request);
                    }
                    else
                    {
                        return _rep.EditRules(FormatLoopRuleTempID, null, FacilitySelectedID, TransactionID, FormatID, Description, request);
                    }
                }
            }
            else
            {
                return _rep.EditRules(FormatLoopRuleTempID, null, FacilitySelectedID, TransactionID, FormatID, Description, request);
            }

        }

        public static GenericReturn AddFormatsLoopsApprovers(int FormatLoopFlowTempID, Guid TransactionID, int DepartmentOriginID, int JobPositionID, int DepartmentID, string ApproverIDs, int AddTolerance, int Tolerance, int ToleranceUoM, GenericRequest request)
        {
            return _rep.AddFormatsLoopsApprovers(FormatLoopFlowTempID, TransactionID, DepartmentOriginID, JobPositionID, DepartmentID, ApproverIDs, AddTolerance, Tolerance, ToleranceUoM, request);
        }   
        public static GenericReturn PDFFilesDetail_TEMP_Add(Guid TransactionID, int FileID, string FieldNames, string FieldType, GenericRequest request)
        {
            return _rep.PDFFilesDetail_TEMP_Add(TransactionID, FileID, FieldNames, FieldType, request);
        }
        public static void Formats_GenericFieldData(int CatalogDetailID, int FacilityID, out string DataTypeValue, out int IsAdditionalField, out string CatalogTag, out int TableAdditionalFieldID)
        {
            DataTypeValue = "";
            IsAdditionalField = 0;
            CatalogTag = "";
            TableAdditionalFieldID = 0;
            _rep.Formats_GenericFieldData(CatalogDetailID, FacilityID, out DataTypeValue, out IsAdditionalField, out CatalogTag, out TableAdditionalFieldID);
        }
        public static void Formats_GetFieldData4ValueID(string ValueID, int FacilityID, string CultureID, out string DataTypeValue, out int IsAdditionalField, out string CatalogTag, out int TableAdditionalFieldID)
        {
            DataTypeValue = "";
            IsAdditionalField = 0;
            CatalogTag = "";
            TableAdditionalFieldID = 0;
            _rep.Formats_GetFieldData4ValueID(ValueID, FacilityID, CultureID, out DataTypeValue, out IsAdditionalField, out CatalogTag, out TableAdditionalFieldID);
        }
        public static GenericReturn FormatsLoopsRulesDetail_TEMP_Create(Guid TransactionID, int FormatLoopRuleTempID, int FieldID, int IsAdditionalField, string DatePartArgument, int ComparisonOperator, string RuleDetailType, decimal? Seq, string ValuesArray, int FacilityID, GenericRequest request)
        {
            return _rep.FormatsLoopsRulesDetail_TEMP_Create(TransactionID, FormatLoopRuleTempID, FieldID, IsAdditionalField, DatePartArgument, ComparisonOperator, RuleDetailType, Seq, ValuesArray, FacilityID, request);
        }  
        public static GenericReturn FormatsLoopsRulesDetail_TEMP_Delete(int FormatLoopRuleDetailTempID, GenericRequest request) 
        { 
            return _rep.FormatsLoopsRulesDetail_TEMP_Delete(FormatLoopRuleDetailTempID, request);
        }

        public static List<FormatsLoopsRulesDetail_TEMP> FormatsLoopsRulesDetail_TEMP_List4EditRules(int FormatID, int FormatLoopRuleTempID, Guid TransactionID, decimal Seq, int FacilityID, GenericRequest request)
        {
            using (DataTable dt = _rep.FormatsLoopsRulesDetail_TEMP_List4EditRules(FormatID, FormatLoopRuleTempID, TransactionID, Seq, FacilityID, request))
            {
                List<FormatsLoopsRulesDetail_TEMP> _list = dt.ConvertToList<FormatsLoopsRulesDetail_TEMP>();

                return _list;
            }
        }   
        public static List<FormatsLoopsRulesDetail_TEMP> FormatsLoopsRulesDetail_TEMP_List4EditRulesExceptions(int FormatID, int FormatLoopRuleTempID, Guid TransactionID, decimal Seq, int FacilityID, GenericRequest request)
        {
            using (DataTable dt = _rep.FormatsLoopsRulesDetail_TEMP_List4EditRulesExceptions(FormatID, FormatLoopRuleTempID, TransactionID, Seq, FacilityID, request))
            {
                List<FormatsLoopsRulesDetail_TEMP> _list = dt.ConvertToList<FormatsLoopsRulesDetail_TEMP>();

                return _list;
            }
        }
    }
}
