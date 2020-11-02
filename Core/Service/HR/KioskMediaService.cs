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
    public class KioskMediaService
    {
        private static KioskMediaRepository _rep;

        static KioskMediaService()
        {
            _rep = new KioskMediaRepository();
        }

        public static int GetKioskCarouselID(GenericRequest request)
        {
            var ErrorMessage = "";

            try
            {
                return _rep.GetKioskCarouselID(request);
            }
            catch (Exception ex)
            {
                ErrorMessage = "ERROR: " + ex.Message;
                return 0;
            }
        }
        public static List<KioskCarouselMedia> List(int? KioskCarouselVideoID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(KioskCarouselVideoID, request))
            {
                List<KioskCarouselMedia> _list = dt.ConvertToList<KioskCarouselMedia>();

                return _list;
            }
        }
        public static List<KioskCarouselMedia> List(GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, request))
            {
                List<KioskCarouselMedia> _list = dt.ConvertToList<KioskCarouselMedia>();

                return _list;
            }
        }
        public static List<KioskCarouselMediaTmp> TempList(int KioskCarouselMediaID, int TempReferenceID, GenericRequest request)
        {
            using (DataTable dt = _rep.TempList(KioskCarouselMediaID, TempReferenceID, request))
            {
                List<KioskCarouselMediaTmp> _list = dt.ConvertToList<KioskCarouselMediaTmp>();

                return _list;
            }
        }
        public static GenericReturn Delete(int KioskCarouselMediaID, GenericRequest request)
        {
            return _rep.Delete(KioskCarouselMediaID, request);
        }
        public static GenericReturn Update(string ReferenceID, List<KioskCarouselMedia> FileInfo, GenericRequest request)
        {
            //return _rep.Update(ReferenceID, FileInfo, request);
            using (DataTable dt = FileInfo.Select(x => new
            {
                x.FileName,
                x.Path
            }).ToList().ConvertToDataTable())
            {
                return _rep.Update(ReferenceID, dt, request);
            }
        }
        public static GenericReturn Insert(GenericRequest request)
        {
            return _rep.Insert(request);
        }

    }
}
