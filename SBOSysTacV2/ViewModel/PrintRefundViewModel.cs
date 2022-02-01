using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class PrintRefundViewModel
    {
        public long Rf_id { get; set; }
        public int trn_Id { get; set; }
        public string rf_Reason { get; set; }
        public decimal rf_Amount { get; set; }
        public decimal rfDeduction { get; set; }
        public decimal rfNetAmount { get; set; }
        public int rf_Stat { get; set; }
        public DateTime rfDate { get; set; }

        PegasusEntities dbEntities=new PegasusEntities();

        public PrintRefundViewModel PrintRefundDetails(int rfId)
        {
            return dbEntities.Refunds.Select(t => new PrintRefundViewModel()
            {
                Rf_id = (int) t.Rf_id,
                trn_Id = (int) t.trn_Id,
                rf_Reason = t.rf_Reason,
                rf_Amount = (decimal) t.rf_Amount,
                rfDeduction = (decimal) t.rfDeduction,
                rfNetAmount = (decimal) t.rfNetAmount,
                rf_Stat = (int) t.rf_Stat,
                rfDate = (DateTime) t.rfDate



            }).FirstOrDefault(t=>t.Rf_id==rfId);
        }
    }
}