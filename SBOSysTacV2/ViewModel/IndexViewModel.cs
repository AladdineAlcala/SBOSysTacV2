using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class IndexViewModel
    {

        public BookingsCountViewModel bookingscount = new BookingsCountViewModel();

        public BookingScheduleViewModel bookingsschedulecount = new BookingScheduleViewModel();

        public ReservationCountViewModel reservationViewModel=new ReservationCountViewModel();

        //public SalesCountViewModel salescount=new SalesCountViewModel();

        public IndexViewModel()
        {
            //this.salescount=new SalesCountViewModel();
            //this.bookingscount=new BookingsCountViewModel();
        }

      
    }
}