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
    public class CI_DashboardCarouselVideosService
    {
        private static CI_DashboardCarouselVideosRepository _rep;

        static CI_DashboardCarouselVideosService()
        {
            _rep = new CI_DashboardCarouselVideosRepository();
        }

        #region Methods

        public static List<DashboardCarouselVideos> List(int? DashboardCarouselVideoID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(DashboardCarouselVideoID, request))
            {
                List<DashboardCarouselVideos> _list = dt.ConvertToList<DashboardCarouselVideos>();
              
                return _list;
            }
        }

        public static DashboardCarouselVideos GetBySeq(int? CurrentSeq, GenericRequest request)
        {
            var video = new DashboardCarouselVideos();
            using (DataTable dt = _rep.GetBySeq(CurrentSeq, request))
            {
                List<DashboardCarouselVideos> _list = dt.ConvertToList<DashboardCarouselVideos>();
                if (_list != null && _list.Count > 0)
                { video = _list[0]; }
            }

            return video;
        }


        #endregion
    }
}
