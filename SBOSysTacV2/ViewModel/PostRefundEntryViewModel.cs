using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class PostRefundEntryViewModel
    {
        public long rfID { get; set; }
        public int transId { get; set; }
        [Required(ErrorMessage = "Reference Required ")]
        public string refundentrydetail { get; set; }
        [Required(ErrorMessage = "Payment Amount Required")]
        [RegularExpression(@"^\d+\.\d{0,2}$", ErrorMessage = "Invalid Amount Format; Max. Two Decimal Points.")]
        [Range(1, 9999999999999999.99, ErrorMessage = "Amount Entered Is Not in Range or cannot accept 0 values")]
        public decimal EntryAmount { get; set; }


      
    }
}