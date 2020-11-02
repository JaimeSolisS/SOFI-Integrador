using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebSite.Areas.HR.Models.ViewModels.KioskAdministration
{
    public class KioskAreaDetailUpsertViewModel
    {
        public List<Catalog> FileTypeList;
        public IEnumerable<SelectListItem> KioskSizeList;
        public IEnumerable<SelectListItem> KioskBackColorList;
        public IEnumerable<SelectListItem> KioskFontColorList;
        public IEnumerable<SelectListItem> KioskIconList;
        public IEnumerable<SelectListItem> KioskDataEffectList;
        public List<Catalog> KioskNameTranslate;
        public KioskAreaDetail KioskAreaDetailInfo;
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

        public string BackgroundColor;
        public string FontColor;
        public bool IsRoot;
        public string SizeClass;

        public KioskAreaDetailUpsertViewModel()
        {
            FileTypeList = new List<Catalog>();
            KioskSizeList = new SelectList(new List<Catalog>());
            KioskBackColorList = new SelectList(new List<Catalog>());
            KioskFontColorList = new SelectList(new List<Catalog>());
            KioskIconList = new SelectList(new List<Catalog>());
            KioskDataEffectList = new SelectList(new List<Catalog>());
            KioskNameTranslate = new List<Catalog>();
            KioskAreaDetailInfo = new KioskAreaDetail();
            FileTypeGaleryCss = "hidden";
            FileTypeMediaOrFileCss = "hidden";
            FileTypeLinkCss = "hidden";
            FileTypeDisabled = "disabled";
            SourcePathHidden = "hidden";
            TransactionID = Guid.NewGuid().ToString();
            HeaderModal = Resources.HR.Kiosk.title_CreateNode;
            ButtonAcceptModal = "";
            ParentsNodesList = new SelectList(new List<SelectListItem>());
            SectionsList = new SelectList(new List<SelectListItem>());
            BackgroundColor = "#ffffff";
            FontColor = "#000000";
            IsRoot = false;
            SizeClass = "col-xs-6";
        }
    }
}