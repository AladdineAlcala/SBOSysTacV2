using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.Models;
using SBOSysTacV2.ServiceLayer;

namespace SBOSysTacV2.ViewModel
{
    public class RefundsViewModel
    {
        //public int Ref_Id { get; set; }
        public int TransId { get; set; }
        public int cId { get; set; }
        public bool isCancelled { get; set; }
        public bool hasrefundEntry { get; set; }
        public string CustomerName { get; set; }
        public DateTime Eventdate { get; set; }
        public string EventLocation { get; set; }
        public decimal PaymemntAmount { get; set; }
        public decimal RefundAmount { get; set; }
        public string Status { get; set; }

       
        private PegasusEntities dbentities = new PegasusEntities();
        private BookingPaymentsViewModel bookingPayments = new BookingPaymentsViewModel();
       

        public IEnumerable<RefundsViewModel> GetAllRefundsList()
        {
            Func<int, decimal> _getBookingAmount = BookingsService.Get_TotalAmountBook;

            List<RefundsViewModel> listRefunds = new List<RefundsViewModel>();

            try
            {
                var refunds = (from rf in dbentities.Refunds select rf).ToList();
                var bookings = (from booking in dbentities.Bookings where booking.serve_stat == false || booking.is_deleted == false select booking).ToList();


                    listRefunds = (from booking in bookings
                                   join rf in refunds on booking.trn_Id equals rf.trn_Id
                                   select new
                                        {
                                            _tId = booking.trn_Id,
                                            _trEvtDate = booking.startdate,
                                            _cusId = booking.c_Id,
                                            _lastname=booking.Customer.lastname,
                                            _first=booking.Customer.firstname,
                                            _mid=booking.Customer.middle,
                                            _occasion = booking.occasion,
                                            _evtVenue = booking.venue,
                                            _iscancelledbooking = booking.is_cancelled,
                                            _serveStat = booking.serve_stat,
                                            _tpackageAmt = _getBookingAmount(booking.trn_Id),
                                            _totapayment = (from p in dbentities.Payments select p).Where(s => s.trn_Id == booking.trn_Id).Sum(x => x.amtPay),
                                            _hasrefundEntry=(from rp in dbentities.Refunds select rp).Any(t => t.trn_Id== booking.trn_Id),
                                            _netRefundable=rf.rf_Amount,
                                            _totalRefundEntry=dbentities.RefundEntries.Where(t=>t.Rf_id==rf.Rf_id).Sum(t=>t.Amount)
                                   }).AsEnumerable()
                                    .Select(book => new RefundsViewModel()
                                    {
                                        TransId = book._tId,
                                        cId = (int)book._cusId,
                                        Eventdate = (DateTime)book._trEvtDate,
                                        CustomerName = Utilities.Getfullname(book._lastname, book._first, book._mid),
                                        EventLocation = book._evtVenue,
                                        isCancelled = (bool)book._iscancelledbooking,
                                        PaymemntAmount = Convert.ToDecimal(book._totapayment),
                                        RefundAmount = book._totapayment > book._tpackageAmt ? Convert.ToDecimal(book._totapayment - book._tpackageAmt) : 0,
                                        hasrefundEntry = book._hasrefundEntry,
                                        Status =book._netRefundable==book._totalRefundEntry?"serve":"pending"
                                    }).ToList();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           

            

            return listRefunds;

        }


           

        
    }
}