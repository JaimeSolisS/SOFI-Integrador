using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models.ViewModels
{
    public class AdditionalFieldsViewModel
    {
        public List<TableAdditionalFields> CollectionAdditionalFields { get; set; }
        public bool ViewReadOnly { get; set; }
        public int ReferenceID { get; set; }
        public string disabled { get{ if (ViewReadOnly) { return "disabled"; } return string.Empty; }}
    }
}