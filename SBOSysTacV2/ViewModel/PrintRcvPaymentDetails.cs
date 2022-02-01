using SBOSysTacV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBOSysTacV2.ViewModel
{
    public class PrintRcvPaymentDetails
    {
        public string PayNo { get; set; }
        public int transId { get; set; }
        public DateTime dateofPayment { get; set; }
        public string particular { get; set; }
        public int payType { get; set; }
        public decimal amtPay { get; set; }
        public string pay_means { get; set; }
        public string checkNo { get; set; }
        public string notes { get; set; }


        private PegasusEntities _dbcontext = new PegasusEntities();
        public IEnumerable<PrintRcvPaymentDetails> GetPaymentsList()
        {

            List<PrintRcvPaymentDetails> paymentslist = new List<PrintRcvPaymentDetails>();

            try
            {

                var payments = (from p in _dbcontext.Payments select p).ToList();

                paymentslist=(from pmt in payments
                              select new PrintRcvPaymentDetails()
                    {
                        PayNo = pmt.payNo,
                        transId =(int)pmt.trn_Id,
                        dateofPayment =Convert.ToDateTime(pmt.dateofPayment),
                        particular = pmt.particular,
                        payType =(int)pmt.payType,
                        amtPay =Convert.ToDecimal(pmt.amtPay),
                        pay_means = pmt.pay_means,
                        checkNo = pmt.checkNo,
                        notes = pmt.notes


                    }).OrderBy(x => x.dateofPayment).ToList();



            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return paymentslist.ToList();
        }

    }
}