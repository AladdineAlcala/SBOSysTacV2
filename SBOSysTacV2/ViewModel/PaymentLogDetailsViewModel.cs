using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBOSysTacV2.ViewModel
{
    public class PaymentLogDetailsViewModel
    {
        public int transId { get; set; }
        public DateTime paymentDate { get; set; }
        public decimal totalBookingsAmount { get; set; }
        public decimal totalPayment { get; set; }

    }
}