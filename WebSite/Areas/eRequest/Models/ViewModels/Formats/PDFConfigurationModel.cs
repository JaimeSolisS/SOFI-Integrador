using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.eRequest.Models.ViewModels.Formats
{
    public class PDFConfigurationModel
    {
        public List<PDFFilesDetail_TEMP> _PDFAdditionalFields { get; set; }
        public IEnumerable<SelectListItem> _FontList { get; set; }
        public IEnumerable<SelectListItem> _FontColorList { get; set; }
        public PDFConfigurationModel()
        {
            _PDFAdditionalFields = new List<PDFFilesDetail_TEMP>();
            _FontList = new SelectList(new List<Catalog>());
            _FontColorList = new SelectList(new List<Catalog>());
        }
    }
}