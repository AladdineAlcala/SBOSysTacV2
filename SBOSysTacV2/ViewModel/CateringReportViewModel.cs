using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
   

    public class CateringReportViewModel
    {
        public DateTime EventDate { get; set; }
        public int cId { get; set; }
        public string Client { get; set; }
        public string Occasion { get; set; }
        public string Venue { get; set; }
        public int noofPax { get; set; }
        public decimal PackageRate { get; set; }
        public string Addons { get; set; }
        public decimal AddonsTotal { get; set; }
        public string PaymentMode { get; set; }
        public decimal AmountPaid { get; set; }
        public string Status { get; set; }
        public bool iscancelled { get; set; }

        private readonly PegasusEntities dbEntities=new PegasusEntities();
        private readonly TransactionDetailsViewModel transactionDetails = new TransactionDetailsViewModel();

        //public IQueryable<CateringReportViewModel> GetCateringReport(IQueryable<Booking> bookings)
        //{
        //    return bookings.Select(x => new CateringReportViewModel()
        //    {
        //        EventDate = (DateTime) x.startdate,
        //        cId = x.Customer.c_Id,
        //        Client =x.Customer.lastname + " ," + x.Customer.firstname + " " + x.Customer.middle,
        //        Occasion = x.occasion,
        //        Venue = x.venue,
        //        noofPax = (int) x.noofperson,
        //        PackageRate = (decimal) x.Package.p_amountPax,
        //        Addons = x.BookingAddons.Count>1 ?"Various Add-ons" : x.BookingAddons.Select(t => t.Addondesc).FirstOrDefault(),
        //        TotalAddons = x.BookingAddons.Any() ? x.BookingAddons.Select(t=>(decimal)t.AddonAmount).Sum() : 0,
        //        AmountPaid = x.Payments.Any()?x.Payments.Select(t=>(decimal)t.amtPay).Sum():0,
        //        PaymentMode = x.Payments.Any()?x.Payments.Select(t => t.pay_means).FirstOrDefault():"Credit"
        //    });

        //}




        public IEnumerable<CateringReportViewModel> GetCateringReport(IEnumerable<Booking> bookings)
        {
            return bookings.Select(x => new CateringReportViewModel()
            {
                EventDate = (DateTime) x.startdate,
                cId = x.Customer.c_Id,
                Client = Utilities.Getfullname(x.Customer.lastname,x.Customer.firstname,x.Customer.middle),
                Occasion = x.occasion,
                Venue = x.venue,
                noofPax = (int)x.noofperson,
                PackageRate=x.Package.p_type.Trim()!="vip"? (decimal)x.Package.p_amountPax - transactionDetails.GetCateringdiscountByPax((int)x.noofperson): (decimal)x.Package.p_amountPax,
                Addons = x.BookingAddons.Any() ? string.Join(", ",x.BookingAddons.Select(t=>t.Addondesc)):String.Empty,
                AddonsTotal = x.BookingAddons.Any() ? x.BookingAddons.Select(t =>Convert.ToDecimal(t.AddonAmount)).Sum() : 0,
                AmountPaid = x.Payments.Any() ? x.Payments.Select(t => Convert.ToDecimal(t.amtPay)).Sum() : 0,
                PaymentMode = x.Payments.Any()? x.Payments.Select(t => t.pay_means).FirstOrDefault():"---",
                Status = x.Payments.Any()?"pd":"unpd",
                iscancelled = (bool) x.is_cancelled
            }).Where(t=>t.iscancelled==false);
        }

        //public decimal getPackageRate()
    }
}