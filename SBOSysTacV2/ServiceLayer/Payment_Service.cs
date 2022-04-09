using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;
using SBOSysTacV2.ViewModel;

namespace SBOSysTacV2.ServiceLayer
{
    public class Payment_Service : IDisposable
    {
        private static PegasusEntities _dbEntities = new PegasusEntities();

        public static int GetTransctionIdByPayment(string paymentNo) => (int)_dbEntities.Payments.FirstOrDefault(t => t.payNo == paymentNo).trn_Id;

        public static IEnumerable<PrintRcvPaymentDetails> GetPaymentsListById(string payNo)
        {

            List<PrintRcvPaymentDetails> paymentslist = new List<PrintRcvPaymentDetails>();

            try
            {

                var payments = (from p in _dbEntities.Payments select p).Where(t => t.payNo == payNo).ToList();

                paymentslist = (from pmt in payments 
                    select new PrintRcvPaymentDetails()
                    {
                        PayNo = pmt.payNo,
                        transId = (int)pmt.trn_Id,
                        dateofPayment = Convert.ToDateTime(pmt.dateofPayment),
                        particular = pmt.particular,
                        payType = (int)pmt.payType,
                        amtPay = Convert.ToDecimal(pmt.amtPay),
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

            return paymentslist;
        }

        public static DateTime GetPaymentDate(string paymentNo) =>
            (DateTime)_dbEntities.Payments.Find(paymentNo).dateofPayment;
        public static decimal GetTotalPaymentByTransId(int transId)
        {
            var payments = _dbEntities.Payments.Where(t => t.trn_Id == transId).ToList();

            return (decimal)(payments.Count > 0 ? payments.Sum(t => t.amtPay) : 0);
        }

        public void Dispose()
        {
            _dbEntities.Dispose();
        }
    }
}