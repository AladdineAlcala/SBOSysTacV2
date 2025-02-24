﻿using System;
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
        public string p_type { get; set; }
        public string Venue { get; set; }
        public int noofPax { get; set; }
        public decimal PackageRate { get; set; }
        public string Addons { get; set; }
        public decimal AddonsTotal { get; set; }
        public string PaymentMode { get; set; }
        public decimal AmountPaid { get; set; }
        public string Status { get; set; }
        public bool iscancelled { get; set; }
        public bool isDeletedTran { get; set; }
        public decimal bookOtherCharge { get; set; }
        public decimal transpoCharge { get; set; }


        //public CateringReportViewModel()
        //{
        //    transactionDetails = new TransactionDetailsViewModel();
        //}
        //public static IEnumerable<CateringReportViewModel> GetCateringReport(IEnumerable<Booking> bookings)
        //{
        //    Func<Booking, List<ICollection<BookAddonsDetail>>> _getAddonDetails = BookingAddonDetailsViewModel.GetAddonDetails;

        //        return bookings.Select(x => new CateringReportViewModel()
        //        {

        //            EventDate = (DateTime) x.startdate,
        //            cId = x.Customer.c_Id,
        //            Client = Utilities.Getfullname(x.Customer.lastname,x.Customer.firstname,x.Customer.middle),
        //            Occasion = x.occasion,
        //            p_type = x.Package.p_type.TrimEnd()==PackageEnum.packageType.vip.ToString() || x.Package.p_type.TrimEnd() == PackageEnum.packageType.regular.ToString() ?"cat" : x.Package.p_type.TrimEnd(),
        //            Venue = x.venue,
        //            noofPax = (int)x.noofperson,
        //            PackageRate=x.Package.p_type.Trim()!="vip"? (decimal)x.Package.p_amountPax - TransactionDetailsViewModel.GetCateringdiscountByPax((int)x.noofperson): (decimal)x.Package.p_amountPax,
        //            Addons = x.BookingAddons.Any() ? string.Join(", ",x.BookingAddons.Select(t=>t.Addondesc)):String.Empty,
        //            AddonsTotal = x.BookingAddons.Any() ? AddonsViewModel.AddonsTotal(_getAddonDetails(x)) :0,
        //            AmountPaid = x.Payments.Any() ? x.Payments.Select(t => Convert.ToDecimal(t.amtPay)).Sum() : 0,
        //            PaymentMode = x.Payments.Any()? x.Payments.Select(t => t.pay_means).ToList().Aggregate((i,j)=> i + "," + j!=i?j:""):"---",
        //            Status = x.Payments.Any()?"pd":"unpd",
        //            iscancelled = (bool) x.is_cancelled,
        //            isDeletedTran = (bool) x.is_deleted


        //        }).Where(t=>t.iscancelled==false && t.isDeletedTran==false && t.p_type==PackageEnum.packageType.cat.ToString() || t.p_type==PackageEnum.packageType.pm.ToString()).OrderBy(o=>o.p_type)
        //        .ToList();
        //}


    }
}