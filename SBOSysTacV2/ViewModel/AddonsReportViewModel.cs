using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBOSysTacV2.ViewModel
{
    public class AddonsReportViewModel
    {
        public int transid { get; set; }
        public DateTime eventDate { get; set; }
        public int cusid { get; set; }
        public int addondetailId { get; set; }
        public int addonid { get; set; }
        public decimal book_addon_qty { get; set; }
        public decimal addonAmount { get; set; }
        public string addoncatDesc { get; set; }
        public string addoncatdescription { get; set; }
        public string p_type { get; set; }
        public string p_descripton { get; set; }
        public decimal p_amountPax { get; set; }
        public string lastname { get; set; }
        public string firstname { get; set; }
        public string middle { get; set; }


    }
}