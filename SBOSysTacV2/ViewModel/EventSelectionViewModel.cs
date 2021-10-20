using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBOSysTacV2.ViewModel
{
    public class EventSelectionViewModel
    {
        public DateTime eventdateselected { get; set; }
        public IEnumerable<CustomerBookingsViewModel> eventlist { get; set; }
    }
}