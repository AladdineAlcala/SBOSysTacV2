using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;
using System.ComponentModel.DataAnnotations;

namespace SBOSysTacV2.ViewModel
{
    public class PaymentsViewModel
    {
        public string PayNo { get; set; }
        public int? transId { get; set; }
        public DateTime? dateofPayment { get; set; }
        [Display(Name = "Particular:")]
        [Required(ErrorMessage = "Reference Required ")]
        public string particular { get; set; }
        [Display(Name = "Payment Type:")]
        [Required(ErrorMessage = "Payment Type Required")]
        public int? payType { get; set; }

        [Required(ErrorMessage = "Payment Amount Required")]
        [RegularExpression(@"^\d+\.\d{0,2}$", ErrorMessage = "Invalid Amount Format; Max. Two Decimal Points.")]
        [Range(1, 9999999999999999.99,ErrorMessage = "Amount Entered Is Not in Range or cannot accept 0 values")]
        public decimal? amtPay { get; set; }
        [Display(Name = "Payment Means:")]
        [Required(ErrorMessage = "Payment Means Required")]
        public string pay_means { get; set; }
        [Display(Name = "Check No:")]
        public string checkNo { get; set; }
        [Display(Name = "Notes:")]
        public string notes { get; set; }

        public string createdByUserId { get; set; }
        public string createdByUserName { get; set; }
        public DateTime p_createdDate { get; set; }=DateTime.UtcNow;
        public DateTime p_updateDate { get; set; } = DateTime.UtcNow;
       

       

        public IEnumerable<PaymentsViewModel> GetPaymentsList()
        {
           var _dbcontext = new PegasusEntities();
            List<PaymentsViewModel> paymentslist = new List<PaymentsViewModel>();

            try
            {

                paymentslist = (from p in _dbcontext.Payments
             
                    select new PaymentsViewModel()
                    {
                        PayNo = p.payNo,
                        transId = p.trn_Id,
                        dateofPayment = p.dateofPayment,
                        particular = p.particular,
                        payType = p.payType,
                        amtPay = p.amtPay,
                        pay_means = p.pay_means,
                        checkNo = p.checkNo,
                        notes = p.notes,
                        p_createdDate = (DateTime)p.p_createdDate,
                        p_updateDate = (DateTime)p.p_updatedDate

                    }).OrderBy(x=>x.dateofPayment).ToList();


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            _dbcontext.Dispose();

            return paymentslist.ToList();
        }


        public IEnumerable<PaymentsViewModel> GetPaymentsListByClient(int transactionId)
        {
            var _dbcontext = new PegasusEntities();
            var paymentslist = new List<PaymentsViewModel>();

            try
            {

                paymentslist = (from p in _dbcontext.Payments where p.trn_Id==transactionId

                                select new PaymentsViewModel()
                                {
                                    PayNo = p.payNo,
                                    transId = p.trn_Id,
                                    dateofPayment = p.dateofPayment,
                                    particular = p.particular,
                                    payType = p.payType,
                                    amtPay = p.amtPay,
                                    pay_means = p.pay_means,
                                    checkNo = p.checkNo,
                                    notes = p.notes


                                }).OrderBy(x => x.dateofPayment).ToList();



            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            _dbcontext.Dispose();

            return paymentslist.ToList();
        }

    }
}
