using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SBOSysTacV2.Models;
using System.ComponentModel.DataAnnotations;

namespace SBOSysTacV2.ViewModel
{
    public class DiscountCodeViewModel
    {
        public int transId { get; set; }
        [Required(ErrorMessage = "Discount Code Required")]
        public int discountCode { get; set; }
        public decimal discountAmount { get; set; }
        public string discountType { get; set; }
        public DateTime discountdatefrom { get; set; }


        public IEnumerable<SelectListItem> DiscountSelectlist { get; set; }

        public IEnumerable<SelectListItem> GetListofDiscount()
        {
            using (var dbentities=new PegasusEntities())
            {
                var listofdiscode =dbentities.Discounts.AsEnumerable().Select(x => new SelectListItem
                {
                    Value = x.disc_Id.ToString(),
                    Text = x.discCode
                }).ToList();


                return new SelectList(listofdiscode, "Value", "Text");

            }
            

        }

    }
}