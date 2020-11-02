using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.CI.Models.ViewModels.Administration
{
    public class DashboardAreaDetailUpsertViewModel
    {
        public List<Catalog> FileTypeList;
        public IEnumerable<SelectListItem> DashboardSizeList;
        public IEnumerable<SelectListItem> DashboardBackColorList;
        public IEnumerable<SelectListItem> DashboardFontColorList;
        public IEnumerable<SelectListItem> DashboardIconList;
        public IEnumerable<SelectListItem> DashboardDataEffectList;
        public List<Catalog> DashboardNameTranslate;
        public DashboardAreaDetail DashboardAreaDetailInfo;
        public string FileTypeLinkCss;
        public string FileTypeMediaOrFileCss;
        public string FileTypeGaleryCss;
        public string TransactionID;
        public string FileTypeDisabled;
        public string SourcePathHidden;
        public string HeaderModal;
        public string ButtonAcceptModal;
        public IEnumerable<SelectListItem> ParentsNodesList;
        public IEnumerable<SelectListItem> SectionsList;


        public DashboardAreaDetailUpsertViewModel()
        {
            FileTypeList = new List<Catalog>();
            DashboardSizeList = new SelectList(new List<Catalog>());
            DashboardBackColorList = new SelectList(new List<Catalog>());
            DashboardFontColorList = new SelectList(new List<Catalog>());
            DashboardIconList = new SelectList(new List<Catalog>());
            DashboardDataEffectList = new SelectList(new List<Catalog>());
            DashboardNameTranslate = new List<Catalog>();
            DashboardAreaDetailInfo = new DashboardAreaDetail();
            FileTypeGaleryCss = "hidden";
            FileTypeMediaOrFileCss = "hidden";
            FileTypeLinkCss = "hidden";
            FileTypeDisabled = "disabled";
            SourcePathHidden = "hidden";
            TransactionID = Guid.NewGuid().ToString();
            HeaderModal = "";
            ButtonAcceptModal = "";
            ParentsNodesList = new SelectList(new List<SelectListItem>());
            SectionsList = new SelectList(new List<SelectListItem>());

        }
    }
}