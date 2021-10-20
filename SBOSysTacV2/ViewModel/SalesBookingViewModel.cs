using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBOSysTacV2.ViewModel
{
    public class SalesBookingViewModel
    {
        public int transId { get; set; }
        public DateTime d_of_transaction { get; set; }
        public decimal totalAmountdue { get; set; }
    }
}