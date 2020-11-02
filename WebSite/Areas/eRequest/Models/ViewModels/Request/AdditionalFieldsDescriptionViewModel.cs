using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.eRequest.Models.ViewModels.Request
{
    public class AdditionalFieldsDescriptionViewModel
    {
        public List<TableAdditionalFields> CollectionAdditionalFields { get; set; }
        public List<RequestGenericDetail_tb> CollectionAdditionalFieldsTable { get; set; }
        public bool ViewReadOnly { get; set; }
        public int ReferenceID { get; set; }
        public string disabled { get{ if (ViewReadOnly) { return "disabled"; } return string.Empty; }}
    }
}