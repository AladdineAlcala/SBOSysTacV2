using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SBOSysTacV2.Models;
using SBOSysTacV2.ViewModel;

namespace SBOSysTacV2.Controllers
{
    public class AddonCategoryController : Controller
    {
        private  PegasusEntities dbEntities=new PegasusEntities();
        private AddonCatViewModel addoncat=new AddonCatViewModel();

        public ActionResult LoadAddonsCategoryList()
        {
            var addonscatlist = addoncat.GetListofAddonCategories();

            return Json(new {data = addonscatlist}, JsonRequestBehavior.AllowGet);
        }



        // GET: AddonCategory
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create_AddonCategory()
        {
       
            return PartialView();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_AddonCategory(AddonCatViewModel newaddoncategory)
        {
            bool success = false;

            if (!ModelState.IsValid)
            {
                return PartialView(newaddoncategory);
            }

            try
            {
                AddonCategory addoncategory = new AddonCategory()
                {
               
                    addoncatdesc = newaddoncategory.addoncatdetails

                };
                dbEntities.AddonCategories.Add(addoncategory);
                dbEntities.SaveChanges();

                success = true;
            }
            catch (Exception e)
            {
                var message = e.Message.ToString();

                success = false;
            }


            return Json(new {success= success }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult Modify_AddonCategory(int addoncatId)
        {
            AddonCatViewModel modifyaddoncat = new AddonCatViewModel();
            if (addoncatId > 0)
            {
                var addoncat = dbEntities.AddonCategories.Find(addoncatId);

                modifyaddoncat = new AddonCatViewModel()
                {
                    addoncatId = addoncat.addoncatId,
                    addoncatdetails = addoncat.addoncatdesc
                };
            }


            return PartialView(modifyaddoncat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Modify_AddonCategory(AddonCatViewModel modifiedaddoncat)
        {
            var success = false;
            var message = "";

            if (!ModelState.IsValid)
            {
                return PartialView(modifiedaddoncat);
            }

            try
            {

                var addoncat = new AddonCategory()
                {
                    addoncatId = (int) modifiedaddoncat.addoncatId,
                    addoncatdesc = modifiedaddoncat.addoncatdetails
                };


                dbEntities.AddonCategories.Attach(addoncat);
                dbEntities.Entry(addoncat).State=EntityState.Modified;
                dbEntities.SaveChanges();

                success = true;

            }
            catch (Exception e)
            {
                message = e.Message.ToString();
                success = false;
            }

            return Json(new {success=success }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RemoveAddonsCategory(int addoncatId)
        {
            var success = false;

            try
            {
                var addoncat = dbEntities.AddonCategories.Find(addoncatId);

                if (addoncat != null)
                {

                    dbEntities.AddonCategories.Remove(addoncat);
                    dbEntities.SaveChanges();

                    success = true;
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Json(new {success=success }, JsonRequestBehavior.AllowGet);
        }
    }
}

