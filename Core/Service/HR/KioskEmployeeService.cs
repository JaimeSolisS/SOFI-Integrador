using Core.Data;
using Core.Entities;
using Core.Entities.SQL_DataType;
using Core.Entities.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class KioskEmployeeService
    {
        private static KioskEmployeeRepository _rep;

        static KioskEmployeeService()
        {
            _rep = new KioskEmployeeRepository();
        }

        public static GenericReturn Comments_Insert(int KioskCommentCategoryID, string Comment, string EmployeeID, GenericRequest request)
        {
            return _rep.Comments_Insert(KioskCommentCategoryID, Comment, EmployeeID, request);
        }

        public static List<KioskUserInfo> GetUserData(string UserCode)
        {
            using (DataTable dt = _rep.GetUserData(UserCode))
            {
                List<KioskUserInfo> _list = dt.ConvertToList<KioskUserInfo>();
                return _list;
            }
        }

        public static List<KioskEmployeMovements> GetEmployeeMovements(string UserPrivateInfo, GenericRequest request)
        {
            using (DataTable dt = _rep.GetEmployeeMovements(UserPrivateInfo, request))
            {
                List<KioskEmployeMovements> _list = dt.ConvertToList<KioskEmployeMovements>();
                return _list;
            }
        }

        public static List<KioskNotificationsDetail> GetNotificationsUnreaded(string EmployeeUserID, GenericRequest request)
        {
            using (DataTable dt = _rep.GetNotifications(null, null, EmployeeUserID, false, request))
            {
                List<KioskNotificationsDetail> _list = dt.ConvertToList<KioskNotificationsDetail>();
                return _list;
            }
        }

        public static List<KioskPrePayroll> GetPrePayroll(string ClaveEmp)
        {
            List<KioskPrePayroll> List = new List<KioskPrePayroll>();
            using (DataSet ds = _rep.GetPrePayroll(ClaveEmp))
            {

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    List.Add(new KioskPrePayroll
                    {
                        Fecha = dr.Field<DateTime>("Fecha"),
                        Ent1 = dr.Field<string>("Ent1"),
                        Sal1 = dr.Field<string>("Sal1"),
                        Ent2 = dr.Field<string>("Ent2"),
                        Sal2 = dr.Field<string>("Sal2"),
                        Ent3 = dr.Field<string>("Ent3"),
                        Sal3 = dr.Field<string>("Sal3"),
                        Ent4 = dr.Field<string>("Ent4"),
                        Sal4 = dr.Field<string>("Sal4"),
                        Observaciones = dr.Field<string>("Observacion"),
                        HorasLaborales = dr.Field<Decimal>("Horas Laboradas"),
                        HorasExtras = dr.Field<Decimal>("Horas Extra")
                    });
                }
                return List;
            }
        }
        public static List<KioskPaymentReceipt> GetPaymentReceipts(string ClaveEmp)
        {
            List<KioskPaymentReceipt> List = new List<KioskPaymentReceipt>();
            using (DataSet ds = _rep.GetPaymentReceipts(ClaveEmp))
            {

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    List.Add(new KioskPaymentReceipt
                    {
                        Recibo = dr.Field<string>("Recibo"),
                        Receptor = dr.Field<string>("Receptor"),
                        FolioSobre = dr.Field<string>("Folio Sobre"),
                        aFiscal = dr.Field<int>("aFiscal"),
                        sFiscal = dr.Field<int>("sFiscal")
                        //ByteXML = dr.Field<byte[]>("byteXML"),
                        //BytePDF = dr.Field<byte[]>("bytePDF")
                    });
                }
                return List;
            }
        }

        public static GenericReturn Receipts_Notifications(string EmployeeNumber, string ReceiptNumber, string InvoiceName, string HTMLBodyInfo, string eMailAddressTo, string eMailAttachments, GenericRequest request)
        {
            return _rep.Receipts_Notifications(EmployeeNumber, ReceiptNumber, InvoiceName, HTMLBodyInfo, eMailAddressTo, eMailAttachments, request);
        }

        public static KioskCoursesPermit GetUserCoursesPermit(string UserCode, GenericRequest request)
        {
            using (DataTable dt = _rep.GetUserCoursesPermit(UserCode, request))
            {
                List<KioskCoursesPermit> _list = dt.ConvertToList<KioskCoursesPermit>();
                return _list.FirstOrDefault();
            }
        }

        public static List<KioskCourses> GetCoursesAvailable(string clave, GenericRequest request)
        {
            using (DataTable dt = _rep.GetCoursesAvailable(clave, request))
            {
                List<KioskCourses> _list = dt.ConvertToList<KioskCourses>();
                return _list;
            }
        }

        public static List<KioskCourses> GetCoursesInscribed(string clave, GenericRequest request)
        {
            using (DataTable dt = _rep.GetCoursesInscribed(clave, request))
            {
                List<KioskCourses> _list = dt.ConvertToList<KioskCourses>();
                return _list;
            }
        }

        #region PointsSystem
        public static List<KioskExchangeableItem> GetAvailableItems(string EmployeeNumber, GenericRequest request)
        {
            using (DataTable dt = _rep.Exec_PointSystemQueriesByEmployee(EmployeeNumber, "GetAvailableItems", null, request))
            {
                List<KioskExchangeableItem> _list = dt.ConvertToList<KioskExchangeableItem>();
                return _list;
            }
        }

        public static List<KioskUserPointsMovement> GetPointsMovements(string EmployeeNumber, GenericRequest request)
        {
            using (DataTable dt = _rep.Exec_PointSystemQueriesByEmployee(EmployeeNumber, "GetPointsMovements", null, request))
            {
                List<KioskUserPointsMovement> _list = dt.ConvertToList<KioskUserPointsMovement>();
                return _list;
            }
        }

        public static List<KioskUserPendingPointsExchange> GetPendingPetitions(string EmployeeNumber, string RFC, GenericRequest request)
        {
            using (DataTable dt = _rep.Exec_PointSystemQueriesByEmployee(EmployeeNumber, "GetPendingPetitions", RFC, request))
            {
                List<KioskUserPendingPointsExchange> _list = dt.ConvertToList<KioskUserPendingPointsExchange>();
                return _list;
            }
        }

        public static GenericReturn Insert_PointsExchangePetitions(string ClaveEmp, string RFC, List<KioskExchangeItemsForList> listaArticulos, GenericRequest request)
        {
            using (DataTable dt = listaArticulos.Select(x => new
            {
                x.clave,
                x.idConcepto,
                x.puntos
            }).ToList().ConvertToDataTable())

            {
                return _rep.Insert_PointsExchangePetitions(ClaveEmp, RFC, dt, request);
            }
        }

        public static GenericReturn InscribeUserToCourse(int? CourseID, string EmployeeNumber, string Name, GenericRequest request)
        {
            return _rep.InscribeUserToCourse(CourseID, EmployeeNumber, Name, request);
        }

        public static List<KioskPaymentReceipt> GetReceiptsAttachmentsByIDs(string EmployeeID, string Invoices, GenericRequest request)
        {
            List<KioskPaymentReceipt> List = new List<KioskPaymentReceipt>();
            using (DataSet ds = _rep.GetReceiptsAttachmentsByIDs(EmployeeID, Invoices, request))
            {
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    List.Add(new KioskPaymentReceipt
                    {
                        FolioSobre = ds.Tables[i].AsEnumerable().Select(r => r.Field<string>("Folio Sobre")).FirstOrDefault(),
                        BytePDF = ds.Tables[i].AsEnumerable().Select(r => r.Field<byte[]>("BytePDF")).FirstOrDefault(),
                        ByteXML = ds.Tables[i].AsEnumerable().Select(r => r.Field<byte[]>("ByteXML")).FirstOrDefault()
                    });
                }

                return List;
            }
        }

        public static List<KioskUserInfo> AutocompleteUserInfo(string UserCode, GenericRequest request)
        {
            using (DataTable dt = _rep.AutocompleteUserInfo(UserCode, request))
            {
                List<KioskUserInfo> _list = dt.ConvertToList<KioskUserInfo>();
                return _list;
            }

        }
        #endregion
    }
}
