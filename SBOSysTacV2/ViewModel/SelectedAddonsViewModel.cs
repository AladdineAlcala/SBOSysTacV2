using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class SelectedAddonsViewModel
    {

        public int addondetail_No { get; set; }
        public int booking_No { get; set; }
        public int bookaddon_No { get; set; }
        public int  addon_Id{ get; set; }
        [Display(Name = "Add-on Description :")]
        public string addon_details { get; set; }
        [Display(Name = "Unit:")]
        public string unit { get; set; }
        [Display(Name = "Amount :")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Range(0, 9999999999999999.99)]
        public decimal amount { get; set; }
        [Display(Name = "Order Qty :")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        [Required(ErrorMessage = "Add on information required")]
        public decimal orderQty  { get; set; }

        //public IEnumerable<SelectedAddonsViewModel> GetSelectedAddons()
        //{
        //    var dbconext=new PegasusEntities();

        //    var list = (from b in dbconext.BookingAddons select new SelectedAddonsViewModel()
        //    {
        //        bookingNo = (int) b.trn_Id,
        //        addonId = (int) b.addonId,
        //        addondetails = b.Addondesc
        //    });

        //    return list;
        //}

        //public SelectedAddonsViewModel GetSelectedAddons(int addonNo)
        //{
        //    var dbconext = new PegasusEntities();

        //    var bookaddons = (from b in dbconext.BookingAddons
        //        join d in dbconext.AddonDetails on b.addonId equals d.addonId
        //        where b.bookaddonNo == addonNo select new SelectedAddonsViewModel()
        //        {
        //            No = b.bookaddonNo,
        //            bookingNo=(int) b.trn_Id,
        //            addonId = (int) b.addonId,
        //            addondetails=d.addondescription,
        //            unit = d.unit,
        //            amount = (decimal) d.amount,
        //            //orderQty = (decimal) b.addonQty,

        //        }).FirstOrDefault();



        //    return bookaddons;
        //}
    }
}