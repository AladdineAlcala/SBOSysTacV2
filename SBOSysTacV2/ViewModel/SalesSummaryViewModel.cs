using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class SalesSummaryViewModel
    {
        public int transId { get; set; }
        public string accountname { get; set; }
        public DateTime dateTrans { get; set; }
        public string reference { get; set; }
        public string particular { get; set; }
        public decimal CashSales { get; set; }
        public decimal OnAccount { get; set; }



        public IEnumerable<SalesSummaryViewModel> GetSalesSummary()
        {
            List<SalesSummaryViewModel> listofSales=new List<SalesSummaryViewModel>();
            TransRecievablesViewModel t=new TransRecievablesViewModel();
            TransactionDetailsViewModel transdetails = new TransactionDetailsViewModel();
            PegasusEntities db_entities=new PegasusEntities();
            Func<int, decimal> _getTotalPaymentByBooking = TransactionDetailsViewModel.GetTotalPaymentByTrans;

            try
            {

                var bookings = (from booking in db_entities.Bookings where booking.is_cancelled == false && booking.is_deleted == false select booking).OrderBy(x => x.Customer.lastname).ToList();
                listofSales = (from b in bookings join p in db_entities.Payments on b.trn_Id equals p.trn_Id
                    select new SalesSummaryViewModel()
                    {
                        transId = b.trn_Id,
                        accountname = Utilities.Getfullname(b.Customer.lastname, b.Customer.firstname,b.Customer.middle),
                        dateTrans =Convert.ToDateTime(p.dateofPayment),
                        reference = p.particular,
                        particular = b.Package.p_descripton,
                        CashSales =Convert.ToDecimal(p.amtPay),
                        OnAccount =transdetails.GetTotalBookingAmount(b.trn_Id)- _getTotalPaymentByBooking(b.trn_Id)

                    }).ToList();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return listofSales.ToList();
        }
    }


}