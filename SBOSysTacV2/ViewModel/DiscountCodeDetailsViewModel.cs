using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class DiscountCodeDetailsViewModel
    {
        public int? disc_Id { get; set; }
        [Required(ErrorMessage = "Discount Code Required")]
        public string discCode { get; set; }
        [Required(ErrorMessage = "Discount Type Required")]
        public string disctype { get; set; }
        [Required(ErrorMessage = "Discount Amount Required")]
        public decimal? discount_amt { get; set; }
        public DateTime? discStartdate { get; set; }
        public DateTime? discEnddate { get; set; }

        public IEnumerable<DiscountCodeDetailsViewModel> getAllListofDiscounts()
        {
            PegasusEntities _dbEntities = new PegasusEntities();

            List<DiscountCodeDetailsViewModel> listdiscount=new List<DiscountCodeDetailsViewModel>();

            var discounts = (from d in _dbEntities.Discounts select d).ToList();

            var discountcodelist = (from d in discounts
                select new DiscountCodeDetailsViewModel()
                {
                   disc_Id = d.disc_Id,
                   discCode = d.discCode,
                   disctype = d.disctype,
                   discount_amt = d.discount1,
                   discStartdate = d.discStartdate,
                   discEnddate = d.discEnddate
                    
                }).ToList();

            return discountcodelist;
        }
    }
}