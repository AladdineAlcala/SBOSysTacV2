using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class EventsViewModel
    {
        private PegasusEntities _dbEntities=new PegasusEntities();
        private BookingsViewModel bookModel=new BookingsViewModel();
        private  ReservationViewModel resModel=new ReservationViewModel();
        private CustomerBookingsViewModel cusModel=new CustomerBookingsViewModel();

        public int EventId { get; set; }
        public Customer eventCustomer { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string eventLocation { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDatetime { get; set; }
        public string eventType { get; set; }
        public string packageType { get; set; }
        public string booktype { get; set; }
        public bool Allday { get; set; }



        public IEnumerable<EventsViewModel> GetAllBookingEvents()
        {
            List<BookingsViewModel> eventbookings = new List<BookingsViewModel>();

            eventbookings = bookModel.GetListofBookings().Where(x=>x.iscancelled==false).ToList();

            return (from e in eventbookings
                    select new EventsViewModel
                    {
                        EventId = e.trn_Id,
                        EventName = e.occasion,
                        EventDescription = e.venue,
                        StartDateTime = e.startdate,
                        EndDatetime = e.enddate,
                        packageType = e.packageType,
                        booktype = e.booktypecode?.Trim() ?? "",
                        eventType = "booking",
                        Allday = false
                    }).ToList();


        }

        public IEnumerable<EventsViewModel> GetAllReservationEvents()
        {
            
            List<ReservationViewModel> eventreservations=new List<ReservationViewModel>();

            eventreservations = resModel.GetAll_Reservations().ToList();

            return (from e in eventreservations
                select new EventsViewModel()
                {
                    EventId =Convert.ToInt32(e.reservationId),
                    EventName = e.occasion,
                    EventDescription =  e.eventVenue,
                    StartDateTime = e.reserveDate,
                    EndDatetime = e.reserveDate,
                    //booktype = e.booktypecode?.Trim() ?? "",
                    packageType ="none",
                    eventType = "reservation",
                    Allday = false

                }).ToList();
        }



        public IEnumerable<EventsViewModel> MixedEvents()
        {
            List<EventsViewModel> listofAllEvents=new List<EventsViewModel>();

            listofAllEvents.AddRange(this.GetAllBookingEvents());
            listofAllEvents.AddRange(this.GetAllReservationEvents());

            return listofAllEvents.ToList();

        }

        public IEnumerable<CustomerBookingsViewModel> MixedEventsDayPreview()
        {
            List<CustomerBookingsViewModel> listofdailyevents=new List<CustomerBookingsViewModel>();

            listofdailyevents.AddRange(this.cusModel.GetCusBookings());
            listofdailyevents.AddRange(this.cusModel.getAllReservationBookings());

            return listofdailyevents.ToList();
        }


    }
}