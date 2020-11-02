﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.eRequest.Models.ViewModels.Formats
{
    public class ModalAddDetailFieldViewModal
    {
        public IEnumerable<SelectListItem> CatalogsList { get; set; }
        public IEnumerable<SelectListItem> ParameterList { get; set; }
        public IEnumerable<SelectListItem> ReferenceList { get; set; }
        public IEnumerable<SelectListItem> ReferenceTypesList { get; set; }
        public FormatGenericDetailTemp FormatDetail { get; set; }
        public string ParameterName { get; set; }
        public int? ParameterType { get; set; }
        public decimal? ParameterLength { get; set; }
        public decimal? NumberOfDecimals { get; set; }
        public bool? UserReference { get; set; }
        public bool? IsCavity { get; set; }
        public bool? Enabled { get; set; }
        public string ReferenceName { get; set; }
        public string Type { get; set; }
        public int? SelectedOption { get; set; }
        public string Title { get; set; }
        public string EnglishName { get; set; }
        public string SpanishName { get; set; }
        public string ParameterValueID { get; set; }

        public ModalAddDetailFieldViewModal()
        {
            CatalogsList = new SelectList(new List<SelectListItem>());
            ParameterList = new SelectList(new List<SelectListItem>());
            ReferenceList = new SelectList(new List<SelectListItem>());
            ReferenceTypesList = new SelectList(new List<SelectListItem>());
            FormatDetail = new FormatGenericDetailTemp();
            ParameterName = "";
            ParameterType = 0;
            ParameterLength = 0;
            NumberOfDecimals = 0;
            UserReference = false;
            IsCavity = false;
            Enabled = true;
            ReferenceName = "";
            Type = "";
            SelectedOption = 0;
            Title = "";
            EnglishName = "";
            SpanishName = "";
            ParameterValueID = "";
        }
    }
}