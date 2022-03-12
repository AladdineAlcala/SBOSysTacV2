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


        

    }
}