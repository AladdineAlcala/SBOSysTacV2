using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SBOSysTacV2.ViewModel
{
    public class BookOtherChargeDetailViewModel
    {
        public int chargeId { get; set; }
        public int transId { get; set; }
        [Required(ErrorMessage = "Reference Required ")]
        public string chargeDesc { get; set; }
        public string chargeNote { get; set; }
  
        [DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Payment Amount Required")]
        [RegularExpression(@"^\d+\.\d{0,2}$", ErrorMessage = "Invalid Amount Format; Max. Two Decimal Points.")]
        [Range(1, 9999999999999999.99, ErrorMessage = "Amount Entered Is Not in Range or cannot accept 0 values")]
        public decimal chargeAmount { get; set; }
        public DateTime chargeCreatedDate { get; set; }=DateTime.UtcNow;
        public DateTime chargeUpdatedDate { get; set; }=DateTime.UtcNow;

    }
}