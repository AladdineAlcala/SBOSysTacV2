using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class AddonsViewModel
    {
        public int bookaddonNo { get; set; }
        public int TransId { get; set; }
        [Display(Name = "Add-on Description :")]
        [Required(ErrorMessage = "Add on information required")]
        public string AddonsDescription { get; set; }
        [Display(Name = "Note :")]
        public string Unit { get; set; }
        public string AddonNote { get; set; }
        [Display(Name = "Add-on Amount :")]
        public decimal AddonAmount { get; set; }
        [Required(ErrorMessage = "Addon Category required")]
        public int addoncatId { get; set; }
        public string addoncategory { get; set; }
        public int? addonId { get; set; }
        [Display(Name = "Dept-Incharge :")]
        [Required(ErrorMessage = "Department required")]
        public int deptId { get; set; }
        public string deptname { get; set; }
        public bool isSelected { get; set; } = false;

        public IEnumerable<SelectListItem>  addonscatselectlist { get; set; }
        public IEnumerable<SelectListItem> deptincharge_list { get; set; }


        public static Func<decimal, decimal, decimal> calcAddons=(qty,amt) => qty * amt;


        public IEnumerable<AddonsViewModel> ListofAddons()
        {
            List<AddonsViewModel> list=new List<AddonsViewModel>();
            try
            {
                using (var dbcontext=new PegasusEntities())
                {
                    list = (from bal in dbcontext.BookingAddons
                        select new
                        {
                            _bookaddonNo = bal.bookaddonNo,
                            _TransId = (int)bal.trn_Id,
                            _AddonsDescription = bal.Addondesc,

                            _Addons = bal.BookAddonsDetails.Select(g => new { g.addonId, tota = g.amount * g.qty }),
                            _AddonNote = bal.Note
                        }).Select(p => new AddonsViewModel()
                    {

                        bookaddonNo = p._bookaddonNo,
                        TransId = p._TransId,
                        AddonsDescription = p._AddonsDescription,
                        AddonNote = p._AddonNote,
                        AddonAmount = p._Addons.Sum(t => t.tota) != null ? (decimal)p._Addons.Sum(t => t.tota) : 0


                    }).ToList();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return list;
        }


        public AddonsViewModel GetAddonsViewModelbyitemNo(int itemNo)
        {
            var dbcontext = new PegasusEntities();
            var modifiedaddons = dbcontext.BookingAddons.Find(itemNo);

            var addons = new AddonsViewModel
            {
                bookaddonNo = modifiedaddons.bookaddonNo,
                TransId =Convert.ToInt32(modifiedaddons.trn_Id),
                //addonId = modifiedaddons.addonId,
                AddonsDescription = modifiedaddons.Addondesc,
                //AddonAmount =Convert.ToDecimal(modifiedaddons.AddonAmount),
                AddonNote = modifiedaddons.Note
            };

            dbcontext.Dispose();
            return addons;
        }


        public IEnumerable<SelectListItem> GetSelectListAddonCat()
        {
            using (var dbcontext = new PegasusEntities())
            {
                var addoncatselectlist = dbcontext.AddonCategories.AsEnumerable().Select(x => new SelectListItem()
                {
                    Value = x.addoncatId.ToString(),
                    Text = x.addoncatdesc

                }).ToList();

                return new SelectList(addoncatselectlist, "Value", "Text");
            }
            
           
        }


        public IEnumerable<AddonsViewModel> GetListofAddonDetails()
        {
            var dbcontext = new PegasusEntities();

            List<AddonsViewModel> list=new List<AddonsViewModel>();

            list = (from a in dbcontext.AddonDetails join b in dbcontext.AddonCategories on a.addoncatId equals b.addoncatId 
                select new AddonsViewModel()
                {
                    addonId = a.addonId,
                    addoncatId = (int) a.addoncatId,
                    addoncategory = b.addoncatdesc,
                    AddonsDescription = a.addondescription,
                    AddonAmount = (decimal) a.amount,
                    Unit = a.unit

                }).ToList();

            dbcontext.Dispose();

            return list;
        }


        public static decimal AddonsTotal(IEnumerable<ICollection<BookAddonsDetail>> bookAddonsDetails)
        {
        
            decimal addonsTotal = 0;

            
            try
            {
                var addonsList = bookAddonsDetails.ToList();

                if (addonsList.Count != 0)
                {
                    foreach (var addons in addonsList.Select(addon => addon.Select(t => calcAddons((decimal)t.qty, (decimal)t.amount))))
                    {
                        addonsTotal += addons.Sum();
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return addonsTotal;
        }


        public static decimal AddonsTotalByaddonId(IEnumerable<ICollection<BookAddonsDetail>> bookAddonsDetails)
        {

            decimal addonsTotal = 0;


            try
            {
                var addonsList = bookAddonsDetails.ToList();

                if (addonsList.Count != 0)
                {
                    foreach (var addons in addonsList.Select(addon => addon.Select(t => calcAddons((decimal)t.qty, (decimal)t.amount))))
                    {
                        addonsTotal += addons.Sum();
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return addonsTotal;
        }
    }
}