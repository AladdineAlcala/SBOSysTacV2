using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class BookingRefundViewModel
    {
        public int? rfId { get; set; }
        public DateTime rfDate { get; set; }
        public int transId { get; set; }
        [Required(ErrorMessage = "Reason for creating refund required")]
        public string refundReason { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}")]
        public decimal refundAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}")]
        public decimal refundDeduction { get; set; }
        [DisplayFormat(DataFormatString = "{0:N}")]
        public decimal refundNet { get; set; }
        public bool refStatus { get; set; }

        public BookingRefundViewModel GetBookingRefund(int transId)
        {
            var dbentities=new PegasusEntities();
            var list = (from r in dbentities.Refunds select r).Where(t => t.trn_Id == transId);

            var bookingrefund =list.Select( r=>new BookingRefundViewModel()
                {
                    rfId = (int?) r.Rf_id,
                    rfDate = (DateTime) r.rfDate,
                    transId = (int) r.trn_Id,
                    refundAmount = (decimal) r.rf_Amount,
                    refundDeduction = (decimal) r.rfDeduction,
                    refundNet = (decimal) r.rfNetAmount,
                    refundReason = r.rf_Reason,
                    refStatus = r.rf_Stat==1?true:false
                }).FirstOrDefault();

            return bookingrefund;
        }

    }
}