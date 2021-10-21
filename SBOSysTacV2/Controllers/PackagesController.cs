using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using SBOSysTacV2.Models;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.ViewModel;
using PagedList;

namespace SBOSysTacV2.Controllers
{
    public class PackagesController : Controller
    {
      
        private PegasusEntities _dbcontext;
        private PackageDetailsLocationViewModel packagesIndex=new PackageDetailsLocationViewModel();
        private PackageAreaLocationViewModel packageArea=new PackageAreaLocationViewModel();
        private AreaPackageViewModel areaPackage= new AreaPackageViewModel();
        private PackageBookingViewModel pb=new PackageBookingViewModel();
        private PackageViewModel pviewmodel=new PackageViewModel();
        private PackageBodyViewModel pbody=new PackageBodyViewModel();


        public PackagesController()
        {
            ViewBag.ActiveForm= Utilities.ActiveForm;
            _dbcontext =new PegasusEntities();
        }

        // GET: Packages
        public ActionResult Index(string currentFilter, string searchString,string packagetype,int?page)
        {

            var packageIndexDetails=new List<PackageDetailsLocationViewModel>();

            ViewBag.FormTitle = "Package Buffet Details";

            if (searchString!=null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;

            }

            ViewBag.CurrentFilter = searchString;



            if (!String.IsNullOrEmpty(searchString))
            {

                //packageIndexDetails =
                //    packagesIndex.GetPackageDetailsView()
                //        .Where(loc =>loc.PackageAreaDetails.Any(area => area.pAreaDetail.ToLower()
                //            .Contains(searchString.ToLower()))

                //            ).ToList();


                packageIndexDetails =packagesIndex.GetPackageDetailsView()
                                        .Where(loc =>
                                        {
                                            return loc.Packages.p_amountPax != null 
                                                        && ((loc.PackageAreaDetails.Any(area => area.pAreaDetail.ToLower()
                                                      .Contains(searchString.ToLower()))) || (loc.Packages.p_amountPax.Value.ToString(CultureInfo.InvariantCulture).Contains(searchString)));
                                        }).ToList();

            }
            else
            {
                if (packagetype!= "all" || string.IsNullOrEmpty(packagetype))
                {
                    packageIndexDetails = !string.IsNullOrEmpty(packagetype)
                        ? packagesIndex.GetPackageDetailsView()
                            .Where(ptype => ptype.Packages.p_type.ToLower().Trim() == packagetype.ToLower().Trim())
                            .ToList()
                        : packagesIndex.GetPackageDetailsView().ToList();
                }
                else
                {
                    packageIndexDetails = packagesIndex.GetPackageDetailsView().ToList();
                }

            }

            int pageSize = 4;
            int pageNumber = (page ?? 1);


            if (Request.IsAjaxRequest())
            {
                return PartialView("_packageListPartialView", packageIndexDetails.ToList().ToPagedList(pageNumber,pageSize) as PagedList<PackageDetailsLocationViewModel>);
            }

            return View(packageIndexDetails.ToPagedList(pageNumber,pageSize)as PagedList<PackageDetailsLocationViewModel>);
        }


        public ActionResult CreatePackage()
        {
            Utilities.ActiveForm = " ";
            ViewBag.FormTitle = "Create New Buffet Package";
            
            return View();
        }

        [HttpGet,ChildActionOnly]
        public ActionResult PackageForm()
        {
            Utilities.ActiveForm = " ";
            ViewBag.ActiveForm = Utilities.ActiveForm;

            var packagevm = new PackageViewModel()
            {
                packageNoPax_listitem = pviewmodel.GetPackageNoofPaxListItems()
            };

            return PartialView("_packages", packagevm);
        }

