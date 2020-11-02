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
    public class CompaniesService
    {
        private static CompaniesRepository _rep;

        static CompaniesService()
        {
            _rep = new CompaniesRepository();
        }

        public static List<Company> ListUserAccess (GenericRequest request){
		using (DataTable dt = _rep.ListUserAccess (request))
		{
			List<Company> _list = dt.ConvertToList<Company>();
			return _list;
		}
}
    }
}
