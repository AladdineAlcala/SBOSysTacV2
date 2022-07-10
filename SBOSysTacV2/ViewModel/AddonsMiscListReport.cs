using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace SBOSysTacV2.ViewModel
{
    public class AddonsMiscListReport
    {
        public int transId { get; set; }
        public string transType  { get; set; }
        public string description { get; set; }
        public decimal amount { get; set; }
    }
}