        [HttpGet,ChildActionOnly]
        public ActionResult PackageBody()
        {
            ViewBag.ActiveForm = Utilities.ActiveForm;

            return PartialView("_packagebody", Utilities.PackageBodyModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ProcessNewPackage")]
        public ActionResult CreatePackage(PackageViewModel newpackage)
        {
            if (!ModelState.IsValid)
            {

                newpackage.packageNoPax_listitem = pviewmodel.GetPackageNoofPaxListItems();

                return PartialView("_packages", newpackage);
            }
           

            string packagename = string.Empty;


            if (!String.IsNullOrEmpty(newpackage.p_descripton))
            {
                packagename = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(newpackage.p_descripton);
            }
            else
            {
                packagename = newpackage.p_descripton;
            }
            
            var packagedata = new Package
            {
                p_id = Convert.ToInt32(newpackage.p_id),
                p_type = newpackage.packagetype,
                p_descripton = packagename,
                p_amountPax = Convert.ToDecimal(newpackage.p_amountPax),
                p_min = newpackage.p_min,
                isActive = true,
                nopax_id = newpackage.packagenopax_id

            };

            _dbcontext.Packages.Add(packagedata);
            _dbcontext.SaveChanges();
           
            Utilities.ActiveForm= "newPackagebody";
          
            Utilities.PackageBodyModel.package_Id = Convert.ToInt32(packagedata.p_id);
            Utilities.PackageBodyModel.package_name = packagedata.p_descripton;

            var url = Url.Action("Package_Body_Details", "Packages");

            return Json(new {success=true,url=url }, JsonRequestBehavior.AllowGet);
            //return PartialView("_packagebody", Utilities.PackageBodyModel);

        }

        [HttpGet]
        public ActionResult Package_Body_Details()
        {
            PackageBodyViewModel packagebody = new PackageBodyViewModel()
            {
                package_Id = Utilities.PackageBodyModel.package_Id,
                package_name = Utilities.PackageBodyModel.package_name
            };

        
            return PartialView("_packagebody", packagebody);
        }

        //add course to package
        [HttpPost]
        public ActionResult AddCoursetoPackageBody(int packageId,int courseId)
        {
           
            bool isrecordexist = false;

            string _url = String.Empty;

            try
            {

                //check if course id exist in package

                var is_course_exist =
                    _dbcontext.PackageBodies.FirstOrDefault(x => x.p_id == packageId && x.courseId == courseId);

                if (is_course_exist != null)
                {
                    isrecordexist = true;
                }
                else
                {
                    
                    var coursepackagebody=new PackageBody()
                    {
                        p_id = packageId,
                        courseId = courseId
                    };

                    _dbcontext.PackageBodies.Add(coursepackagebody);
                    _dbcontext.SaveChanges();

                    _url = Url.Action("DisplaySelectedMenus", "Packages", new { _packageId = packageId });
                  

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return Json(new {is_recordexist= isrecordexist,url= _url }, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ProcessPackageDetails")]
        public ActionResult CreatePackage(PackageBodyViewModel p_bodyViewModel)
        {
            if (!ModelState.IsValid) return PartialView("_packagebody", p_bodyViewModel);

            var packagebodyDetails = new PackageBody
            {
                p_id = p_bodyViewModel.package_Id,
                //mainCourse = p_bodyViewModel.mainCourse,
                //sea_vegi = p_bodyViewModel.sea_vegi,
                //noodlepasta = p_bodyViewModel.noodlepasta,
                //salad = p_bodyViewModel.salad,
                //dessert = p_bodyViewModel.dessert,
                //pineapple = p_bodyViewModel.pineapple,
                //drinks = p_bodyViewModel.drinks,
                //rice = p_bodyViewModel.rice
            };
            _dbcontext.PackageBodies.Add(packagebodyDetails);
            _dbcontext.SaveChanges();

            Utilities.ActiveForm = "";

            ViewBag.ActiveForm = Utilities.ActiveForm;

            //var package_id = packagebodyDetails.p_id;

            return Json(new {success=true, packageId = packagebodyDetails.p_id },JsonRequestBehavior.AllowGet);


        }


        public ActionResult PackageDetails(int packageId)
        {
            ViewBag.FormTitle = "Package Buffet Details";

            Package package = new Package();
            package = _dbcontext.Packages.Find(packageId);
         
            //IQueryable<PackageAreaCoverage> areaCoverage =
            //(from areacover in _dbcontext.PackageAreaCoverages.Include(x=>x.Area) where areacover.p_id == (Int32) packageId  orderby areacover.p_id select areacover);

            var appno = _dbcontext.Packages_No_Pax_applicable.FirstOrDefault(pax => pax.nopax_id == package.nopax_id)
                .package_n_pax;

            PackageDetailsLocationViewModel P_details = new PackageDetailsLocationViewModel()
            {
                PackageId = packageId,
                Packages = package,
                packageApplicablepax = appno
                //PBody = _dbcontext.PackageBodies.Where(p => p.p_id ==packageId),
                //  PackageAreaCoverages = areaCoverage.ToPagedList(pageIndex,dataCount)
            };

          

            return View(P_details);
        }


        //Display all courses in packagebody
        [HttpGet,ChildActionOnly]
        public ActionResult DisplayPackageBodyCourses(int packageId)
        {

            //display courses for this package
            var cps=new Course_PackageSelectionViewModel();

            var listofcourses = cps.GetAllCoursesforPackage(packageId).ToList();

            return PartialView("_tablecoursespackagebody", listofcourses);
        }


        //Display All Selected Menus in Package Body
        [HttpGet]
        public ActionResult DisplaySelectedMenus(int _packageId)
        {

            var listofselectedpackagebodymenus = pbody.GetAllMenusinPackage().Where(pid => pid.package_Id == _packageId)
                .ToList();
             
            
            return PartialView("_selectedmenuspartial", listofselectedpackagebodymenus);
        }


        [HttpGet]
        public ActionResult AddPackageCoverage(int packageId)
        {
          
           
            PackageAreaLocationViewModel p_area=new PackageAreaLocationViewModel()
            {
                p_id = packageId,
                //AreasSelectList = packageArea.GetArea_SelectListItems()
            };

            return PartialView("_PackageCoverage", p_area);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPackageCoverage(PackageAreaLocationViewModel packageAreaViewModel)
        {
            if (!ModelState.IsValid) return PartialView("_PackageCoverage", packageAreaViewModel);

            var packageCoverage = new PackageAreaCoverage()
            {
                p_id=packageAreaViewModel.p_id,
                aID=packageAreaViewModel.aID,
                is_extended=packageAreaViewModel.is_extended,
                ext_amount=packageAreaViewModel.ext_amount,

            };



            _dbcontext.PackageAreaCoverages.Add(packageCoverage);
            _dbcontext.SaveChanges();

            return Json(new {success=true, packageId = packageAreaViewModel.p_id},JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ModifyPackageLocation(int packageId,int p_area_No)
        {

            var dbentities = new PegasusEntities();
            PackageAreaLocationViewModel p_area = new PackageAreaLocationViewModel();

            var packageareacoverage = dbentities.PackageAreaCoverages.FirstOrDefault(p => p.p_areaNo == p_area_No);
            if (packageareacoverage != null)
            {
                p_area = new PackageAreaLocationViewModel()
                {
                    p_id = packageId,
                    p_areaNo = p_area_No,
                  
                    aID = Convert.ToInt32(packageareacoverage.aID),
                    is_extended = Convert.ToBoolean(packageareacoverage.is_extended),
                    ext_amount = Convert.ToDecimal(packageareacoverage.ext_amount
                    )

                    //AreasSelectList = packageArea.GetArea_SelectListItems()
                };

                var arealoc = (dbentities.PackageAreaCoverages.Join(dbentities.Areas, p => p.aID, pa => pa.aID,
                        (p, pa) => new {areano = p.p_areaNo, area_detail = pa.AreaDetails}))
                    .ToList().FirstOrDefault(x => x.areano == p_area_No);
                if (arealoc != null)
                {
                    p_area.areadeatails = arealoc.area_detail;
                }

            }
        

            return PartialView("_modifypackagelocationCoverage", p_area);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifyPackageLocation(PackageAreaLocationViewModel packageAreaViewModel)
        {
            if (!ModelState.IsValid) return PartialView("_modifypackagelocationCoverage", packageAreaViewModel);

            var packageCoverage = new PackageAreaCoverage()
            {
                
                p_id = packageAreaViewModel.p_id,
                aID = packageAreaViewModel.aID,
                p_areaNo = (int) packageAreaViewModel.p_areaNo,
                is_extended = packageAreaViewModel.is_extended,
                ext_amount = packageAreaViewModel.ext_amount

            };



            _dbcontext.PackageAreaCoverages.Attach(packageCoverage);
            _dbcontext.Entry(packageCoverage).State=EntityState.Modified;
            _dbcontext.SaveChanges();

            return Json(new { success = true, packageId = packageAreaViewModel.p_id }, JsonRequestBehavior.AllowGet);
        }


        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin)]
        [HttpPost]
        public ActionResult RemovePackage(int packageId)
        {
            Package package=new Package();
            package = _dbcontext.Packages.Find(packageId);
            bool success = false;

            //==check package if has existing/active bookings
            bool hasExistingBooking = pb.VerifyPackagehasBookings(packageId);

            if (hasExistingBooking==false)
            {

                try
                {
                    var packagebody = _dbcontext.PackageBodies.FirstOrDefault(p => p.p_id == packageId);

                    if (packagebody != null)
                    {
                        _dbcontext.PackageBodies.Remove(packagebody);
                        _dbcontext.Entry(packagebody).State = EntityState.Deleted;
                    }
                  

                  
                    _dbcontext.Packages.Remove(package);

                    _dbcontext.SaveChanges();

                    success = true;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

              

              
            }
            

            return Json(new {success=success,package_name=package.p_descripton}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SearchPackage_Transaction()
        {
            var packagebyAreaDistinct =
                new PackageAreaLocationViewModel
                {
                    AreaByPackageDistinct = packageArea.GetPackageByAreaDistinct(),
                    ApplicablePaxSelectList = packageArea.GetApplicablePax_SelectListItems()
                };


            return PartialView("_searchPackage", packagebyAreaDistinct);
        }

        
        public JsonResult GetAreas(string query)
        {
            var areaList = packageArea.GetSelect2AreaViewModels().Where(x => x.text.ToLower().Contains(query.ToLower())).ToList();

            return Json(new { areaList }, JsonRequestBehavior.AllowGet);

        }

        //search and load to table => packages
        public JsonResult GetPackagesByLocation(int? areaId)
        {
           
            var packagesbyLocation=new List<AreaPackageViewModel>();

            try
            {
                packagesbyLocation = areaPackage.GetAreasByPackages()
                                    .Where(x => x.areaId.Equals(areaId))
                                    .OrderBy(x=>x.packageId).ToList();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Json(new {data= packagesbyLocation}, JsonRequestBehavior.AllowGet);
        }



        public JsonResult getResultSearchPackageBooking(string searchstr, string selectedId)
        {
         

            var packages = new List<AreaPackageViewModel>();

            if (selectedId == "packageTypeSelectList")
            {
                packages = areaPackage.GetPackageByType()
                    .Where(x => x.pType == searchstr.Trim())
                    .OrderByDescending(order=>order.packageId).ToList();
            }
            else if(selectedId == "areaSelectList")
            {
                if (!string.IsNullOrEmpty(searchstr))
                {
                    packages = areaPackage.GetAreasByPackages()
                        .Where(x => x.areaId.Equals(Convert.ToInt32(searchstr)))
                         .OrderByDescending(order => order.packageId).ToList();
                        
                       
                }
                

            }

            else
            {
                packages = areaPackage.GetPackageByType()
                    .Where(x => x.noPaxId == Convert.ToInt32(searchstr))
                    .OrderByDescending(order => order.packageId).ToList();
            }

            //packagesbytype = areaPackage.GetPackageByType().ToList();

            return Json(new {data= packages},JsonRequestBehavior.AllowGet);
        }


        //load package on booking

        [HttpGet]
       [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin)]
        public ActionResult ModifyPackage(int packageId)
       {
           ViewBag.packageId = packageId;
            Utilities.ActiveForm = "";

            ViewBag.ActiveForm = Utilities.ActiveForm;

            return View();
        }

        [HttpGet]
        public ActionResult PackageMain_Modify(int packageId)
        {
            //ViewBag.ActiveForm = Utilities.ActiveForm;

            var package = _dbcontext.Packages.Find(packageId);
            var packageviewmodel=new PackageViewModel();

            if (package != null)
            {

                packageviewmodel.p_id = package.p_id;
                packageviewmodel.packagetype =Convert.ToString(package.p_type).Trim();
                packageviewmodel.packageNoPax_listitem = pviewmodel.GetPackageNoofPaxListItems();
                packageviewmodel.p_descripton = package.p_descripton;
                packageviewmodel.p_amountPax =Convert.ToDecimal(package.p_amountPax);
                packageviewmodel.p_min = package.p_min;
                packageviewmodel.packagenopax_id = Convert.ToInt32(package.nopax_id);

            }

            return PartialView("_packageMain_modify_partialview", packageviewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PackageMain_Modify(PackageViewModel updatedPackage)
        {
            //update package main

            if (!ModelState.IsValid) return PartialView("_packages", updatedPackage);

            var packagemodify = _dbcontext.Packages.FirstOrDefault(package => package.p_id == updatedPackage.p_id);

            if (packagemodify != null)
            {

                packagemodify.p_id = Convert.ToInt32(updatedPackage.p_id);
                packagemodify.p_type = updatedPackage.packagetype;
                packagemodify.p_descripton = updatedPackage.p_descripton.Trim();
                packagemodify.p_amountPax = Convert.ToDecimal(updatedPackage.p_amountPax);
                packagemodify.p_min = updatedPackage.p_min;
                packagemodify.isActive = true;
                packagemodify.nopax_id = updatedPackage.packagenopax_id;

                Utilities.PackageBodyModel.package_Id = Convert.ToInt32(packagemodify.p_id);
                Utilities.PackageBodyModel.package_name = packagemodify.p_descripton;

              

            }

     
            _dbcontext.SaveChanges();

         

            Utilities.ActiveForm = "modifyPackagebody";

            var url = Url.Action("PackageBody_Modify", "Packages", new { packageId = packagemodify.p_id });

            return Json(new { success = true, url = url }, JsonRequestBehavior.AllowGet);

        }



        [HttpGet]
        public ActionResult PackageBody_Modify(int packageId)
        {
           // Utilities.ActiveForm = "modifyPackagebody";

            Utilities.PackageBodyModel.package_Id = Convert.ToInt32(packageId);

            Utilities.PackageBodyModel.package_name = _dbcontext.Packages.FirstOrDefault(p => p.p_id == packageId)
                .p_descripton;


            PackageBodyViewModel packagebody = new PackageBodyViewModel()
            {
                package_Id = Utilities.PackageBodyModel.package_Id,
                package_name = Utilities.PackageBodyModel.package_name
            };


            return PartialView("_packageBody_modify_partialview", packagebody);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PackageBody_Modify(PackageBodyViewModel packagebody)
        {
            if (!ModelState.IsValid) return PartialView("_packagebody", packagebody);

            var packagebodyDetails = new PackageBody
            {
                No = packagebody.pbodyKey,
                p_id = packagebody.package_Id,
                //mainCourse = packagebody.mainCourse,
                //sea_vegi = packagebody.sea_vegi,
                //noodlepasta = packagebody.noodlepasta,
                //salad = packagebody.salad,
                //dessert = packagebody.dessert,
                //pineapple = packagebody.pineapple,
                //drinks = packagebody.drinks,
                //rice = packagebody.rice
            };
            _dbcontext.PackageBodies.Attach(packagebodyDetails);
            _dbcontext.Entry(packagebodyDetails).State = EntityState.Modified;
            _dbcontext.SaveChanges();


       

            //Utilities.ActiveForm = "";

            //ViewBag.ActiveForm = Utilities.ActiveForm;

            //var package_id = packagebodyDetails.p_id;

            return Json(new { success = true, packageId = packagebody.package_Id }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult get_ListofPackageCourse(int packageId)
        {
            var list = pbody.GetAllMenusinPackage().Where(x => x.package_Id == packageId);

            return PartialView(list);
        }

        [HttpGet]
        public ActionResult get_ListofPackagesbyLocation(int packageId, int? page)
        {

            int pageIndex = page ?? 1;
            int dataCount = 7;


            IQueryable<PackageAreaCoverage> areaCoverage =
                (from areacover in _dbcontext.PackageAreaCoverages.Include(x => x.Area) where areacover.p_id == (Int32)packageId orderby areacover.p_id select areacover);

            PackageDetailsLocationViewModel P_details = new PackageDetailsLocationViewModel()
            {
                PackageId = packageId,
                Packages = (from p in _dbcontext.Packages where p.p_id==packageId select p).FirstOrDefault(),
                PackageAreaCoverages = areaCoverage.ToPagedList(pageIndex, dataCount)
            };

            return PartialView("_packagelocationCoverage_ViewModel", P_details);
        }


        [HttpPost]
        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin)]
        public ActionResult Remove_PackageLocaton(int pAreaNo)
        {
            bool success = false;

            int packageId=0;
            var packageAreaCoverage = _dbcontext.PackageAreaCoverages.Find(pAreaNo);

            if (packageAreaCoverage != null)
            {
                packageId =Convert.ToInt32(packageAreaCoverage.p_id);

                _dbcontext.PackageAreaCoverages.Remove(packageAreaCoverage);
                _dbcontext.SaveChanges();

                success = true;
            }
          
            var url=Url.Action("get_ListofPackagesbyLocation", "Packages", new { packageId = packageId });

            return Json(new {success=success,url= url }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin)]
        public ActionResult RemoveSelectedPackageCourse(int packageId, int courseId)
        {
            var _success = false;
            string _url=String.Empty;
            
            try
            {
                bool hasExistingBooking = pb.VerifyPackagehasBookings(packageId);

                if (hasExistingBooking == false)
                {
                    var delepackagebody =
                        _dbcontext.PackageBodies.FirstOrDefault(x => x.courseId == courseId && x.p_id == packageId);

                    if (delepackagebody != null)
                    {
                        _dbcontext.PackageBodies.Remove(delepackagebody);
                        _dbcontext.Entry(delepackagebody).State = EntityState.Deleted;
                        _dbcontext.SaveChanges();


                        _success = true;
                    }
                    _url = Url.Action("DisplaySelectedMenus", "Packages", new {_packageId = packageId});
                }
                else
                {
                    //has course has existing menus in booking

                    _success = false;
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return Json(new {success=_success, url= _url }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin)]
        public ActionResult PackageStatus(bool bolStat,int pId)
        {
           var packageStat=new Package();
            var url = String.Empty;
            try
            {
                packageStat = _dbcontext.Packages.Find(pId);
                if (packageStat != null) packageStat.isActive = bolStat;

                _dbcontext.Packages.Attach(packageStat);
                _dbcontext.Entry(packageStat).State = EntityState.Modified;
                _dbcontext.SaveChanges();

                url = Url.Action("get_ListofPackagesbyLocation", "Packages", new { packageId = pId });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

           

            return Json(new {success=true,url=url}, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            _dbcontext.Dispose();
        }
    }
}