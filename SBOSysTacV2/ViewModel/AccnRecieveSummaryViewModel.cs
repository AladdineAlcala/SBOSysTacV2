using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.Models;
using SBOSysTacV2.ServiceLayer;

namespace SBOSysTacV2.ViewModel
{
    public class AccnRecieveSummaryViewModel
    {
        public int cusId { get; set; }
        public string cusname { get; set; }
        public int transId { get; set; }
        public DateTime transDate { get; set; }
        public DateTime duedate { get; set; }
        public int daysOdd { get; set; }
        public decimal balance { get; set; }
        public decimal refunds { get; set; }

        static Func<int, decimal> _getBookingAmount = BookingsService.Get_TotalAmountBook;
        public IEnumerable<AccnRecieveSummaryViewModel> GetAllAccnRecievables()
        {
            List<AccnRecieveSummaryViewModel> listAccn=new List<AccnRecieveSummaryViewModel>();
            var _dbentities=new PegasusEntities();
            var bookingPayments = new BookingPaymentsViewModel();
            var transdetails = new TransactionDetailsViewModel();
            var bookingRefund = new BookingRefundViewModel();
           

            try
            {

                var bookings = (from booking in _dbentities.Bookings where booking.is_cancelled==false select booking).OrderBy(x => x.Customer.lastname).ToList();


                listAccn = (from b in bookings
                    //let daydue = Convert.ToDateTime(b.transdate).AddDays(30)
                    let eventdatedue = Convert.ToDateTime(b.startdate).AddDays(30)
                    where b.startdate != null && DateTime.Now.Subtract((DateTime) b.startdate).Days >= 0

                    select new AccnRecieveSummaryViewModel
                    {
                        cusId = Convert.ToInt32(b.c_Id),
                        cusname = Utilities.Getfullname(b.Customer.lastname, b.Customer.firstname, b.Customer.middle),
                        transId = b.trn_Id,
                        transDate = Convert.ToDateTime(b.startdate),
                        duedate = eventdatedue,
                        daysOdd = Convert.ToInt32(DateTime.Now.Subtract(eventdatedue).Days) <= 0
                            ? 0
                            : Convert.ToInt32(DateTime.Now.Subtract(eventdatedue).Days),

                        balance = _getBookingAmount(b.trn_Id) - transdetails.GetTotalPaymentByTrans(b.trn_Id)
                        
                             

                    }).ToList();


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return listAccn.Where(x=>x.balance>0).ToList();
        }
    }
}