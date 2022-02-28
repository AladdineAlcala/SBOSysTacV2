using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
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

        public IEnumerable<AccnRecieveSummaryViewModel> GetAllAccnRecievables()
        {
            List<AccnRecieveSummaryViewModel> listAccn=new List<AccnRecieveSummaryViewModel>();
            Func<int, decimal> _getBookingAmount = BookingsService.Get_TotalAmountBook;
            Func<int, decimal> _getTotalPaymentByBooking = TransactionDetailsViewModel.GetTotalPaymentByTrans;

            var _dbentities=new PegasusEntities();
            
                try
                {

                    var bookings = (from booking in _dbentities.Bookings where booking.is_cancelled==false && booking.is_deleted==false select booking).ToList();


                         listAccn = (from b in bookings
                        //let daydue = Convert.ToDateTime(b.transdate).AddDays(30)
                        let eventdatedue = Convert.ToDateTime(b.startdate).AddDays(30)
                        where b.startdate != null && DateTime.Now.Subtract((DateTime) b.startdate).Days >= 0
                        select new
                        {
                            _transId = b.trn_Id,
                            _duedate = eventdatedue,
                            _transDate= Convert.ToDateTime(b.startdate),
                            _amtBooking = _getBookingAmount(b.trn_Id),
                            _totaPayment = _getTotalPaymentByBooking(b.trn_Id),
                            _accnNo=b.c_Id,
                            _cusname = Utilities.Getfullname(b.Customer.lastname, b.Customer.firstname, b.Customer.middle),

                        }).Select(acc=>  new AccnRecieveSummaryViewModel
                        {
                            transId =acc._transId,
                            cusId = (int)acc._accnNo,
                            cusname = acc._cusname,
                            transDate =acc._transDate,
                            duedate = acc._duedate,
                            daysOdd = Convert.ToInt32(DateTime.Now.Subtract(acc._duedate).Days) <= 0
                                ? 0
                                : Convert.ToInt32(DateTime.Now.Subtract(acc._duedate).Days),

                            balance =acc._amtBooking - acc._totaPayment



                        }).ToList();


                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            _dbentities.Dispose();

            return listAccn.Where(x=>x.balance>0).OrderBy(t=>t.cusname).ToList();
        }
    }
}