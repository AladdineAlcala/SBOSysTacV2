using Microsoft.AspNet.Identity;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.Models;
using SBOSysTacV2.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Drawing.Printing;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web.Services.Protocols;
using System.Web.SessionState;
using SBOSysTacV2.ServiceLayer;

namespace SBOSysTacV2.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    { 

        private PegasusEntities _dbcontext;
        private BookingsViewModel booking;
        private MainMenuListViewModel mainmenulistviewmodel;
        private AddonsViewModel addonsviewmodel;
        private TransactionDetailsViewModel transDetailsvm;
        private ContractReceiptViewModel cr;
        private AddonsUpgrade_BookRegisterViewModel addupgradereg;
        private SelectedAddonsViewModel seladdons;
        private BookMenusViewModel book_menus_vm;
        private CancelBookingViewModel cancelledBooking;
        private CourseCategoryViewModel courseCategory;
        private PackageBookingViewModel package_book_vm;
        private BookingOtherChargeViewModel bookOtherCharge;
        Func<Booking, List<ICollection<BookAddonsDetail>>> getAddonDetails = BookingAddonDetailsViewModel.GetAddonDetails;

        public BookingsController()
        {
            _dbcontext = new PegasusEntities();
            booking = new BookingsViewModel();
            mainmenulistviewmodel = new MainMenuListViewModel();
            addonsviewmodel = new AddonsViewModel();
            transDetailsvm = new TransactionDetailsViewModel();
            cr = new ContractReceiptViewModel();
            addupgradereg = new AddonsUpgrade_BookRegisterViewModel();
            seladdons = new SelectedAddonsViewModel();
            book_menus_vm = new BookMenusViewModel();
            cancelledBooking = new CancelBookingViewModel();
            courseCategory = new CourseCategoryViewModel();
            package_book_vm = new PackageBookingViewModel();
            bookOtherCharge = new BookingOtherChargeViewModel();
        }

        // GET: Bookings
        public ActionResult Index()
        {
            ViewBag.FormTitle = "Bookings & Events Details";

            return View();
        }


