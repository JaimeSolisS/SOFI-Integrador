using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class CheckListService
    {
        private static CheckListRepository _rep;

        static CheckListService()
        {
            _rep = new CheckListRepository();
        }

        public static List<CheckListTemplatesDetail> TemplatesDetail_List(int? CheckListTemplateDetailID, int? CheckListTemplateID, string CheckListName, int? Seq, string Question, GenericRequest request)
        {
            using (DataTable dt = _rep.TemplatesDetail_List(CheckListTemplateDetailID, CheckListTemplateID, CheckListName, Seq, Question, request))
            {
                List<CheckListTemplatesDetail> _list = dt.ConvertToList<CheckListTemplatesDetail>();
                return _list;
            }
        }

        public static List<CheckListTemplatesDetail> TemplatesDetail_List(string CheckListName, GenericRequest request)
        {
            using (DataTable dt = _rep.TemplatesDetail_List(null, null, CheckListName, null, null, request))
            {
                List<CheckListTemplatesDetail> _list = dt.ConvertToList<CheckListTemplatesDetail>();
                return _list;
            }
        }

        public static List<CheckListTemplate> Templates_List(int? CheckListTemplateID, string CheckListName, bool? Enabled, int? OrganizationID, GenericRequest request)
        {
            using (DataTable dt = _rep.Templates_List(CheckListTemplateID, CheckListName, Enabled, OrganizationID, request))
            {
                List<CheckListTemplate> _list = dt.ConvertToList<CheckListTemplate>();
                return _list;
            }
        }

        public static CheckListTemplate GetTemplateInfo(string CheckListName, GenericRequest request)
        {
            CheckListTemplate _entity = new CheckListTemplate();
            using (DataTable dt = _rep.Templates_List(null, CheckListName, null, null, request))
            {
                List<CheckListTemplate> _list = dt.ConvertToList<CheckListTemplate>();
                if (_list.Count() > 0)
                {
                    return _list.FirstOrDefault();
                }
                return _entity;
            }
        }

    }
}
