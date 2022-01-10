using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class BookingOtherChargeViewModel
    {
        public int transId { get; set; }

        public List<BookOtherChargeDetailViewModel> BookOtherChargeList{ get; set; }



        public decimal GetTotalOtherCharges(List<Book_OtherCharge> bookOtherCharges)
        {
            decimal otherCharge=0;

            try
            {

                    otherCharge = bookOtherCharges.Count > 0 ?(decimal)bookOtherCharges.Select(t => t.amount).Sum():0;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return otherCharge;

        }


        
    }
}