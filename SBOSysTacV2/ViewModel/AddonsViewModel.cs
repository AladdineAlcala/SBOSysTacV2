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
        public int No { get; set; }
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
        public IEnumerable<SelectListItem>  addonscatselectlist { get; set; }
        public IEnumerable<SelectListItem> deptincharge_list { get; set; }


        public IEnumerable<AddonsViewModel> ListofAddons()
        {
            List<AddonsViewModel> list=new List<AddonsViewModel>();
            var _dbentities = new PegasusEntities();
            try
            {
                list = (from a in _dbentities.BookingAddons
                    select new AddonsViewModel
                    {
                        No = a.No,
                        TransId = (int) a.trn_Id,
                        AddonsDescription = a.Addondesc,
                        AddonAmount = (decimal) a.AddonAmount,
                        AddonNote = a.Note
                    }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return list.ToList();
        }


        public AddonsViewModel GetAddonsViewModelbyitemNo(int itemNo)
        {
            var dbentites=new PegasusEntities();
            var modifiedaddons =dbentites.BookingAddons.Find(itemNo);

            var addons = new AddonsViewModel
            {
                No = modifiedaddons.No,
                TransId =Convert.ToInt32(modifiedaddons.trn_Id),
                addonId = modifiedaddons.addonId,
                AddonsDescription = modifiedaddons.Addondesc,
                AddonAmount =Convert.ToDecimal(modifiedaddons.AddonAmount),
                AddonNote = modifiedaddons.Note
            };

            return addons;
        }


        public IEnumerable<SelectListItem> GetSelectListAddonCat()
        {
            var dbcontext=new PegasusEntities();

            var addoncatselectlist = dbcontext.AddonCategories.AsEnumerable().Select(x => new SelectListItem()
            {
                Value = x.addoncatId.ToString(),
                Text = x.addoncatdesc
                
            });
            return new SelectList(addoncatselectlist, "Value", "Text");
        }


        public IEnumerable<AddonsViewModel> GetListofAddonDetails()
        {
            var dbcontext = new PegasusEntities();

            List<AddonsViewModel> list=new List<AddonsViewModel>();

            list = (from a in dbcontext.AddonDetails join b in dbcontext.AddonCategories on a.addoncatId equals b.addoncatId 
                select new AddonsViewModel()
                {
                    No=a.addonId,
                    addoncatId = (int) a.addoncatId,
                    addoncategory = b.addoncatdesc,
                    AddonsDescription = a.addondescription,
                    AddonAmount = (decimal) a.amount,
                    Unit = a.unit

                }).ToList();

            dbcontext.Dispose();

            return list;
        }

       
    }
}