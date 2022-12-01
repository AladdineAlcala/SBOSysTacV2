using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
   
    public class BookingAddonDetailsViewModel
    {
        public int addondetail_Id { get; set; }
        public int bookaddon_No { get; set; }
        public int addon_Id { get; set; }
        public string addon_description { get; set; }
        public string unit_measure { get; set; }
        public decimal addon_qty { get; set; }
        public decimal addon_amount { get; set; }

       

        public static List<ICollection<BookAddonsDetail>> GetAddonDetails(Booking booking)
        {
            return booking.BookingAddons.Select(t => t.BookAddonsDetails).ToList();
        }
    }
}