using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class BookingAddonsViewModel
    {
        public int bookaddon_No { get; set; }
        public BookingAddon BookingAddon { get; set; }
        public IEnumerable<BookingAddonDetailsViewModel> BookAddOn_Details { get; set; }

    }
}