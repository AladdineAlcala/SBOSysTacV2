using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class AddonCatViewModel
    {
        public int? addoncatId { get; set; }
        [Required(ErrorMessage = "Addon Details Required")]
        public string addoncatdetails { get; set; }


        public IEnumerable<AddonCatViewModel> GetListofAddonCategories()
        {
            var dbentities=new PegasusEntities();

            List<AddonCatViewModel> list=new List<AddonCatViewModel>();
            list = (from a in dbentities.AddonCategories
                select new AddonCatViewModel()
                {
                    addoncatId = a.addoncatId,
                    addoncatdetails = a.addoncatdesc
                }).ToList();

            return list;
        }
    }
}