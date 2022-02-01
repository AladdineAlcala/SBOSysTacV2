using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class RefundTransViewModel
    {
        public int transId { get; set; }
        public BookingRefundViewModel BookingRefund { get; set; }
        public List<RefundEntry> PostRefundEntry { get; set; }
    }
}