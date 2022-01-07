using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class BookingOtherChargeViewModel
    {
        public int transId { get; set; }

        public List<BookOtherChargeDetailViewModel> BookOtherChargeList{ get; set; }

        private PegasusEntities _dbEntities;
        public BookingOtherChargeViewModel()
        {
            _dbEntities = new PegasusEntities();
        }

        public decimal GetTotalOtherCharges(int transactionId)
        {

         var bookCharge= _dbEntities.Book_OtherCharge.Find(transactionId);

              if (bookCharge!=null)
              {
                  return (decimal)_dbEntities.Book_OtherCharge
                      .Where(t => t.trn_Id == transactionId)
                      .Select(s => s.amount).Sum();
              }
              else
              {
                  return 0;
              }
           
        }


        
    }
}