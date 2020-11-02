using Core.Data;
using System;

namespace Core.Service
{
    public static class CI_DashboardVisitService
    {
        private static CI_DashboardVisitRepository _rep;

        static CI_DashboardVisitService()
        {
            _rep = new CI_DashboardVisitRepository();
        }

        public static int Add(int UserID)
        {
            return _rep.Add(UserID);
        }

        public static int Counter(DateTime? Date)
        {
            return _rep.Counter(Date);
        }
    }
}
