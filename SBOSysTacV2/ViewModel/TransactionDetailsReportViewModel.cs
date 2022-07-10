using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBOSysTacV2.ViewModel
{
    public class TransactionDetailsReportViewModel
    {
        public int transactionId { get; set; }
        public decimal book_discounts { get; set; } = 0;
        public decimal BookOtherCharges { get; set; } = 0;
        public decimal PackageAmount { get; set; } = 0;
        public decimal TotaAddons { get; set; } = 0;
        public decimal TotaMiscCharge { get; set; } = 0;
        public decimal TotaBelowMinPax { get; set; } = 0;
        public decimal TotaDp { get; set; } = 0;
        public decimal extLocAmount { get; set; } = 0;
        public decimal cateringdiscount { get; set; } = 0;
        public decimal fullpaymnt { get; set; } = 0;

    }
}