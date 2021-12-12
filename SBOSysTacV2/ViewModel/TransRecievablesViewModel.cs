using System;
using System.Collections.Generic;
using System.Linq;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class TransRecievablesViewModel
    {
        public int transId { get; set; }
        public int cusId { get; set; }
        public string cusfullname { get; set; }
        public string address { get; set; }
        public string contact { get; set; }
        public DateTime transDate { get; set; }
        public DateTime bookdatetime { get; set; }
     
        public string occasion { get; set; }
        public string venue { get; set; }
        public string packagedetails { get; set; }
        public decimal p_amountperPax { get; set; }
        public bool iscancelled { get; set; }
        public decimal totalPackageAmount { get; set; }
        public decimal totalPayment { get; set; }
        public decimal balance { get; set; }
        public decimal refunds { get; set; }

        private readonly PegasusEntities db_entities = new PegasusEntities();
        private BookingPaymentsViewModel bookingPayments = new BookingPaymentsViewModel();
        private TransactionDetailsViewModel transdetails = new TransactionDetailsViewModel();


        public IEnumerable<TransRecievablesViewModel> GetAllRecievables(IEnumerable<Booking> tBookings)
        {
            List<TransRecievablesViewModel> recievable_list=new List<TransRecievablesViewModel>();
            try
            {

                //IEnumerable<Booking> bookings = tBookings.ToList();


                recievable_list = (from b in tBookings
                                   select new
                    {
                        _tId = b.trn_Id,
                        _trDate = b.transdate,
                        _evtDate = b.startdate,
                        _cusId = b.c_Id,
                        _cusfullname =
                        Utilities.Getfullname(b.Customer.lastname, b.Customer.firstname, b.Customer.middle),
                        _address = b.Customer.address,
                        _contact = b.Customer.contact1,
                        _occasion = b.occasion,
                        _venue = b.venue,
                        _iscancelledbooking = b.is_cancelled,
                        _packagedetails = b.Package.p_descripton,
                        _pAmountperPax = b.Package.p_amountPax,
                        _tpackageAmt = bookingPayments.Get_TotalAmountBook(b.trn_Id),
                        _totapayment = (from p in db_entities.Payments select p).Where(s => s.trn_Id == b.trn_Id)
                        .Sum(x => x.amtPay),
                        _refunds = (from re in db_entities.Refunds select re).FirstOrDefault(t => t.trn_Id == b.trn_Id)

                    }).Select(p => new TransRecievablesViewModel()
                    {
                        transId = p._tId,
                        transDate = Convert.ToDateTime(p._trDate),
                        bookdatetime = Convert.ToDateTime(p._evtDate),
                        cusId = Convert.ToInt32(p._cusId),
                        cusfullname = p._cusfullname,
                        address = p._address,
                        contact = p._contact,
                        occasion = p._occasion,
                        venue = p._venue,
                        iscancelled = Convert.ToBoolean(p._iscancelledbooking),
                        packagedetails = p._packagedetails,
                        p_amountperPax = Convert.ToDecimal(p._pAmountperPax),
                        totalPackageAmount = p._tpackageAmt,
                        totalPayment = Convert.ToDecimal(p._totapayment),
                            //balance = Convert.ToDecimal(p._totapayment)> p._tpackageAmt?Convert.ToDecimal(p._refunds) > 0 ? Convert.ToDecimal(((p._tpackageAmt - p._totapayment) + Convert.ToDecimal(p._refunds.rf_Amount))) : Convert.ToDecimal(p._tpackageAmt - p._totapayment): p._tpackageAmt,
                        balance = Convert.ToDecimal(p._totapayment) > p._tpackageAmt ? p._refunds!=null ? Convert.ToDecimal(((p._tpackageAmt - p._totapayment) + Convert.ToDecimal(p._refunds.rf_Amount))) : Convert.ToDecimal(p._tpackageAmt - p._totapayment) : p._refunds != null?0: p._tpackageAmt==p._totapayment?0: p._tpackageAmt>p._totapayment?Convert.ToDecimal(p._tpackageAmt-p._totapayment):p._tpackageAmt,
                            refunds = p._refunds != null ? Convert.ToDecimal(p._refunds.rf_Amount) : 0
                    }).ToList();
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

          


            return recievable_list;
        }


        public IEnumerable<TransRecievablesViewModel> GetRecieveDetails()
        {
            List<TransRecievablesViewModel> recievable_list = new List<TransRecievablesViewModel>();
            try
            {
                var bookings = (from booking in db_entities.Bookings select booking).ToList();

                recievable_list = (from b in bookings

                    select new TransRecievablesViewModel()
                    {


                        transId = b.trn_Id,
                        transDate = Convert.ToDateTime(b.transdate),
                        bookdatetime = Convert.ToDateTime(b.startdate),
                        cusId = Convert.ToInt32(b.Customer.c_Id),
                       
                        //cusfullname = Utilities.getfullname(b.Customer.lastname, b.Customer.firstname,
                        //    b.Customer.middle),
                        //occasion = b.occasion,
                        //venue = b.venue,
                        //packagedetails = b.Package.p_descripton,
                        //p_amountperPax = b.Package.p_amountPax,
                        totalPackageAmount = bookingPayments.Get_TotalAmountBook(b.trn_Id)

                    }).ToList();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return recievable_list.ToList();
        }


        public TransRecievablesViewModel GetRecieveDetailsByTransaction(Booking booking)
        {

            var recievable = new TransRecievablesViewModel();

            try
            {

                recievable.transId = booking.trn_Id;
                recievable.transDate = Convert.ToDateTime(booking.transdate);
                recievable.bookdatetime = Convert.ToDateTime(booking.startdate);
                recievable.cusId = Convert.ToInt32(booking.Customer.c_Id);
                recievable.totalPackageAmount = bookingPayments.Get_TotalAmountBook(booking.trn_Id);
                

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return recievable;
        }

    }
}