using Core.Data;
using Core.Entities;
using Core.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace Core.Service
{
    public class MNT_EnergySensorsFamiliesService
    {
        private static MNT_EnergySensorsFamiliesRepository _rep;

        static MNT_EnergySensorsFamiliesService()
        {
            _rep = new MNT_EnergySensorsFamiliesRepository();
        }

        public static GenericReturn Insert(string FamilyName, decimal? MaxValueperHour, string ImagePath, bool? Enabled, GenericRequest request)
        {
            return _rep.Insert(FamilyName, MaxValueperHour, ImagePath, Enabled, request);
        }

        public static List<EnergySensorFamilies> Dashboard_List(GenericRequest req)
        {
            using (DataTable dt = _rep.Dashboard_List(null, null, req))
            {
                List<EnergySensorFamilies> _list = dt.ConvertToList<EnergySensorFamilies>();
                return _list;
            }
        }

        public static List<EnergySensorFamilies> Dashboard_List(DateTime? Date, GenericRequest req)
        {
            using (DataTable dt = _rep.Dashboard_List(Date, null, req))
            {
                List<EnergySensorFamilies> _list = dt.ConvertToList<EnergySensorFamilies>();
                return _list;
            }
        }


        public static List<EnergySensorFamilies> List(GenericRequest req)
        {
            using (DataTable dt = _rep.List(null, "", null, null, req))
            {
                List<EnergySensorFamilies> _list = dt.ConvertToList<EnergySensorFamilies>();
                return _list;
            }
        }

        public static List<EnergySensorFamilies> List(int? EnergySensorFamilyID, GenericRequest req)
        {
            using (DataTable dt = _rep.List(EnergySensorFamilyID, "", null, null, req))
            {
                List<EnergySensorFamilies> _list = dt.ConvertToList<EnergySensorFamilies>();
                return _list;
            }
        }

        public static List<EnergySensorFamilies> List(string FamilyName, GenericRequest req)
        {
            using (DataTable dt = _rep.List(null, FamilyName, null, null, req))
            {
                List<EnergySensorFamilies> _list = dt.ConvertToList<EnergySensorFamilies>();
                return _list;
            }
        }

        public static List<EnergySensorFamilies> List(DateTime? date, int? CurrentHour, GenericRequest req)
        {
            using (DataTable dt = _rep.List(null, "", date, CurrentHour, req))
            {
                List<EnergySensorFamilies> _list = dt.ConvertToList<EnergySensorFamilies>();
                return _list;
            }
        }


        public static GenericReturn Delete(int? EnergySensorFamilyID, GenericRequest request)
        {
            return _rep.Delete(EnergySensorFamilyID, request);
        }

        public static GenericReturn Update(int? EnergySensorFamilyID, string FamilyName, decimal? MaxValueperHour, string ImagePath, bool? Enabled, GenericRequest request)
        {
            return _rep.Update(EnergySensorFamilyID, FamilyName, MaxValueperHour, ImagePath, Enabled, request);
        }

        public static GenericReturn CopyAs(int? EnergySensorID, string SensorName, GenericRequest request)
        {
            return _rep.CopyAs(EnergySensorID, SensorName, request);
        }
    }
}
