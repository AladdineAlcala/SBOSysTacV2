using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBOSysTacV2.ViewModel
{
    public class IncentivesReportViewModel
    {
        public DateTime EventDate { get; set; }
        public int cId { get; set; }
        public string Client { get; set; }
        public string Occasion { get; set; }
        public string p_type { get; set; }
        public string Venue { get; set; }
        public int noofPax { get; set; }
        public decimal PackageRate { get; set; }
        public string Addons { get; set; }
        public decimal AddonsTotal { get; set; }
        public string PaymentMode { get; set; }
        public DateTime LastPaymentDate { get; set; }
        public decimal AmountPaid { get; set; }
        public bool IsFullyPaidTrans { get; set; }
        public string Status { get; set; }
        public bool iscancelled { get; set; }
        public bool isDeletedTran { get; set; }

    }
}