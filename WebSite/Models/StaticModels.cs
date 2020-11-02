using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Models
{
    public class StaticModels
    {
        public static List<Languages> AvailableLanguages = new List<Languages> {
            new Languages {
                LanguageFullName = "English", LanguageCultureName = "EN-US"
            },
            new Languages {
                LanguageFullName = "Spanish", LanguageCultureName = "ES-MX"
            }

        };
        public enum NotifyType
        {
            warning, // orage
            info,   // blue
            success,// green
            error   // red
        }
        public enum AttachmentFile
        {
            Image,
            File,
            Other
        }

        public static List<Month> MonthsList = new List<Month>
        {
            new Month {monthid =1,monthname =new DateTime(2019, 1, 1).ToString("MMM") },
            new Month {monthid =2,monthname =new DateTime(2019, 2, 1).ToString("MMM") },
            new Month {monthid =3,monthname =new DateTime(2019, 3, 1).ToString("MMM") },
            new Month {monthid =4,monthname =new DateTime(2019, 4, 1).ToString("MMM") },
            new Month {monthid =5,monthname =new DateTime(2019, 5, 1).ToString("MMM") },
            new Month {monthid =6,monthname =new DateTime(2019, 6, 1).ToString("MMM") },
            new Month {monthid =7,monthname =new DateTime(2019, 7, 1).ToString("MMM") },
            new Month {monthid =8,monthname =new DateTime(2019, 8, 1).ToString("MMM") },
            new Month {monthid =9,monthname =new DateTime(2019, 9, 1).ToString("MMM") },
            new Month {monthid =10,monthname =new DateTime(2019, 10, 1).ToString("MMM") },
            new Month {monthid =11,monthname =new DateTime(2019, 11, 1).ToString("MMM") },
            new Month {monthid =12,monthname =new DateTime(2019, 12, 1).ToString("MMM") }
        };
    }
    public class Languages
    {
        public string LanguageFullName
        {
            get;
            set;
        }
        public string LanguageCultureName
        {
            get;
            set;
        }
    }
    public class dataTablesLang
    {
        public string sProcessing { get; set; }
        public string sLengthMenu { get; set; }
        public string sZeroRecords { get; set; }
        public string sEmptyTable { get; set; }
        public string sInfo { get; set; }
        public string sInfoEmpty { get; set; }
        public string sInfoFiltered { get; set; }
        public string sInfoPostFix { get; set; }
        public string sSearch { get; set; }
        public string sUrl { get; set; }
        public string sInfoThousands { get; set; }
        public string sLoadingRecords { get; set; }

        public dataTablesLangoPaginate oPaginate { get; set; }
        public dataTablesLangoAria oAria { get; set; }
        public class dataTablesLangoPaginate
        {
            public string sFirst { get; set; }
            public string sLast { get; set; }
            public string sNext { get; set; }
            public string sPrevious { get; set; }
        }

        public class dataTablesLangoAria
        {
            public string sSortAscending { get; set; }
            public string sSortDescending { get; set; }
        }
    }
    public class JsTreeModel
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public Jsli li_attr { get; set; }
        public Jsa a_attr { get; set; }
        public JsData data { get; set; }
        public JsTreeModelState state { get; set; }


    }
    public class JsTreeModelState
    {
        public bool opened { get; set; }
        public bool disabled { get; set; }
        public bool selected { get; set; }
    }
    public class Jsli
    {
        public string style { get; set; }
    }
    public class Jsa
    {
        public string style { get; set; }

    }
    public class JsData
    {
        public string entityid { get; set; }
    }

    public class Month {
        public int monthid { get; set; }
        public string monthname { get; set; }

    }
}