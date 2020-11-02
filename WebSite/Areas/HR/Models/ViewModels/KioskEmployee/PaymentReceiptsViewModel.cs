using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.HR.Models.ViewModels.KioskEmployee
{
    public class PaymentReceiptsViewModel
    {
        public List<KioskPaymentReceipt> PaymentReceiptsList;
        public string EmployeeNumber;

        public PaymentReceiptsViewModel()
        {
            PaymentReceiptsList = new List<KioskPaymentReceipt>();
            EmployeeNumber = "";
        }
    }
}