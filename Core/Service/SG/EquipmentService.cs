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
    public class EquipmentService
    {
        private static EquipmentRepository _rep;
        static EquipmentService()
        {
            _rep = new EquipmentRepository();
        }

        public static List<Equipment> List(GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, null, null, null, request))
            {
                List<Equipment> _list = dt.ConvertToList<Equipment>();
                return _list;
            }
        }

        public static Equipment Get(int equipmentID, GenericRequest request)
        {
            using (DataTable dt = _rep.List(equipmentID, null, null, null, null, request))
            {
                List<Equipment> _list = dt.ConvertToList<Equipment>();
                if (_list.Count > 0)
                {
                    return _list.FirstOrDefault();
                }
                else
                {
                    return new Equipment();
                }
            }
        }


        public static List<Equipment> List(string UPC, GenericRequest request)
        {
            using (DataTable dt = _rep.List(null, null, null, null, UPC, request))
            {
                List<Equipment> _list = dt.ConvertToList<Equipment>();
                return _list;
            }
        }

        public static List<Equipment> List(int? EquipmentID, string Serial, string EquipmentName, string EquipmentTypeIDs, string UPC, GenericRequest request)
        {
            using (DataTable dt = _rep.List(EquipmentID, Serial, EquipmentName, EquipmentTypeIDs, UPC, request))
            {
                List<Equipment> _list = dt.ConvertToList<Equipment>();
                return _list;
            }
        }

        public static List<Equipment> GetHistoryEquipment(string AccessCode, GenericRequest request)
        {
            using (DataTable dt = _rep.GetHistoryEquipment(AccessCode, request))
            {
                List<Equipment> _list = dt.ConvertToList<Equipment>();
                return _list;
            }
        }

        public static GenericReturn Insert(Equipment equipment, GenericRequest request)
        {
            return _rep.Insert(equipment, request);
        }

        public static GenericReturn Update(Equipment equipment, GenericRequest request)
        {
            return _rep.Update(equipment, request);
        }


        public static GenericReturn Delete(int? EquipmentID, GenericRequest request)
        {
            return _rep.Delete(EquipmentID, request);
        }

    }
}
