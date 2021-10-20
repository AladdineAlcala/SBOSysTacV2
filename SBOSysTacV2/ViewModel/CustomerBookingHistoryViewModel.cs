using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class CustomerBookingHistoryViewModel
    {
        public int TransId { get; set; }
        public Customer Customer { get; set; }
    }
}