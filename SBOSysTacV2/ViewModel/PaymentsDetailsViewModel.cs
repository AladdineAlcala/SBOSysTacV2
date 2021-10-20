using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class PaymentsDetailsViewModel
    {
        public int transId { get; set; }
        public BookingsViewModel bookingview { get; set; }
       


        public PaymentsDetailsViewModel PayDetails(int trans_Id)
        {
            var _dbcontext=new PegasusEntities();

            var bk = new BookingsViewModel();
            var bookings = bk.GetListofBookings().ToList();
            var pDetails=new PaymentsDetailsViewModel();


            pDetails = (from b in bookings
                select new PaymentsDetailsViewModel()
                {
                    transId = trans_Id,
                    bookingview = b

                }).FirstOrDefault(x => x.transId==trans_Id);

            return pDetails;
        }
    }
}