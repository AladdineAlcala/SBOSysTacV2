using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    //bookingscountClass
    public class BookingsCountViewModel
    {
        public int todateBooking { get; set; }
        public int thisWeekBooking { get; set; }
        public int thisMonthBooking { get; set; }

        private PegasusEntities dbEntities;
        public BookingsCountViewModel()
        {
            this.todateBooking = getTodateBookings();
            this.thisWeekBooking = getThisWeekBookings();
            this.thisMonthBooking = get_thisMonthBookings();


        }

        private int getTodateBookings()
        {
            dbEntities = new PegasusEntities();

            return dbEntities.Bookings.Count() != 0 ? dbEntities.Bookings.Count() : 0;
        }

        private int getThisWeekBookings()
        {
            dbEntities = new PegasusEntities();

            //DateTime firstday = DateTime.Now.AddDays(-(int) DateTime.Now.DayOfWeek);

            DateTime startDayofWeek = DateTime.Today.AddDays(-1 * (int)DateTime.Today.DayOfWeek);
            DateTime endDayofWeek = DateTime.Today.AddDays(6 - (int)DateTime.Today.DayOfWeek);

            int totalbookthisWeek = dbEntities.Bookings
                .Where(x => DbFunctions.TruncateTime(x.transdate) >= DbFunctions.TruncateTime(startDayofWeek) && DbFunctions.TruncateTime(x.transdate) <= DbFunctions.TruncateTime(endDayofWeek)).ToList().Count;

            return totalbookthisWeek;
        }

        private int get_thisMonthBookings()
        {

            dbEntities = new PegasusEntities();

            DateTime now = DateTime.Now;
            DateTime startDayofMonth = new DateTime(now.Year, now.Month, 1);
            DateTime endDayofMonth = startDayofMonth.AddMonths(1).AddDays(-1);

            int totalbookthisMonth = dbEntities.Bookings
                .Where(x => DbFunctions.TruncateTime(x.transdate) >= DbFunctions.TruncateTime(startDayofMonth) && DbFunctions.TruncateTime(x.transdate) <= DbFunctions.TruncateTime(endDayofMonth)).ToList().Count;

            return totalbookthisMonth;
        }
    }

    //bookingsSchedulecountClass
    public class BookingScheduleViewModel
    {
        public decimal TodateBookingSchedule { get; set; }
        public decimal thisWeekBookingSchedule { get; set; }
        public decimal thisMonthBookingSchedule { get; set; }
        private PegasusEntities dbEntities;
        public BookingScheduleViewModel()
        {
            this.TodateBookingSchedule = getTodateBookingSchedule();
            this.thisWeekBookingSchedule = getThisWeekBookingSchedule();
            this.thisMonthBookingSchedule = getThisMonthBookingSchedule();
        }

        public int getTodateBookingSchedule()
        {
            dbEntities = new PegasusEntities();

            DateTime now = DateTime.Now;

            return dbEntities.Bookings.Where(x => DbFunctions.TruncateTime(x.startdate.Value) == DbFunctions.TruncateTime(now)).ToList().Count != 0 ? dbEntities.Bookings.Where(x => DbFunctions.TruncateTime(x.startdate.Value) == DbFunctions.TruncateTime(now)).ToList().Count : 0;

            //return dbEntities.Bookings.Count() != 0 ? dbEntities.Bookings.Count() : 0;
        }

        public int getThisWeekBookingSchedule()
        {
            dbEntities = new PegasusEntities();

            //DateTime firstday = DateTime.Now.AddDays(-(int) DateTime.Now.DayOfWeek);

            DateTime startDayofWeek = DateTime.Today.AddDays(-1 * (int)DateTime.Today.DayOfWeek);
            DateTime endDayofWeek = DateTime.Today.AddDays(6 - (int)DateTime.Today.DayOfWeek);

            int totalbookschedulethisWeek = dbEntities.Bookings
                .Where(x => DbFunctions.TruncateTime(x.startdate.Value) >= DbFunctions.TruncateTime(startDayofWeek) && DbFunctions.TruncateTime(x.startdate.Value) <= DbFunctions.TruncateTime(endDayofWeek)).ToList().Count;

            return totalbookschedulethisWeek;
        }

        public int getThisMonthBookingSchedule()
        {
            dbEntities = new PegasusEntities();

            DateTime now = DateTime.Now;
            DateTime startDayofMonth = new DateTime(now.Year, now.Month, 1);
            DateTime endDayofMonth = startDayofMonth.AddMonths(1).AddDays(-1);

            int totalbookschedulethisMonth = dbEntities.Bookings
                .Where(x => DbFunctions.TruncateTime(x.startdate) >= DbFunctions.TruncateTime(startDayofMonth) && DbFunctions.TruncateTime(x.startdate) <= DbFunctions.TruncateTime(endDayofMonth)).ToList().Count;

            return totalbookschedulethisMonth;

        }
    }

    //salescountclass
    //public class SalesCountViewModel
    //{
    //    public decimal TodateSales { get; set; }
    //    public decimal thisWeekSales { get; set; }
    //    public decimal thisMonthSales { get; set; }

    //    private PegasusEntities dbEntities;

    //    public SalesCountViewModel()
    //    {
            
    //    }

    //    public decimal getTodateSales()
    //    {
    //        return 0;
    //    }
    //}

    //reservationcountclass
    public class ReservationCountViewModel
    {
        public int toDateReservation { get; set; }
        public int thisWeekReservation { get; set; }
        public int thisMonthReservation { get; set; }
        private PegasusEntities dbEntities;
        public ReservationCountViewModel()
        {
            this.toDateReservation = getTodateReservation();
            this.thisWeekReservation = getThisWeekReservation();
            this.thisMonthReservation = getThisMonthReservation();

        }

        public int getTodateReservation()
        {
            dbEntities = new PegasusEntities();

            return dbEntities.Reservations.ToList().Count;
        }

        public int getThisWeekReservation()
        {
            dbEntities = new PegasusEntities();

            //DateTime firstday = DateTime.Now.AddDays(-(int) DateTime.Now.DayOfWeek);

            DateTime startDayofWeek = DateTime.Today.AddDays(-1 * (int)DateTime.Today.DayOfWeek);
            DateTime endDayofWeek = DateTime.Today.AddDays(6 - (int)DateTime.Today.DayOfWeek);

            int totalreserveschedulethisWeek = dbEntities.Reservations
                .Where(x => DbFunctions.TruncateTime(x.resDate) >= DbFunctions.TruncateTime(startDayofWeek) && DbFunctions.TruncateTime(x.resDate) <= DbFunctions.TruncateTime(endDayofWeek)).ToList().Count;

            return totalreserveschedulethisWeek;
        }

        public int getThisMonthReservation()
        {

            dbEntities = new PegasusEntities();

            DateTime now = DateTime.Now;
            DateTime startDayofMonth = new DateTime(now.Year, now.Month, 1);
            DateTime endDayofMonth = startDayofMonth.AddMonths(1).AddDays(-1);

            int totalreservechedulethisMonth = dbEntities.Reservations
                .Where(x => DbFunctions.TruncateTime(x.resDate) >= DbFunctions.TruncateTime(startDayofMonth) && DbFunctions.TruncateTime(x.resDate) <= DbFunctions.TruncateTime(endDayofMonth)).ToList().Count;

            return totalreservechedulethisMonth;
        }
    }

    public class DatapointLine
    {

        //[DataMember(Name = "x")]
        //public string X = null;

        ////setting the name to be used whenserializing to JSON.
        //[DataMember(Name = "y")]
        //public Nullable<double> Y = null;

        //public DatapointLine(string x, double y)
        //{
        //    this.X = x;
        //    this.Y = y;
        //}

        public string X { get; set; }
        public int Y { get; set; }

    }
}