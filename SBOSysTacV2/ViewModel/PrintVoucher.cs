using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SBOSysTacV2.ViewModel
{
    public class PrintVoucher
    {
        
        [Required(ErrorMessage = "Payment Number required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "UPRN must be numeric")]
        public string PaymentNo { get; set; }
    }
}