using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;
using System.ComponentModel.DataAnnotations;
using SBOSysTacV2.HtmlHelperClass;

namespace SBOSysTacV2.ViewModel
{
    public class CustomerBookingsViewModel
    {
        public int transId { get; set; }
        public int? cusId { get; set; }
        public string cusfullname { get; set; }
        public string occasion { get; set; }
        public string venue { get; set; }

        [DisplayFormat(DataFormatString = "{0: MMM-dd-yyyy hh:mm}")]
        public DateTime? bookdatetime { get; set; }
        public string package { get; set; }
        public string packageType { get; set; }
        public decimal packageDue { get; set; }
        public bool isServe { get; set; }
        public bool isCancelled { get; set; }
        public string transType { get; set; }
        public string bookingtype { get; set; }

        private BookingPaymentsViewModel bookingPayments = new BookingPaymentsViewModel();
        public IEnumerable<CustomerBookingsViewModel> GetCusBookings()
        {
            var _dbEntities=new PegasusEntities();


        List<CustomerBookingsViewModel> lst=new List<CustomerBookingsViewModel>();

            try
            {
                List<Booking> listbookings = new List<Booking>();

                listbookings = (from b in _dbEntities.Bookings select b).ToList();

                lst=(from l in listbookings

                    select new CustomerBookingsViewModel()
                    {
                        transId = l.trn_Id,
                        cusId = l.c_Id,
                        cusfullname= Utilities.Getfullname_nonreverse(l.Customer.lastname, l.Customer.firstname, l.Customer.middle),
                        occasion = l.occasion,
                        venue = l.venue,
                        bookdatetime = l.startdate,
                        package = l.Package.p_descripton,
                        packageType = l.Package.p_type.Trim(),
                        packageDue = bookingPayments.Get_TotalAmountBook(l.trn_Id),
                        isServe =Convert.ToBoolean(l.serve_stat),
                        isCancelled = Convert.ToBoolean(l.is_cancelled),
                        bookingtype = l.booktype?.Trim() ?? "",
                        transType = "bk"
                    }).ToList();


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return lst;

        }


        public IEnumerable<CustomerBookingsViewModel> GetCusBookingsById(int cusId)
        {
            var _dbEntities = new PegasusEntities();


            List<CustomerBookingsViewModel> lst = new List<CustomerBookingsViewModel>();

            try
            {
                List<Booking> listbookings = new List<Booking>();

                listbookings = (from b in _dbEntities.Bookings where b.c_Id == cusId select b).ToList();

                lst = listbookings.Select(l => new CustomerBookingsViewModel()
                    {
                        transId = l.trn_Id,
                        cusId = l.c_Id,
                        cusfullname = Utilities.Getfullname_nonreverse(l.Customer.lastname, l.Customer.firstname,
                            l.Customer.middle),
                        occasion = l.occasion,
                        venue = l.venue,
                        bookdatetime = l.startdate,
                        package = l.Package.p_descripton,
                        packageType = l.Package.p_type.Trim(),
                        packageDue = bookingPayments.Get_TotalAmountBook(l.trn_Id),
                        isServe = Convert.ToBoolean(l.serve_stat),
                        isCancelled = Convert.ToBoolean(l.is_cancelled),
                        bookingtype = l.booktype?.Trim() ?? "",
                        transType = "bk"
                    }).OrderByDescending(t => t.bookdatetime)
                    .ToList();




            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return lst;

        }

        public IEnumerable<CustomerBookingsViewModel> getAllReservationBookings()
        {
            var _dbEntities = new PegasusEntities();

            List<CustomerBookingsViewModel> lst = new List<CustomerBookingsViewModel>();
            List<Reservation> reservation = new List<Reservation>();


            try
            {
                reservation = (from res in _dbEntities.Reservations select res).ToList();
                lst = (from r in reservation
                    select new CustomerBookingsViewModel()
                    {
                        transId = r.resId,
                        cusId = r.c_Id,
                        cusfullname = Utilities.Getfullname_nonreverse(r.Customer.lastname, r.Customer.firstname, r.Customer.middle),
                        occasion = r.occasion,
                        venue = r.eventVenue,
                        bookdatetime = r.resDate,
                        package = "None",
                        transType = "res"

                    }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return lst;
        }
    }
}