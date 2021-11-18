using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;
using SBOSysTacV2.HtmlHelperClass;

namespace SBOSysTacV2.ViewModel
{
    public class CancelBookingViewModel
    {
        public int TransId { get; set; }
        [Required(ErrorMessage = "Date Required")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:mm/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CancelDate { get; set; }
        public string CustomerFullname { get; set; }
        public DateTime EventDate { get; set; }
        public string Occasion { get; set; }
        public string PackageDesc { get; set; }
        public string Venue { get; set; }
        public int NoofPax { get; set; }
        public Decimal AmountDue { get; set; }
        public Decimal AmountPaid { get; set; }
        [Required(ErrorMessage = "Pls. Specify Reason for Cancellation")]
        public string ReasonforCancel { get; set; }

        public bool isRefundable { get; set; }

        private readonly BookingPaymentsViewModel bookingPayments = new BookingPaymentsViewModel();
        private readonly TransactionDetailsViewModel transdetails = new TransactionDetailsViewModel();
        private PegasusEntities _dbcontext=new PegasusEntities();



        public CancelBookingViewModel GetCancelledBooking(int transId)
        {
            CancelBookingViewModel cancelledBooking = null;

            var booking = (from b in _dbcontext.Bookings where b.trn_Id.Equals(transId) select b).FirstOrDefault();
            if (booking != null)
            {
                cancelledBooking = new CancelBookingViewModel()
                {
                    TransId = booking.trn_Id,
                    CustomerFullname = Utilities.Getfullname(booking.Customer.lastname, booking.Customer.firstname, booking.Customer.middle),
                    EventDate = (DateTime)booking.startdate,
                    PackageDesc = booking.Package.p_descripton,
                    NoofPax =(int) booking.noofperson,
                    Venue = booking.venue,
                    CancelDate = DateTime.Now,
                    AmountDue = bookingPayments.Get_TotalAmountBook(transId),
                    AmountPaid = transdetails.GetTotalPaymentByTrans(transId),
                };
            }

            _dbcontext.Dispose();

            return cancelledBooking;
        }
    }
}