using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebSockets;

namespace WebSite.Areas.eRequest.Models.ViewModels.Formats
{
    public class CreateViewModel
    {
        public FormatsEntity EntityFormat { get; set; }
        public int FormatID { get; set; }
        public List<EntityField> _CustomFieldsList;
        public List<FormatGenericDetailTemp> _FormatGenericDetail;
        public List<CatalogDetailTemp> _CatalogPhase;
        public List<FormatAccessTemp> _FormatAccessTemp;
        public List<User> _UserList;
        public List<FormatsLoopsRulesTemp> _FormatsLoopsRulesTemp;
        public IEnumerable<SelectListItem> FacilityList;
        public PDFFile PDFFile { get; set; }
        public Guid TransactionID { get; set; }
        public string PathToShow { get; set; }

        public List<PDFFilesDetail_TEMP> _PDFAdditionalFields { get; set; }
        public List<PDFFilesDetail_TEMP> _PDFDetailFields { get; set; }
        public List<PDFFilesDetail_TEMP> _PDFConfigurationSignature { get; set; }
        public IEnumerable<SelectListItem> _FontList { get; set; }
        public List<Catalog> _FontListFE { get; set; }
        public List<Catalog> _FontColorList { get; set; }
        public List<Catalog> _ValueToShow { get; set; }
        public List<Catalog> _ShowWhen { get; set; }
        
        public CreateViewModel()
        {
            EntityFormat = new FormatsEntity();
            FacilityList = new SelectList(new List<Facility>());
            _CustomFieldsList = new List<EntityField>();
            _FormatGenericDetail = new List<FormatGenericDetailTemp>();
            _CatalogPhase = new List<CatalogDetailTemp>();
            _FormatAccessTemp = new List<FormatAccessTemp>();
            _UserList = new List<User>();
            _FormatsLoopsRulesTemp = new List<FormatsLoopsRulesTemp>();
            FormatID = 0;
            PDFFile = new PDFFile();
            PathToShow = "";
            _PDFAdditionalFields = new List<PDFFilesDetail_TEMP>();
            _PDFDetailFields = new List<PDFFilesDetail_TEMP>();
            _FontList = new SelectList(new List<Catalog>());
            _FontColorList = new List<Catalog>();
            _FontListFE = new List<Catalog>();
            _PDFConfigurationSignature = new List<PDFFilesDetail_TEMP>();
        }
    }
   
}