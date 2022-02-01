using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.Models;
using SBOSysTacV2.ViewModel;
using System.Data.Entity;

namespace SBOSysTacV2.Controllers
{
    
   [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin,UserPermessionLevelEnum.admin)]
    public class DiscountController : Controller
    {


        private PegasusEntities _dbEntities;
        private DiscountCodeDetailsViewModel dc=new DiscountCodeDetailsViewModel();
        public DiscountController()
        {
            _dbEntities=new PegasusEntities();
        }
        // GET: Discount
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateNewDiscount()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewDiscount(DiscountCodeDetailsViewModel newdiscountviewmodel)
        {
            if (!ModelState.IsValid) return PartialView("CreateNewDiscount", newdiscountviewmodel);

            try
            {
                var newdiscount = new Discount()
                {
                    discCode = newdiscountviewmodel.discCode,
                    disctype = newdiscountviewmodel.disctype,
                    discount1 = newdiscountviewmodel.discount_amt,
                    discStartdate = newdiscountviewmodel.discStartdate,
                    discEnddate = newdiscountviewmodel.discEnddate

                };

                _dbEntities.Discounts.Add(newdiscount);
                _dbEntities.SaveChanges();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Json(new {success=true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDiscountList()
        {

            List<DiscountCodeDetailsViewModel> discountslist =new List<DiscountCodeDetailsViewModel>();
            discountslist=dc.getAllListofDiscounts().ToList();

            return Json(new {data= discountslist }, JsonRequestBehavior.AllowGet);
        }

        //[UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin)]
        [HttpPost]
        public ActionResult RemoveDiscount(int discountId)
        {
            try
            {
                Discount deletedItem = _dbEntities.Discounts.Find(discountId);

                if (deletedItem != null)
                {
                    _dbEntities.Discounts.Remove(deletedItem);
                    _dbEntities.SaveChanges();
                }
              

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Json(new {success=true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ModifyDiscount(int discountId)
        {
            var modifiedDiscount = _dbEntities.Discounts.Find(discountId);

            DiscountCodeDetailsViewModel discountCodeDetailsView=new DiscountCodeDetailsViewModel();

            if (modifiedDiscount != null)
            {
                discountCodeDetailsView = new DiscountCodeDetailsViewModel()
                {
                    disc_Id = modifiedDiscount.disc_Id,
                    discCode = modifiedDiscount.discCode,
                    disctype = modifiedDiscount.disctype,
                    discount_amt = modifiedDiscount.discount1,
                    discStartdate = modifiedDiscount.discStartdate,
                    discEnddate = modifiedDiscount.discEnddate
                };
            }

            return PartialView("_modifyDiscountPartialView", discountCodeDetailsView);
        }


        [HttpPost]
        public ActionResult ModifyDiscount(DiscountCodeDetailsViewModel modifieddiscountviewmodel)
        {
            bool success = false;

            if (!ModelState.IsValid) return PartialView("_modifyDiscountPartialView", modifieddiscountviewmodel);

            try
            {
                var discount = new Discount()
                {
                    disc_Id =Convert.ToInt32(modifieddiscountviewmodel.disc_Id),
                    discCode = modifieddiscountviewmodel.discCode,
                    disctype = modifieddiscountviewmodel.disctype,
                    discount1 = modifieddiscountviewmodel.discount_amt,
                    discStartdate = modifieddiscountviewmodel.discStartdate,
                    discEnddate = modifieddiscountviewmodel.discEnddate

                };

                _dbEntities.Discounts.Attach(discount);
                _dbEntities.Entry(discount).State = EntityState.Modified;
                _dbEntities.SaveChanges();

                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            _dbEntities.Dispose();
        }
    }
}