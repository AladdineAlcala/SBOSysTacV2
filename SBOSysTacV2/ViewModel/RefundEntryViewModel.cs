using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBOSysTacV2.ViewModel
{
    public class RefundEntryViewModel
    {
        public int transId { get; set; }
        public decimal t_amtBooking { get; set; }
        public decimal t_amountPayment { get; set; }
        public BookingsViewModel Bookings { get; set; }
        public BookingRefundViewModel bookingrefund { get; set; }
    }
}