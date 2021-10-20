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

        public int No { get; set; }
        public int bookingNo { get; set; }
        public int  addonId{ get; set; }
        [Display(Name = "Add-on Description :")]
        public string addondetails { get; set; }
        [Display(Name = "Unit:")]
        public string unit { get; set; }
        [Display(Name = "Amount :")]
        public decimal amount { get; set; }
        [Display(Name = "Order Qty :")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
        [Required(ErrorMessage = "Add on information required")]
        public decimal orderQty  { get; set; }

        public IEnumerable<SelectedAddonsViewModel> GetSelectedAddons()
        {
            var dbconext=new PegasusEntities();

            var list = (from b in dbconext.BookingAddons select new SelectedAddonsViewModel()
            {
                bookingNo = (int) b.trn_Id,
                addonId = (int) b.addonId,
                addondetails = b.Addondesc
            });

            return list;
        }

        public SelectedAddonsViewModel GetSelectedAddons(int addonNo)
        {
            var dbconext = new PegasusEntities();

            var bookaddons = (from b in dbconext.BookingAddons
                join d in dbconext.AddonDetails on b.addonId equals d.addonId
                where b.No == addonNo select new SelectedAddonsViewModel()
                {
                    No = b.No,
                    bookingNo=(int) b.trn_Id,
                    addonId = (int) b.addonId,
                    addondetails=d.addondescription,
                    unit = d.unit,
                    amount = (decimal) d.amount,
                    orderQty = (decimal) b.addonQty,

                }).FirstOrDefault();



            return bookaddons;
        }
    }
}