        [HttpGet]
        public ActionResult LoadBookings()
        {
            var bookings = booking.GetListofBookings().OrderBy(d => d.startdate).ToList();

            return Json(new { data = bookings }, JsonRequestBehavior.AllowGet);

        }

        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin, UserPermessionLevelEnum.user)]
        [HttpGet]
        public ActionResult CreateBooking()
        {
            ViewBag.FormTitle = "Create New Bookings";

            var newBooking = new BookingsViewModel
            {
                transdate = DateTime.Now,
                startdate = DateTime.Now,
                Servicetype_ListItems = booking.GetServiceType_SelectListItems(),
                b_createdbyUser = User.Identity.GetUserId(),
                b_createdbyUserName = User.Identity.GetUserName(),
                b_updatedDate = DateTime.Now,
                DictBooktype = booking.GetDictBookingType()

            };

            return View(newBooking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateBooking(BookingsViewModel bookingViewModel)
        {
            if (!ModelState.IsValid)
            {
                bookingViewModel.Servicetype_ListItems = booking.GetServiceType_SelectListItems();

                return View(bookingViewModel);
            }

            bool success = false;

            //  _dbcontext.Configuration.ProxyCreationEnabled = false;

            DateTime createdDate = DateTime.Now;
            int transactionId = 0;
            decimal amountPax = 0;
            var firstOrDefault = _dbcontext.Packages.FirstOrDefault(x => x.p_id == bookingViewModel.pId);

            if (firstOrDefault != null)
            {
                amountPax = Convert.ToDecimal(firstOrDefault.p_amountPax);
            }

            var cusId = bookingViewModel.c_Id;

            if (!String.IsNullOrEmpty(cusId.ToString()))
            {

                var cus_in_record = _dbcontext.Customers.Any(x => x.c_Id == cusId);

                if (cus_in_record)
                {

                    try
                    {

                        var newBooking = new Booking
                        {
                            c_Id = bookingViewModel.c_Id,
                            booktype = bookingViewModel.booktypecode,
                            noofperson = bookingViewModel.noofperson,
                            occasion = bookingViewModel.occasion,
                            venue = bookingViewModel.venue,
                            typeofservice = bookingViewModel.serviceId ?? 2,
                            startdate = bookingViewModel.startdate,
                            enddate = bookingViewModel.startdate,
                            transdate = bookingViewModel.transdate,
                            p_id = bookingViewModel.pId,
                            apply_extendedAmount = bookingViewModel.apply_extendedAmount,
                            extendedAreaId = bookingViewModel.areaId,
                            serve_stat = false,
                            eventcolor = bookingViewModel.eventcolor,
                            p_amount = amountPax,
                            b_createdbyUser = bookingViewModel.b_createdbyUser,
                            reference = bookingViewModel.refernce,
                            b_updatedDate = createdDate,
                            is_cancelled = false,
                            is_deleted = false
                            
                        };


                        _dbcontext.Bookings.Add(newBooking);
                        _dbcontext.SaveChanges();

                        transactionId = newBooking.trn_Id;

                        success = true;

                        return Json(new { success = success, trnsId = transactionId }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                }




            }


            //bookingViewModel.Servicetype_ListItems = booking.GetServiceType_SelectListItems();

            return Json(new { success = false }, JsonRequestBehavior.AllowGet);

        }


        //booking customer exist 
        [HttpPost]
        public JsonResult IsCustomerRegistered(string fullname)
        {
            bool customerexist;

            string[] splitfullname = fullname.Split(',');

            if (splitfullname.Length > 1)
            {
                customerexist = Hascustomerexist(splitfullname[0].Trim());
            }
            else
            {
                customerexist = false;
            }

            return Json(customerexist, JsonRequestBehavior.AllowGet);
        }

  


        public bool Hascustomerexist(string lname) => _dbcontext.Customers.Any(x => x.lastname == lname);




        //[OutputCache(Duration = int.MaxValue, VaryByParam = "transactionId")]
        public ActionResult GetPackageId(int? transactionId)
        {
            var booking = new Booking();
            var is_success = false;

            booking = _dbcontext.Bookings.Find(transactionId);

            if (booking.p_id != null)
            {
                is_success = true;
            }

            return Json(new { success = is_success }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult GetPackageonBooking(int transId)
        {

            return PartialView("GetPackageonBooking", new TransactionDetailsViewModel()
            {
                transactionId = transId
            });

        }


        [HttpGet]
        public ActionResult GetPackageBookingDetails(int transId) => View(booking.GetBookingByTransaction(transId));


        [HttpGet]
        [ActionName("GetBookingDetailsPartial")]
        public ActionResult BookingDetails(int transId) => PartialView("_BookingDetailsPartial", transDetailsvm.GetTransactionDetailsById(transId));




        /// <summary>
        /// This action will compute  transaction statement of account
        /// </summary>
        /// <param name="transDetails"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetPartialView_AmountDue(TransactionDetailsViewModel transDetails)
        {

            var transId = transDetails.transactionId;
            decimal packageTotal = 0;
            decimal addonsTotal = 0;
            decimal belowminPax = 0;
            decimal extendedLocationAmount = 0;
            decimal dpAmount = 0;
            decimal fpAmount = 0;
            decimal bookdiscountAmount = 0;
            decimal cateringdiscountAmount = 0;
            string discountCode = null;
            decimal packageAmount;


            try
            {

                var packageType = transDetails.Package_Trans.p_type.TrimEnd();

                int noOfPax = Convert.ToInt32(transDetails.Booking_Trans.noofperson);

                var bookings = _dbcontext.Bookings.Find(transDetails.Booking_Trans.trn_Id);

                packageAmount = (decimal) (!string.Equals(packageType, PackageEnum.packageType.sd.ToString(), StringComparison.Ordinal)
                    ? transDetails.Package_Trans.p_amountPax
                    : book_menus_vm.ComputeAmountForSnacksByTransId(transId));


                //var addonslist = _dbcontext.BookingAddons.Where(x => x.trn_Id == transId).ToList();

                // Compute Addons
                addonsTotal = AddonsViewModel.AddonsTotal(getAddonDetails(bookings));


                if ((bool)transDetails.Booking_Trans.apply_extendedAmount) // check if location extended charge is true otherwise extended location will be zero value
                {
                    extendedLocationAmount = transDetailsvm.Get_extendedAmountLoc(transId);
                }

                
                dpAmount = transDetailsvm.GetTotalDownPayment(transId);

                fpAmount = transDetailsvm.GetFullPayment(transId);

                cateringdiscountAmount = transDetailsvm.GetCateringdiscountByPax(packageType.Trim(), noOfPax);

                packageTotal = Convert.ToDecimal(packageAmount) * noOfPax;

                var subtotal = (packageTotal + addonsTotal + extendedLocationAmount + belowminPax);


                //get discount information
                bookdiscountAmount = transDetailsvm.Get_bookingDiscountbyTrans(transId, subtotal);

                discountCode = transDetailsvm.Get_bookingDiscounDetailstbyTrans(transId);

                transDetails.PackageAmount = packageAmount;

                transDetails.TotaMiscCharge = bookOtherCharge.GetTotalOtherCharges(transDetails.BookOtherCharges);

                transDetails.TotaAddons = addonsTotal;
                transDetails.extLocAmount = extendedLocationAmount;
                //_transDetails.TotaBelowMinPax = belowminPax;
                transDetails.TotaDp = dpAmount;
                transDetails.fullpaymnt = fpAmount;
                transDetails.book_discounts = bookdiscountAmount;
                transDetails.bookdiscountdetails = discountCode;
                transDetails.cateringdiscount = cateringdiscountAmount;


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return PartialView("_BookingsAmountDuePartial", transDetails);
        }

        //public IEnumerable<ICollection<BookAddonsDetail>> Func { get; set; }


        [HttpGet]
        [ActionName("GetBookMenusPartial")]
        public ActionResult GetListofBookMenus(int transactionId)
        {
            string actionname = RouteData.Values["action"].ToString();

            PackageActionType.Getpackagecontroller(actionname);

            var packageBooking = package_book_vm.GetBookingDetailById(transactionId);

            return PartialView("_ListofBookMenus", new PackageBookingViewModel()
            {
                transactionId = transactionId,
                Booking = packageBooking.Booking,
                BookMenuses = book_menus_vm.ListOfMenusBook(packageBooking).ToList()

            });
        }


        [HttpGet]
        [ActionName("GetSnacksPartial")]
        public ActionResult GetListofSnacks(int transactionId)
        {
            string actionname = RouteData.Values["action"].ToString();

            PackageActionType.Getpackagecontroller(actionname);

            var packageBooking = package_book_vm.GetBookingDetailById(transactionId);

            return PartialView("_GetSnacksForm",new PackageBookingViewModel()
            {
                transactionId = transactionId,
                Booking = packageBooking.Booking,
                Package = package_book_vm.GetPackageByTransaction_Id(transactionId),
                BookMenuses = book_menus_vm.Get_Menu_on_SnackPackageByTransId(transactionId).ToList()

            });
        }


        [HttpGet]
        [ChildActionOnly]
        public ActionResult GetListofAddons(int transId)
        {
            var listaddons = addonsviewmodel.ListofAddons();

            return PartialView("_GetListofAddons", new PackageBookingViewModel()
            {
                transactionId = transId,
                BookAddOns = listaddons.Where(x => x.TransId.Equals(transId)).OrderBy(x => x.bookaddonNo).ToList()

            });

        }

        [HttpGet]
        public ActionResult GetListofCourse(int transactionId, int courseId,int no_of_pax)
        {
            return PartialView("_GetListofMainCourse", new BookMenusViewModel()
            {
                transId = transactionId,
                servingpax = (int)no_of_pax,
                course_id = courseId

            });

        }


        [HttpGet]
        [ActionName("GetPackageMenusperTransaction")]
        public ActionResult GetPackageMenus(int packageId)
        {
            
            var course = courseCategory.GetCourseByPackageId(packageId);

            var listmenudata = mainmenulistviewmodel.GetAllMenu_By_Course(course.ToList());

            return Json(new { data=listmenudata}, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        //[ActionName("LoadCustomerByBookNo")]
        public ActionResult GetListofCourseforChange(int bookmenuNo)
        {

            var bookMenus = (from bm in _dbcontext.Book_Menus
                             join m in _dbcontext.Menus on bm.menuid equals m.menuid
                             select new BookMenusViewModel()
                             {
                                 transId = (int)bm.trn_Id,
                                 menuId = bm.menuid,
                                 dept = m.Department.deptName,
                                 menu_name = m.menu_name,
                                 menu_No = bm.No,
                                 course_id = (int)m.courseId,
                                 serving = (decimal)bm.serving

                             }).FirstOrDefault(x => x.menu_No == bookmenuNo);

            return PartialView("_GetListofMainCourse_Change", bookMenus);
        }




        [HttpGet]
        public ActionResult LoadListMenus(int courseid)
        {

            var listmenudata = new List<MainMenuListViewModel>();

            var listofmenu = mainmenulistviewmodel.ListofMainMenu().ToList();

            if (courseid > 0)
            {
                //========== Check courseid for posible multiple course .. e.g  ( Pork/Beef/Checken ) ==================

                var coursecategoryhasBar = mainmenulistviewmodel.Check_BarCourseCategory(courseid);

                if (coursecategoryhasBar)
                {
                    var listWithBar = mainmenulistviewmodel.MainMenuList_with_Bar(courseid);

                    listmenudata = listWithBar.Count <= 0 ? listofmenu.Where(t => t.course_id == courseid).ToList() : listofmenu.Where(l1 => listWithBar.Any(l2 => l1.course_id == l2.Key)).ToList();
                }
                else
                {
                    listmenudata = listofmenu.Where(t => t.course_id == courseid).ToList();
                }

            }
            else
            {
                listmenudata = listofmenu;
            }
           

            //var menulistbycourse = listofmenu.Find(t => t.courseid == courseid);

            return Json(new { data = listmenudata }, JsonRequestBehavior.AllowGet);
        }





        [HttpPost]
        public ActionResult AddMenusToPackage(BookMenusViewModel bookmenus)
        {
            bool isRecordExist = false;
            string _message = "";
            string menu_details = "";

            dynamic showErrMessageString = String.Empty;

            var url = "";

            if (!ModelState.IsValid)
            {

                return PartialView("_GetListofMainCourse", bookmenus);
            }
            else
            {

                try
                {

                    Book_Menus isExistMenu =
                        _dbcontext.Book_Menus.FirstOrDefault(x => x.trn_Id == bookmenus.transId && x.menuid == bookmenus.menuId);

                    var menu = _dbcontext.Menus.FirstOrDefault(x => x.menuid == bookmenus.menuId);

                    if (menu != null)
                    {
                        menu_details = menu.menu_name;
                    }


                    if (isExistMenu == null)
                    {
                        TransactionDetailsViewModel td = new TransactionDetailsViewModel();

                        //=====check if selected course is a main menu
                        if (td.isSelectedMenuMainCourse(bookmenus.menuId) == true)
                        {
                            var bookings = _dbcontext.Bookings.Find(bookmenus.transId);

                            if (bookmenus.get_totalselectedMainMenus(bookmenus.transId) <

                                book_menus_vm.Get_PackageMainMenusInt(Convert.ToInt32(bookings.p_id)))
                            {

                                var bookMenu = new Book_Menus()
                                {
                                    trn_Id = bookmenus.transId,
                                    menuid = bookmenus.menuId,
                                    courseId = bookmenus.course_id == 0 ? menu.courseId : bookmenus.course_id,
                                    s_price = bookmenus.price,
                                    createdDate = bookmenus.createdDate,
                                    updatedDate = bookmenus.updatedDate,
                                    serving = bookmenus.serving??bookmenus.servingpax
                                };

                                _dbcontext.Book_Menus.Add(bookMenu);
                       


                                //url = Url.Action("GetBookMenusPartial", "Bookings",
                                //    new { transactionId = bookMenu.trn_Id });
                            }

                            else // exceed on total number count of main menu
                            {
                                isRecordExist = true;
                                _message = menu_details + "  already exceed on maximum main menu count";

                                showErrMessageString = new
                                {
                                    param1 = 404,
                                    param2 = _message
                                };
                            }

                        }
                        else
                        {



                            var bookMenu = new Book_Menus()
                            {
                                trn_Id = bookmenus.transId,
                                menuid = bookmenus.menuId,
                                courseId = bookmenus.course_id==0? menu.courseId: bookmenus.course_id,
                                s_price = bookmenus.price,
                                createdDate = bookmenus.createdDate,
                                updatedDate = bookmenus.updatedDate,
                                serving = bookmenus.serving ?? bookmenus.servingpax
                            };

                            _dbcontext.Book_Menus.Add(bookMenu);
                           


                          
                        }

                        _dbcontext.SaveChanges();
                    }
                    else
                    {
                        isRecordExist = true;
                        _message = menu_details + " already in the list";

                        showErrMessageString = new
                        {
                            param1 = 404,
                            param2 = _message
                        };
                    }

                }
                catch (Exception ex)
                {

                    throw ex;
                }

                var package = package_book_vm.GetPackageByTransaction_Id(bookmenus.transId);


                if (package.p_type.TrimEnd() != "sd")
                {
                    url = Url.Action("GetBookMenusPartial", "Bookings", new { transactionId = bookmenus.transId });
                }
                else
                {
                     url = Url.Action("GetBookingDetailsPartial", "Bookings", new { transId = bookmenus.transId });
                }

                _dbcontext.Dispose();
                //url = Url.Action("GetBookingDetailsPartial", "Bookings", new { transId = bookmenus.transId });

                return Json(new { isRecordExist = isRecordExist, ShowErrMessageString = showErrMessageString, url = url,packageType=package.p_type.TrimEnd() }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        public ActionResult Change_Menu_on_Booking(BookMenusViewModel modifiedBookMenu)
        {

            

            if (!ModelState.IsValid)
            {
                return PartialView("_GetListofMainCourse_Change", modifiedBookMenu);
            }


            bool modifysuccess = false;
            string _message = "";
            string menu_details = "";
            dynamic StatMessageString = String.Empty;
            var url = "";

            try
            {

                TransactionDetailsViewModel td = new TransactionDetailsViewModel();

                /* no changes of menu from previous data posible change of number of serving or price */

                Book_Menus isExistMenu = _dbcontext.Book_Menus.AsNoTracking().FirstOrDefault(x => x.trn_Id == modifiedBookMenu.transId && x.menuid == modifiedBookMenu.menuId);

                var menu = _dbcontext.Menus.FirstOrDefault(x => x.menuid == modifiedBookMenu.menuId);

                if (menu != null)
                {
                    menu_details = menu.menu_name;
                }


                var bookMenus = _dbcontext.Book_Menus.FirstOrDefault(x => x.No == modifiedBookMenu.menu_No);

                if (bookMenus != null)
                {
            
                    bookMenus.trn_Id = modifiedBookMenu.transId;
                    bookMenus.menuid = modifiedBookMenu.menuId;
                    bookMenus.serving = modifiedBookMenu.serving;
                    bookMenus.updatedDate = DateTime.UtcNow;
                }


                if (isExistMenu != null) //menu selected was the same 
                {

                    //check serving if not exceed to no of pax

                    var noofPax = _dbcontext.Bookings.FirstOrDefault(x => x.trn_Id == modifiedBookMenu.transId).noofperson;

                    if (isExistMenu.serving != 0)
                    {

                        if (isExistMenu.serving <= noofPax)
                        {
                            _dbcontext.Book_Menus.Attach(bookMenus);
                            _dbcontext.Entry(bookMenus).State = EntityState.Modified;
                            _dbcontext.SaveChanges();

                            modifysuccess = true;
                            _message = menu_details + " succesfully updated";

                            StatMessageString = new
                            {
                                param1 = 200,
                                param2 = _message
                            };
                        }

                        else
                        {
                            modifysuccess = true;
                            _message = menu_details + " succesfully updated";

                            StatMessageString = new
                            {
                                param1 = 404,
                                param2 = _message
                            };
                        }
                    }
                    else
                    {

                        _dbcontext.Book_Menus.Attach(bookMenus);
                        _dbcontext.Entry(bookMenus).State = EntityState.Modified;
                        _dbcontext.SaveChanges();

                        modifysuccess = true;
                        _message = menu_details + " succesfully updated";

                        StatMessageString = new
                        {
                            param1 = 200,
                            param2 = _message
                        };

                    }
                }
                else //new menu selected
                {

                    _dbcontext.Book_Menus.Attach(bookMenus);
                    _dbcontext.Entry(bookMenus).State = EntityState.Modified;

                    _dbcontext.SaveChanges();

                    modifysuccess = true;
                    _message = menu_details + " succesfully updated";

                    StatMessageString = new
                    {
                        param1 = 404,
                        param2 = _message
                    };




                }//end existmenu

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }// end try

            var package = package_book_vm.GetPackageByTransaction_Id(modifiedBookMenu.transId);

            url = PackageActionType.pactype == bookAction.GetSnacksPartial.ToString() ? Url.Action("GetBookingDetailsPartial", "Bookings", new { transId = modifiedBookMenu.transId }) : Url.Action("GetBookMenusPartial", "Bookings", new { transactionId = modifiedBookMenu.transId });


            return Json(new { success = modifysuccess, StatMessageString, url = url, packageType = package.p_type.TrimEnd() }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult AddOnsInformation(int transactionId)
        {

            AddonsViewModel addons = new AddonsViewModel
            {
                TransId = transactionId
            };

            return PartialView("_AddOnsInformation", addons);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAddons(AddonsViewModel addons)
        {
            if (!ModelState.IsValid) return PartialView("_AddOnsInformation", addons);


            var _addons = new BookingAddon
            {
                trn_Id = addons.TransId,
                Addondesc = addons.AddonsDescription,
                Note = addons.AddonNote,
                //AddonAmount = addons.AddonAmount
            };

            _dbcontext.BookingAddons.Add(_addons);
            _dbcontext.SaveChanges();

            ModelState.Clear();

            var ReturnUrl = Url.Action("GetListofAddons", "Bookings", new { transId = addons.TransId });

            return Json(new { success = true, url = ReturnUrl }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveMenufromListofBookings(int bookmenuNo)
        {

            bool success = true;
            var returnUrl = "";

            Book_Menus bm = new Book_Menus();

            try
            {
                bm = _dbcontext.Book_Menus.Find(bookmenuNo);

                var transactionId = bm.trn_Id;

                if (bm != null)
                {
                    _dbcontext.Book_Menus.Remove(bm);
                    _dbcontext.SaveChanges();

                    returnUrl = Url.Action("GetBookMenusPartial", "Bookings", new { transactionId = transactionId });

                }
                else
                {
                    success = false;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return Json(new { success = success, url = returnUrl }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ModifyAddOns(int addonItemNo)
        {

            string view = string.Empty;

            var addons = addonsviewmodel.GetAddonsViewModelbyitemNo(addonItemNo);



            if (addons != null)
            {
                if (addons.addonId == null)
                {
                    view = "_ModifyAddOns";


                }
                else
                {

                    //var selectedaddons = seladdons.GetSelectedAddons(addonItemNo);

                    //view = "GetSelectedAddons_Modify";

                    //return PartialView("GetSelectedAddons_Modify", selectedaddons);
                    return PartialView("GetSelectedAddons_Modify");
                }

            }

            return PartialView(view, addons);


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifyAddOns(AddonsViewModel modifyaddons)
        {
            if (!ModelState.IsValid) return PartialView("_AddOnsInformation", modifyaddons);


            var _addons = new BookingAddon
            {
                bookaddonNo = modifyaddons.bookaddonNo,
                trn_Id = modifyaddons.TransId,
                Addondesc = modifyaddons.AddonsDescription,
                Note = modifyaddons.AddonNote,
                //AddonAmount = modifyaddons.AddonAmount
            };

            _dbcontext.BookingAddons.Attach(_addons);
            _dbcontext.Entry(_addons).State = EntityState.Modified;

            _dbcontext.SaveChanges();

            ModelState.Clear();

            var ReturnUrl = Url.Action("GetListofAddons", "Bookings", new { transId = modifyaddons.TransId });

            return Json(new { success = true, url = ReturnUrl }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RemoveBookingAddon(int addonNo)
        {

            BookingAddon ba = new BookingAddon();
            int transactionId;
            string returnUrl = String.Empty;
            bool success = true;
            var message = string.Empty;
            ;


            try
            {
                ba = _dbcontext.BookingAddons.Find(addonNo);

                if (ba != null)
                {
                    var query = (from b in _dbcontext.BookAddonsDetails where b.bookaddonNo == ba.bookaddonNo select b).Count();

                    if (query <= 0)
                    {
                        transactionId = Convert.ToInt32(ba.trn_Id);


                        _dbcontext.BookingAddons.Remove(ba);
                        _dbcontext.SaveChanges();


                        returnUrl = Url.Action("GetListofAddons", "Bookings", new { transId = transactionId });



                    }
                    else
                    {
                        success = false;
                        message = "Pls remove data from booking addon details link to this record";
                    }
                }

                else
                {
                    message = "Unable to remove record..Pls. check error";
                    success = false;
                }

                            
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return Json(new { success = success, url = returnUrl,message }, JsonRequestBehavior.AllowGet);
        }


        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin)]
        [HttpGet]
        public ActionResult EditBooking(int transId)
        {

            Booking editBooking = new Booking();
            BookingsViewModel bookingviewmodel = new BookingsViewModel();
            CustomerViewModel cus = new CustomerViewModel();

            editBooking = _dbcontext.Bookings.Find(transId);

            if (editBooking != null)
            {
                bookingviewmodel.trn_Id = editBooking.trn_Id;
                bookingviewmodel.transdate = Convert.ToDateTime(editBooking.transdate);
                bookingviewmodel.c_Id = editBooking.c_Id;
                bookingviewmodel.noofperson = editBooking.noofperson;
                bookingviewmodel.occasion = editBooking.occasion;
                bookingviewmodel.venue = editBooking.venue;
                bookingviewmodel.serviceId = editBooking.typeofservice;
                bookingviewmodel.startdate = editBooking.startdate;
                bookingviewmodel.serve_status = editBooking.serve_stat;
                bookingviewmodel.eventcolor = editBooking.eventcolor;

                bookingviewmodel.pId = Convert.ToInt32(editBooking.p_id);
                bookingviewmodel.fullname = cus.get_CustomerFullname(Convert.ToInt32(editBooking.c_Id));

                bookingviewmodel.Servicetype_ListItems = booking.GetServiceType_SelectListItems();

                bookingviewmodel.selected_servicetype = _dbcontext.ServiceTypes
                    .Where(x => x.serviceId == editBooking.typeofservice).Select(s => s.servicetypedetails).Single();

                if (editBooking.p_id != null)
                {
                    bookingviewmodel.packagename = _dbcontext.Packages.Where(x => x.p_id == editBooking.p_id)
                        .Select(p => p.p_descripton).Single();


                }



                if (editBooking.Package.p_type.Trim() != "vip")
                {

                    if (editBooking.extendedAreaId != null)
                    {

                        var area = _dbcontext.Areas.Find(editBooking.extendedAreaId);

                        if (area != null)
                        {
                            bookingviewmodel.areaId = (int)editBooking.extendedAreaId;

                            bookingviewmodel.area_desc = area.AreaDetails;
                        }


                    }


                }

                bookingviewmodel.booktypecode = !string.Equals(editBooking.booktype, null, StringComparison.Ordinal) ? editBooking.booktype.Trim() : " ";
                bookingviewmodel.DictBooktype = booking.GetDictBookingType();

                bookingviewmodel.apply_extendedAmount = Convert.ToBoolean(editBooking.apply_extendedAmount);

                bookingviewmodel.refernce = editBooking.reference;
                bookingviewmodel.b_createdbyUser = User.Identity.GetUserId();
                bookingviewmodel.b_createdbyUserName = User.Identity.GetUserName();
                bookingviewmodel.b_updatedDate = DateTime.Now;
            }

            return View(bookingviewmodel);
        }



        [HttpPost, ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult EditBooking(BookingsViewModel bookingViewModel)
        {
            if (!ModelState.IsValid) return View(bookingViewModel);

            DateTime createdDate = DateTime.Now;
            //  _dbcontext.Configuration.ProxyCreationEnabled = false;
            bool success = false;

            decimal amountPax = 0;
            var firstOrDefault = _dbcontext.Packages.FirstOrDefault(x => x.p_id == bookingViewModel.pId);

            if (firstOrDefault != null)
            {
                amountPax = Convert.ToDecimal(firstOrDefault.p_amountPax);
            }

            var updatedBooking = _dbcontext.Bookings.FirstOrDefault(b => b.trn_Id == bookingViewModel.trn_Id);

            if (updatedBooking != null)
            {
                updatedBooking.c_Id = bookingViewModel.c_Id;
                updatedBooking.noofperson = bookingViewModel.noofperson;
                updatedBooking.occasion = bookingViewModel.occasion;
                updatedBooking.venue = bookingViewModel.venue;
                updatedBooking.typeofservice = bookingViewModel.serviceId ?? 2;
                updatedBooking.startdate = bookingViewModel.startdate;
                updatedBooking.enddate = bookingViewModel.startdate;
                updatedBooking.eventcolor = bookingViewModel.eventcolor;
                updatedBooking.transdate = bookingViewModel.transdate;
                updatedBooking.apply_extendedAmount = bookingViewModel.apply_extendedAmount;
                updatedBooking.extendedAreaId = bookingViewModel.areaId;
                updatedBooking.p_id = bookingViewModel.pId;
                updatedBooking.booktype = bookingViewModel.booktypecode;
                updatedBooking.p_amount = amountPax;
                updatedBooking.reference = bookingViewModel.refernce;
                updatedBooking.b_updatedDate = createdDate;
                updatedBooking.b_createdbyUser = User.Identity.GetUserId();
                updatedBooking.serve_stat = bookingViewModel.serve_status;
                updatedBooking.is_cancelled = bookingViewModel.iscancelled;
                updatedBooking.is_deleted = bookingViewModel.isDeleted;

            }





            try
            {
                _dbcontext.Bookings.Attach(updatedBooking);
                _dbcontext.Entry(updatedBooking).State = EntityState.Modified;
                _dbcontext.SaveChanges();


                success = true;

                return Json(new { success = success, trnsId = updatedBooking.trn_Id }, JsonRequestBehavior.AllowGet);

            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }


            return Json(new { success = success }, JsonRequestBehavior.AllowGet);



        }


        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin)]
        [HttpPost]
        public ActionResult RemoveBooking(int transId)
        {
            bool success = false;

            Booking booking = new Booking();
            //Book_Discount bk = new Book_Discount();

            //BookingAddon bookaddons = new BookingAddon();

            booking = _dbcontext.Bookings.Find(transId);
            //IEnumerable<Book_Menus> bookmenustransList = _dbcontext.Book_Menus.Where(x => x.trn_Id == transId);
            //IEnumerable<BookingAddon> bookingAddons = _dbcontext.BookingAddons.Where(x => x.trn_Id == transId);
            //IEnumerable<Payment> paymentlist = _dbcontext.Payments.Where(x => x.trn_Id == transId);
            //bk = _dbcontext.Book_Discount.Find(transId);


            try
            {
                if (booking != null)
                {

                    _dbcontext.Bookings.Remove(booking);

                    //_dbcontext.Book_Menus.RemoveRange(bookmenustransList);
                    //_dbcontext.BookingAddons.RemoveRange(bookingAddons);
                    //_dbcontext.Payments.RemoveRange(paymentlist);

                    //if (bk != null)
                    //{
                    //    _dbcontext.Book_Discount.Remove(bk);
                    //}

                    booking.is_deleted = true;
                    booking.b_updatedDate=DateTime.UtcNow;


                    _dbcontext.Bookings.Attach(booking);
                    _dbcontext.Entry(booking).Property(x => x.b_updatedDate).IsModified = true;
                    _dbcontext.Entry(booking).Property(x => x.is_deleted).IsModified = true;


                    _dbcontext.SaveChanges();

                    success = true;
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;

            }

            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
        }


        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin, UserPermessionLevelEnum.user)]
        public ActionResult ServeBookingStatus(int transactionNo)
        {
            var booking = _dbcontext.Bookings.FirstOrDefault(x => x.trn_Id == transactionNo);
            bool success = false;

            var datenow = DateTime.Now;

            try
            {
                if (booking != null)
                {
                    if (booking.startdate <= datenow)
                    {
                        booking.serve_stat = true;
                        booking.b_updatedDate=DateTime.UtcNow;

                        _dbcontext.Bookings.Attach(booking);
                        _dbcontext.Entry(booking).Property(x => x.b_updatedDate).IsModified = true;
                        _dbcontext.Entry(booking).Property(x => x.serve_stat).IsModified = true;
                        _dbcontext.SaveChanges();

                        success = true;
                    }
                    else
                    {
                        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                    }

                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
        }


        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin)]
        [HttpGet]
        public ActionResult CancelBooking(int transId)
        {

            var cancelledbook = cancelledBooking.GetCancelledBooking(transId);

            return View(cancelledbook);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveCancelledBooking(CancelBookingViewModel cancelBookingView)
        {

            if (!ModelState.IsValid)
            {
                return View("CancelBooking", cancelBookingView);
            }



            var newcancelledBooking = new CancelledBooking()
            {

                trn_Id = cancelBookingView.TransId,
                cancelledDated = cancelBookingView.CancelDate,
                reasoncancelled = cancelBookingView.ReasonforCancel,
                isrefundable = cancelBookingView.isRefundable ? true : false

            };

            _dbcontext.CancelledBookings.Add(newcancelledBooking);
            //_dbcontext.SaveChanges();

            //set booking iscancelled stat to true;


            var booking = _dbcontext.Bookings.FirstOrDefault(x => x.trn_Id == cancelBookingView.TransId);

            if (booking != null)
            {
                booking.is_cancelled = true;

                //_dbcontext.Bookings.Attach(booking);
                _dbcontext.Entry(booking).Property(x => x.is_cancelled).IsModified = true;



            }

            _dbcontext.SaveChanges();

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult PrintContractOption(int transId) => PartialView("_PrintContractOptionPartial",new PrintOptionViewModel() {Id = transId });


        public ActionResult PrintContract(int transId, string selprintopt)
        {
            var pOption = new PrintOptionViewModel()
            {
                Id = transId,
                selPrintOpt = selprintopt

            };


            return View("~/Views/Shared/ReportContainer.cshtml", pOption);

        }

        public ActionResult PrintDistribution(int transId)
        {
            var pOption = new PrintOptionViewModel()
            {
                Id = transId,
                selPrintOpt = "distribution"

            };


            return View("~/Views/Shared/ReportContainer.cshtml", pOption);
        }


       
        [HttpGet]
        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin)]
        public ActionResult AddBookingDiscount(int transactionId)
        {
            DiscountCodeViewModel dc = new DiscountCodeViewModel();

            ViewBag.HeadTitle = "Create New Discount Code";

            var discountView = new DiscountCodeViewModel()
            {
                transId = transactionId,
                DiscountSelectlist = dc.GetListofDiscount()
            };

            return PartialView("_AddBookingDiscount", discountView);
        }


        [HttpPost, ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult SaveBookingDiscount(DiscountCodeViewModel newdiscount)
        {

            if (!ModelState.IsValid) return PartialView("_AddBookingDiscount", newdiscount);

            string url;
            try
            {
                var bookDiscount = new Book_Discount()
                {
                    trn_Id = newdiscount.transId,
                    disc_Id = newdiscount.discountCode,
                    userid = User.Identity.GetUserId()

                };

                _dbcontext.Book_Discount.Add(bookDiscount);
                _dbcontext.SaveChanges();

                url = @Url.Action("getPartialView_AmountDue", "Bookings", new { transId = newdiscount.transId });

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }



            return Json(new { success = true, url = url }, JsonRequestBehavior.AllowGet);
        }


        //Remove Booking Discount
        [HttpPost]
        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin)]
        public ActionResult RemoveBookingDiscount(int transId)
        {
            string url = string.Empty;

            bool success = false;

            var bookDiscount = _dbcontext.Book_Discount.FirstOrDefault(x => x.trn_Id == transId);

            try
            {

                if (bookDiscount != null)
                {
                    _dbcontext.Book_Discount.Remove(bookDiscount);
                    _dbcontext.SaveChanges();

                    success = true;
                }


                url = @Url.Action("getPartialView_AmountDue", "Bookings", new { transId = transId });


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return Json(new { success = success, url = url }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetDiscountDetails(int discountId)
        {

            var discountdetails = (from d in _dbcontext.Discounts
                                   where d.disc_Id == discountId
                                   select new
                                   {
                                       discType = d.disctype,
                                       discountAmount = d.discount1,
                                       discStart = d.discStartdate,
                                       discEnd = d.discEnddate
                                   }).FirstOrDefault();

            return Json(new { discountdetails }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BookReservation(int reservationId)
        {
            var reservationinfo = _dbcontext.Reservations.Find(reservationId);

            var customer = _dbcontext.Customers.FirstOrDefault(x => x.c_Id == reservationinfo.c_Id);
            BookingsViewModel bookingviewModel = new BookingsViewModel();

            if (reservationinfo != null)
            {
                bookingviewModel = new BookingsViewModel()
                {
                    transdate = DateTime.Now,
                    c_Id = reservationinfo.c_Id,
                    noofperson = reservationinfo.noofPax,
                    occasion = reservationinfo.occasion,
                    venue = reservationinfo.eventVenue,
                    startdate = reservationinfo.resDate,
                    enddate = reservationinfo.resDate,
                    serve_status = false,
                    fullname = Utilities.Getfullname(customer.lastname, customer.firstname, customer.middle),
                    reservationId = reservationId,
                    Servicetype_ListItems = booking.GetServiceType_SelectListItems()
                };

            }


            return View(bookingviewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookReservation(BookingsViewModel bookingViewModel)
        {

            if (!ModelState.IsValid) return View(bookingViewModel);

            bool success = false;

            //  _dbcontext.Configuration.ProxyCreationEnabled = false;

            int transactionId = 0;

            try
            {

                var newBooking = new Booking
                {
                    c_Id = bookingViewModel.c_Id,
                    noofperson = bookingViewModel.noofperson,
                    occasion = bookingViewModel.occasion,
                    venue = bookingViewModel.venue,
                    typeofservice = bookingViewModel.serviceId ?? 2,
                    startdate = bookingViewModel.startdate,
                    enddate = bookingViewModel.startdate,
                    transdate = bookingViewModel.transdate,
                    p_id = bookingViewModel.pId,
                    apply_extendedAmount = bookingViewModel.apply_extendedAmount,
                    serve_stat = false


                };
                _dbcontext.Bookings.Add(newBooking);
                _dbcontext.SaveChanges();

                transactionId = newBooking.trn_Id;
                var _reservationId = bookingViewModel.reservationId;

                var reservation = new Reservation();
                reservation = _dbcontext.Reservations.Find(_reservationId);


                if (reservation != null)
                {
                    reservation.reserveStat = true;

                    _dbcontext.Reservations.Attach(reservation);
                    _dbcontext.Entry(reservation).Property(x => x.reserveStat).IsModified = true;
                    _dbcontext.SaveChanges();
                }



                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return Json(new { success = success, trnsId = transactionId }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin)]
        public ActionResult RestoreServedBooking(int transactionId)
        {
            var booking = _dbcontext.Bookings.FirstOrDefault(x => x.trn_Id == transactionId);
            bool success = false;

            //var datenow = DateTime.Now;

            try
            {
                if (booking != null)
                {
                    booking.serve_stat = false;

                    _dbcontext.Bookings.Attach(booking);
                    _dbcontext.Entry(booking).Property(x => x.serve_stat).IsModified = true;
                    _dbcontext.SaveChanges();

                    success = true;

                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin)]
        public ActionResult RestoreCancelledBooking(int transactionId)
        {
            var booking = _dbcontext.Bookings.FirstOrDefault(x => x.trn_Id == transactionId);
            var cancelledbooking = _dbcontext.CancelledBookings.FirstOrDefault(x => x.trn_Id == transactionId);

            bool success = false;

            //var datenow = DateTime.Now;

            try
            {
                if (booking != null)
                {
                    booking.is_cancelled = false;

                    _dbcontext.Bookings.Attach(booking);
                    _dbcontext.Entry(booking).Property(x => x.is_cancelled).IsModified = true;

                    if (cancelledbooking != null)
                    {

                        _dbcontext.CancelledBookings.Remove(cancelledbooking);
                    }

                    _dbcontext.SaveChanges();

                    success = true;

                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get_AddonsandUpgrades(int bookaddonNo)
        {

            return PartialView("_Get_AddonsandUpgrades",new AddonsUpgrade_BookRegisterViewModel
            {
                bookaddon_No = bookaddonNo,
                selectlistAddonCat = addupgradereg.Get_SelectListAddonCat()
               
            });
        }

        public ActionResult Get_AddonsandUpgradesByCat(int addonCatId)
        {

            var listofaddons = addonsviewmodel.GetListofAddonDetails().Where(catid => catid.addoncatId == addonCatId).ToList();

            return Json(new { data = listofaddons }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSelectedAddons(int selectedaddonId, int addonNo)
        {

            var seladdonsviewmodel = new SelectedAddonsViewModel();
            var addonDetail = _dbcontext.AddonDetails.Find(selectedaddonId);
            var bookingAddon = _dbcontext.BookingAddons.FirstOrDefault(t => t.bookaddonNo == addonNo);

            if (addonDetail != null)
            {
                seladdonsviewmodel = (from a in _dbcontext.AddonDetails
                    join b in _dbcontext.AddonCategories on a.addoncatId equals b.addoncatId
                    where a.addonId == selectedaddonId
                    select new SelectedAddonsViewModel
                    {
                        bookaddon_No = addonNo,
                        addon_Id = selectedaddonId,
                        booking_No =
                            (int) _dbcontext.BookingAddons.FirstOrDefault(t => t.bookaddonNo == addonNo).trn_Id,
                        addon_details = a.addondescription,
                        unit = a.unit,
                        amount = (decimal)a.amount,

                        orderQty =_dbcontext.Bookings.FirstOrDefault(t=>t.trn_Id==bookingAddon.trn_Id).noofperson??0
                                   

                    }).Single();


            }


            return PartialView(seladdonsviewmodel);
        }


        [HttpGet]
        public ActionResult GetAddOnsDetails(int addon_No)
        {
            return PartialView("_GetAddOnsDetails",new BookingAddonsViewModel()
            {
                bookaddon_No = addon_No,

                BookingAddon = _dbcontext.BookingAddons.FirstOrDefault(t => t.bookaddonNo == addon_No),


                BookAddOn_Details = _dbcontext.BookAddonsDetails.Where(t => t.bookaddonNo == addon_No).Select(t => new BookingAddonDetailsViewModel()
                {
                    addondetail_Id = t.addondetail_Id,
                    bookaddon_No = (int)t.bookaddonNo,
                    addon_Id = (int)t.addonId,
                    addon_description = _dbcontext.AddonDetails.FirstOrDefault(a=>a.addonId==t.addonId).addondescription,
                    addon_qty = (decimal)t.qty,
                    addon_amount = (decimal)t.amount
                }).ToList()

            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSelectedAddon(SelectedAddonsViewModel selectedaddon)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("GetSelectedAddons", selectedaddon);
            }

            var success = false;

         

            var is_addonsexist = _dbcontext.BookAddonsDetails.Any(x => x.addonId == selectedaddon.addon_Id 
                                 && x.bookaddonNo == selectedaddon.bookaddon_No);

            if (is_addonsexist == false)
            {

                var addondetails = new BookAddonsDetail
                        {
                            bookaddonNo = selectedaddon.bookaddon_No,
                            addonId = selectedaddon.addon_Id,
                            qty = selectedaddon.orderQty,
                            amount = selectedaddon.amount
                        };

                _dbcontext.BookAddonsDetails.Add(addondetails);
                _dbcontext.SaveChanges();

                ModelState.Clear();

                success = true;
              

            }

            var ReturnUrl = new[]
            {
                Url.Action("GetAddOnsDetails", "Bookings", new { addon_No = selectedaddon.bookaddon_No }),
                Url.Action("GetListofAddons", "Bookings", new { transId = selectedaddon.booking_No })
            };

            return Json(new {success, url = ReturnUrl}, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult ModifyAddonDetail(int selAddonDetailId)
        {
            BookAddonSelectedViewModel seladdonsviewmodel=null;

            var addonDetail = _dbcontext.BookAddonsDetails.FirstOrDefault(t=>t.addondetail_Id==selAddonDetailId);

            if (addonDetail != null)
            {
                seladdonsviewmodel = new BookAddonSelectedViewModel()
                {
                    BookingAddonsDetail = addonDetail,
                    SelectedBookAddonsDetail = (from a in _dbcontext.AddonDetails where a.addonId==addonDetail.addonId select new SelectedAddonsViewModel()
                    {
                        addondetail_No = addonDetail.addondetail_Id,
                        booking_No = (int)addonDetail.BookingAddon.trn_Id,
                        bookaddon_No = (int) addonDetail.bookaddonNo,
                        addon_Id = a.addonId,
                        addon_details = a.addondescription,
                        unit = a.unit,
                        amount = (decimal)addonDetail.amount,
                        orderQty = (decimal)addonDetail.qty
                    }).Single()
                    //SelectedBookAddonsDetail =(from bkc in _dbcontext.AddonDetails where bkc.addonId == addonDetail.addonId select bkc).Single()
                };
            }


            return PartialView("_ModifyAddonDetail", seladdonsviewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSelectedAddonDetail(BookAddonSelectedViewModel UpdatedUpdateSelectedAddonDetail)
        {
            if (!ModelState.IsValid) return PartialView("_ModifyAddonDetail", UpdatedUpdateSelectedAddonDetail);


            var success = false;

            var selectedaddonDetail = UpdatedUpdateSelectedAddonDetail.SelectedBookAddonsDetail;

            try
            {
               
                var addondetails = new BookAddonsDetail
                {
                    addondetail_Id = selectedaddonDetail.addondetail_No,
                    addonId = UpdatedUpdateSelectedAddonDetail.BookingAddonsDetail.addonId,
                    bookaddonNo = UpdatedUpdateSelectedAddonDetail.BookingAddonsDetail.bookaddonNo,
                    qty = selectedaddonDetail.orderQty,
                    amount = selectedaddonDetail.amount
                };

                _dbcontext.BookAddonsDetails.Attach(addondetails);
                _dbcontext.Entry(addondetails).State = EntityState.Modified;
                _dbcontext.SaveChanges();

                ModelState.Clear();

                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
           

            var ReturnUrl = new[]
            {
                Url.Action("GetAddOnsDetails", "Bookings", new { addon_No =UpdatedUpdateSelectedAddonDetail.BookingAddonsDetail.bookaddonNo }),
                Url.Action("GetListofAddons", "Bookings", new { transId = UpdatedUpdateSelectedAddonDetail.SelectedBookAddonsDetail.booking_No })
            };

            return Json(new {success,url=ReturnUrl }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifySelectedAddon(SelectedAddonsViewModel modifyselectedaddons)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("GetSelectedAddons_Modify", modifyselectedaddons);
            }

            bool success = false;

            //var isaddonsexist =
            //    _dbcontext.BookingAddons.Any(x => x.addonId == modifyselectedaddons.addonId &&
            //                                        x.trn_Id == modifyselectedaddons.bookingNo);

            //if (isaddonsexist == true)
            //{

            //}

            var _addons = new BookingAddon
            {
                //bookaddonNo = modifyselectedaddons.addondetail_No,
                //trn_Id = modifyselectedaddons.bookingNo,
                ////addonId = modifyselectedaddons.addonId,
                //Addondesc = modifyselectedaddons.addondetails + " (" + modifyselectedaddons.unit + " )",
                Note = modifyselectedaddons.orderQty + " @ " + modifyselectedaddons.amount,
                //addonQty = modifyselectedaddons.orderQty,
                //AddonAmount = modifyselectedaddons.amount * modifyselectedaddons.orderQty
            };

            _dbcontext.BookingAddons.Attach(_addons);
            _dbcontext.Entry(_addons).State = EntityState.Modified;
            _dbcontext.SaveChanges();

            ModelState.Clear();

            success = true;

            string ReturnUrl = Url.Action("GetListofAddons", "Bookings", new { transId = modifyselectedaddons.booking_No });

            return Json(new {success, ReturnUrl }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin, UserPermessionLevelEnum.user)]
        public ActionResult RemoveAddonDetail(int addondetailId ,int trans_id)
        {
            bool success = false;
            
            var bookaddonDetail = _dbcontext.BookAddonsDetails.Find(addondetailId);
            int? bookaddon_No = null;
            try
            {

                if (bookaddonDetail != null)
                {
                    bookaddon_No = bookaddonDetail.bookaddonNo;

                    _dbcontext.BookAddonsDetails.Remove(bookaddonDetail);
                    _dbcontext.SaveChanges();

                    success = true;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            var returnUrl = new[]
            {
                Url.Action("GetAddOnsDetails", "Bookings", new {addon_No =bookaddon_No}),
                Url.Action("GetListofAddons", "Bookings", new {transId = trans_id})
            };

            return Json(new {success, url=returnUrl },JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Display Add OtherCharge Form to modal
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult BookingOtherCharge_New(int transactionId)
        {

            return PartialView("_BookingOtherCharge_New ", new BookOtherChargeDetailViewModel()
            {
                transId = transactionId
            });
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("SaveBookingOtherCharge")]
        public ActionResult BookingOtherCharge_New(BookOtherChargeDetailViewModel otherChargeDetail)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_BookingOtherCharge_New ", otherChargeDetail);
            }

            bool success = false;


            var othercharges = new Book_OtherCharge()
            {
                ch_Id = otherChargeDetail.chargeId,
                trn_Id = otherChargeDetail.transId,
                ch_desc = otherChargeDetail.chargeDesc,
                note = otherChargeDetail.chargeNote,
                amount = otherChargeDetail.chargeAmount,
                dateCreated = otherChargeDetail.chargeCreatedDate,
                dateUpdated = otherChargeDetail.chargeUpdatedDate
                
            };

            _dbcontext.Book_OtherCharge.Add(othercharges);
           int save= _dbcontext.SaveChanges();

           if (save == 1)
           {
               success = true;
           }

           var url = Url.Action("GetBookingDetailsPartial", "Bookings", new { transId = otherChargeDetail.transId });

            return Json(new {success,url }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult RemoveBookingOtherCharge(int id)
        {
            bool success = false;

            int persist = 0;

            int transId = 0;


            try
            {
                var bookOtherCharge = _dbcontext.Book_OtherCharge.Find(id);
                transId = bookOtherCharge.trn_Id;

                if (bookOtherCharge!=null)
                {
                    _dbcontext.Book_OtherCharge.Remove(bookOtherCharge);
                    persist = _dbcontext.SaveChanges();

                    if (persist == 1) success = true;


                }



            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var url = Url.Action("GetBookingDetailsPartial", "Bookings", new { transId = transId });

            return Json(new{success,url},JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ModifyBookOtherCharges(int id)
        {
            var bookothercharge_vm = new BookOtherChargeDetailViewModel();

            try
            {
                var bookOtherCharge = _dbcontext.Book_OtherCharge.Find(id);

                if (bookOtherCharge != null)
                {
                    bookothercharge_vm = new BookOtherChargeDetailViewModel()
                    {
                        chargeId = bookOtherCharge.ch_Id,
                        transId = bookOtherCharge.trn_Id,
                        chargeDesc = bookOtherCharge.ch_desc,
                        chargeNote = bookOtherCharge.note,
                        chargeAmount = (decimal)bookOtherCharge.amount,


                    };

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return PartialView("_BookingOtherCharge_Modify", bookothercharge_vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("UpdateBookOtherCharge")]
        public ActionResult ModifyBookOtherCharges(BookOtherChargeDetailViewModel otherChargeDetail)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_BookingOtherCharge_Modify", otherChargeDetail);
            }

            bool success = false;


            var othercharges = new Book_OtherCharge()
            {
                ch_Id = otherChargeDetail.chargeId,
                trn_Id = otherChargeDetail.transId,
                ch_desc = otherChargeDetail.chargeDesc,
                note = otherChargeDetail.chargeNote,
                amount = otherChargeDetail.chargeAmount,
                dateUpdated = otherChargeDetail.chargeUpdatedDate

            };

            _dbcontext.Book_OtherCharge.Attach(othercharges);
            _dbcontext.Entry(othercharges).State = EntityState.Modified;
            int saveChanges = _dbcontext.SaveChanges();

            if (saveChanges == 1)
            {
                success = true;
            }

            var url = Url.Action("GetBookingDetailsPartial", "Bookings", new { transId = otherChargeDetail.transId });

            return Json(new { success,url=url }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveSnackandDrinks(int id)
        {
            var bookmenus = _dbcontext.Book_Menus.Find(id);

            bool success = false;
            int persist = 0;

            int? _transId = null;

            try
            {
                if (bookmenus != null)
                {
                    _transId = bookmenus.trn_Id;

                    _dbcontext.Book_Menus.Remove(bookmenus);
                  persist=_dbcontext.SaveChanges();


                }

                if (persist == 1) success = true;

               
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var url = Url.Action("GetBookingDetailsPartial", "Bookings", new { transId = _transId });

            return Json(new {success =success,url=url}, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            _dbcontext.Dispose();
        }

    }
}