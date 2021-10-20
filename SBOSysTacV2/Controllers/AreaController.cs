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
    public class AreaController : Controller
    {
        private PegasusEntities _dbcontext;
        private AreaDetailsViewModel a_details=new AreaDetailsViewModel();

        public AreaController()
        {
            _dbcontext=new PegasusEntities();    
        }

        // GET: Area
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LoadListofArea()
        {
            var areadetails = a_details.GetAreaDetailsList();

            return Json(new {data=areadetails }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CreateArea()
        {

            return PartialView("_create_areaPartialView");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateArea(AreaDetailsViewModel new_Area)
        {

            if (!ModelState.IsValid) return PartialView("_create_areaPartialView", new_Area);

            var recordexist =
                _dbcontext.Areas.Any(x => x.AreaDetails.ToLower().Contains(new_Area.areaDetails.ToLower()));

            if (recordexist)
            {
                return Json(new { success = false, message = new_Area.areaDetails + "is already in the list" },
                    JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {

                    var _area = new_Area.areaDetails.ToUpper();

                    Area newArea = new Area()
                    {
                        aID = Convert.ToInt32(new_Area.areaId),
                        AreaDetails = _area
                    };

                    _dbcontext.Areas.Add(newArea);
                    _dbcontext.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }


            }

            

            return Json(new {success=true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveArea(int areaId)
        {
            Area deletedArea=new Area();
            deletedArea = _dbcontext.Areas.Find(areaId);


            //check area id if exist in package area coverage Table
            var has_existingRecord = _dbcontext.PackageAreaCoverages.Any(x => x.aID.Value.Equals(areaId));

            if (has_existingRecord)
            {

                return Json(new
                {
                    success = false,
                    message = deletedArea.AreaDetails + " exist in Package Area Coverage Table.."
                },JsonRequestBehavior.AllowGet);

            }
            else
            {

                if (deletedArea != null)
                {
                    _dbcontext.Areas.Remove(deletedArea);
                    _dbcontext.SaveChanges();
                }
               
            }
           



            return Json(new {success = true}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ModifyArea(int areaId)
        {

            Area modifyArea=new Area();
            AreaDetailsViewModel areadetails = new AreaDetailsViewModel();

            modifyArea = _dbcontext.Areas.Find(areaId);

            if (modifyArea != null)
            {

               areadetails = new AreaDetailsViewModel()
                {
                    areaId = modifyArea.aID,
                    areaDetails = modifyArea.AreaDetails
                };

            }
          

            return PartialView("_modifyAreaPartialView", areadetails);
        }


        //Update Area
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifyArea(AreaDetailsViewModel modify_Area)
        {

            if (!ModelState.IsValid) return PartialView("_modifyAreaPartialView", modify_Area);

            var recordexist =
                _dbcontext.Areas.Any(x => x.AreaDetails.ToLower().Contains(modify_Area.areaDetails.ToLower()));

            if (recordexist)
            {
                return Json(new { success = false, message = modify_Area.areaDetails + "is already in the list" },
                    JsonRequestBehavior.AllowGet);
            }
            else
            {
                try
                {

                    var _area = modify_Area.areaDetails.ToUpper();

                    Area modifyArea = new Area()
                    {
                        aID = Convert.ToInt32(modify_Area.areaId),
                        AreaDetails = _area
                    };

                    _dbcontext.Areas.Attach(modifyArea);
                    _dbcontext.Entry(modifyArea).State = EntityState.Modified;
                    _dbcontext.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }


            }



            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }



        protected override void Dispose(bool disposing)
        {
            _dbcontext.Dispose();
        }

    }
}