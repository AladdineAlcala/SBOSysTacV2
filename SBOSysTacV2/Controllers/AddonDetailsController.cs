using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.Models;
using SBOSysTacV2.ViewModel;

namespace SBOSysTacV2.Controllers
{
    public class AddonDetailsController : Controller
    {

        private PegasusEntities dbEntities;
        private AddonsViewModel addonsviewmodel=new AddonsViewModel();
        private DepartmentViewModel dept=new DepartmentViewModel();

        public AddonDetailsController()
        {
            dbEntities = new PegasusEntities();
        }

        public ActionResult LoadAdddonDetails()
        {

            var datalist = addonsviewmodel.GetListofAddonDetails().ToList();

            return Json(new {data=datalist }, JsonRequestBehavior.AllowGet);
        }


        // GET: AddOns
        public ActionResult Index()
        {
            return View();
        }


    
        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin, UserPermessionLevelEnum.user)]
        [HttpGet]
        public ActionResult CreateAddons()
        {
            var addonsvm = new AddonsViewModel()
            {
                deptincharge_list = dept.Get_MenuDepartmentInchargeListItems(),
                addonscatselectlist = addonsviewmodel.GetSelectListAddonCat()
            };

            return PartialView("CreateAddon", addonsvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAddons(AddonsViewModel newAddons)
        {
            var success = false;
            var message = "";
            if (!ModelState.IsValid)
            {
                return PartialView("CreateAddon", newAddons);
            }

            try
            {

                AddonDetail newaddondetail = new AddonDetail()
                {
                    addoncatId = newAddons.addoncatId,
                    addondescription = newAddons.AddonsDescription,
                    deptId = newAddons.deptId,
                    unit = newAddons.Unit,
                    amount = newAddons.AddonAmount
                };

                dbEntities.AddonDetails.Add(newaddondetail);
                dbEntities.SaveChanges();

                success = true;
            }
            catch (Exception e)
            {
                message = e.Message.ToString();
                success = false;
            }

            return Json(new {success=success,message=message }, JsonRequestBehavior.AllowGet);
        }

        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin)]
        [HttpPost]
        public ActionResult RemoveAddondetail(int addonId)
        {
            var success = false;

            try
            {
                var addondetails = dbEntities.AddonDetails.Find(addonId);

                if (addondetails != null)
                {
                    dbEntities.AddonDetails.Remove(addondetails);
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

        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin)]
        [HttpGet]
        public ActionResult ModifyAddonDetails(int addonId)
        {
            var addondetails = dbEntities.AddonDetails.AsNoTracking().FirstOrDefault(x => x.addonId == addonId);

            AddonsViewModel addonsdetailviewmodel=new AddonsViewModel();
            if (addondetails!=null)
            {

                addonsdetailviewmodel = new AddonsViewModel
                {
                    addonId = addondetails.addonId,
                    addoncatId = (int) addondetails.addoncatId,
                    addoncategory = addondetails.AddonCategory.addoncatdesc,
                    AddonsDescription = addondetails.addondescription,
                    //deptId = addondetails.deptId!=0 ? Convert.ToInt32(addondetails.deptId) : 0,
                    deptId = Convert.ToInt32(addondetails.deptId),
                    deptname = addondetails.AddonCategory.addoncatdesc,
                    Unit = addondetails.unit,
                    AddonAmount = (decimal) addondetails.amount,

                    addonscatselectlist = addonsviewmodel.GetSelectListAddonCat(),
                    deptincharge_list = dept.Get_MenuDepartmentInchargeListItems()
                };


            }


            return PartialView(addonsdetailviewmodel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifyAddonDetails(AddonsViewModel modifyaddonsdetails)
        {

            var success = false;
            var message = "";
            if (!ModelState.IsValid)
            {
                return PartialView("ModifyAddonDetails", modifyaddonsdetails);
            }


            try
            {

                AddonDetail addondetail = new AddonDetail()
                {
                    addonId = (int) modifyaddonsdetails.addonId,
                    addoncatId = modifyaddonsdetails.addoncatId,
                    deptId = modifyaddonsdetails.deptId,
                    addondescription = modifyaddonsdetails.AddonsDescription,
                    unit = modifyaddonsdetails.Unit,
                    amount = modifyaddonsdetails.AddonAmount
                };

                dbEntities.AddonDetails.Attach(addondetail);
                dbEntities.Entry(addondetail).State=EntityState.Modified;
                dbEntities.SaveChanges();

                success = true;
            }
            catch (Exception e)
            {
                message = e.Message.ToString();
                success = false;
            }



            return Json(new {success=success}, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
           dbEntities.Dispose();
        }
    }
}