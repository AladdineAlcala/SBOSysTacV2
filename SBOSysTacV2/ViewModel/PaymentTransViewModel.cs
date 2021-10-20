using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class PaymentTransViewModel
    {
        public int transId { get; set; }
        public Booking Booking { get; set; }
        public TransRecievablesViewModel TransRecievables { get; set; }
        public IEnumerable<PaymentsViewModel> Payments { get; set; }
        public Refund refundentry { get; set; }


    }
}