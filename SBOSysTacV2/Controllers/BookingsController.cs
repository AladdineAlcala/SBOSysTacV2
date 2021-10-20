using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.Models;
using SBOSysTacV2.ViewModel;

namespace SBOSysTacV2.Controllers
{
   [Authorize]
    public class BookingsController : Controller
    {

        private PegasusEntities _dbcontext;
        private BookingsViewModel booking=new BookingsViewModel();
        private PackageBookingViewModel packageBook=new PackageBookingViewModel();
        private MainMenuListViewModel mainmenulistviewmodel=new MainMenuListViewModel();
        private AddonsViewModel addonsviewmodel=new AddonsViewModel();
        private TransactionDetailsViewModel transactionDetails=new TransactionDetailsViewModel();
        private ContractReceiptViewModel cr=new ContractReceiptViewModel();
        private AddonsUpgrade_BookRegisterViewModel addupgradereg=new AddonsUpgrade_BookRegisterViewModel();
        private SelectedAddonsViewModel seladdons=new SelectedAddonsViewModel();
        private BookMenusViewModel bookMenusView=new BookMenusViewModel(); 
        private CancelBookingViewModel cancelledBooking=new CancelBookingViewModel();
       



        public BookingsController()
        {
            _dbcontext=new PegasusEntities();
        }

        // GET: Bookings
        public ActionResult Index()
        {
            ViewBag.FormTitle = "Bookings & Events Details";

            return View();
        }


        [HttpGet]
        [OutputCache(Duration =60)]
        public ActionResult LoadBookings()
        {
            var bookings = booking.GetListofBookings().OrderBy(d => d.startdate).ToList();

            return Json(new {data=bookings}, JsonRequestBehavior.AllowGet);

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

            DateTime createdDate=DateTime.Now;
            int transactionId = 0;
            decimal amountPax=0;
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
                            is_cancelled = false
                        };


                        _dbcontext.Bookings.Add(newBooking);
                        _dbcontext.SaveChanges();

                        transactionId = newBooking.trn_Id;

                        success = true;

                        return Json(new {success = success, trnsId = transactionId}, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                }

             


            }


            //bookingViewModel.Servicetype_ListItems = booking.GetServiceType_SelectListItems();

            return Json(new { success = false}, JsonRequestBehavior.AllowGet);

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

        public bool Hascustomerexist(string lname)
        {
         
            return _dbcontext.Customers.Any(x => x.lastname == lname);
        }


        public ActionResult GetPackageId(int? transactionId)
        {
            var booking=new Booking();
            var is_success = false;

            booking = _dbcontext.Bookings.Find(transactionId);

            if (booking.p_id != null)
            {
                is_success = true;
            }

            return Json(new {success= is_success }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult GetPackageonBooking(int? transId)
        {
             PackageBookingViewModel pBooking=new PackageBookingViewModel();
             BookMenusViewModel bm = new BookMenusViewModel();

             pBooking = packageBook.GetBookingDetails().FirstOrDefault(x => x.transactionId==transId);


            return View(pBooking);
        }


        [HttpGet]
        public ActionResult GetPackageBookingDetails(int transId)
        {
            TransactionDetailsViewModel _transDetails = new TransactionDetailsViewModel();
            try
            {

                _transDetails = transactionDetails.GetTransactionDetails().FirstOrDefault(x => x.transactionId.Equals(transId));

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return View(_transDetails);
        }

        [HttpGet]
        public ActionResult getPartialView_AmountDue(int transId)
        {
            List<BookingAddon> addonslist = new List<BookingAddon>();
            TransactionDetailsViewModel _transDetails = new TransactionDetailsViewModel();

            try
            {
                //var transId = transModel.transactionId;
                _transDetails = transactionDetails.GetTransactionDetails().FirstOrDefault(x => x.transactionId.Equals(transId));

               

                decimal packageTotal = 0;
                decimal addonsTotal = 0;
                decimal belowminPax = 0;
                decimal extendedLocationAmount = 0;
                decimal dpAmount = 0;
                decimal fpAmount = 0;
                decimal bookdiscountAmount = 0;
                decimal cateringdiscountAmount = 0;
                string bookdiscountCode = string.Empty;

                var packageAmount = _transDetails.Package_Trans.p_amountPax;
                var packageType = _transDetails.Package_Trans.p_type;
                int no_of_pax = Convert.ToInt32(_transDetails.Booking_Trans.noofperson);


                addonslist = _dbcontext.BookingAddons.Where(x => x.trn_Id == transId).ToList();
                addonsTotal = addonslist.Sum(y => Convert.ToDecimal(y.AddonAmount));


                if (_transDetails.Booking_Trans.apply_extendedAmount) // check if location extended charge is true otherwise extended location will be zero value
                {
                    extendedLocationAmount = transactionDetails.Get_extendedAmountLoc(transId);
                }
              

                //belowminPax = packageType.Trim() == "vip" ? 0 : transactionDetails.GetBelowMinPaxAmount(no_of_pax);

                dpAmount = transactionDetails.GetTotalDownPayment(transId);
                fpAmount = transactionDetails.GetFullPayment(transId);
                cateringdiscountAmount = packageType.Trim() == "vip" ? 0 : transactionDetails.getCateringdiscount(no_of_pax);
             
                //var cateringTotalAmount=cateringdiscountAmount * no_of_pax;
                packageTotal = Convert.ToDecimal(packageAmount) * no_of_pax;

                var subtotal = (packageTotal + addonsTotal + extendedLocationAmount + belowminPax);

                //get discount information
                bookdiscountAmount = transactionDetails.Get_bookingDiscountbyTrans(transId, subtotal);
                bookdiscountCode = transactionDetails.Get_bookingDiscounDetailstbyTrans(transId);

                _transDetails.TotaAddons = addonsTotal;
                _transDetails.extLocAmount = extendedLocationAmount;
                //_transDetails.TotaBelowMinPax = belowminPax;
                _transDetails.TotaDp = dpAmount;
                _transDetails.Fullpaymnt = fpAmount;
                _transDetails.book_discounts = bookdiscountAmount;
                _transDetails.bookdiscountdetails = bookdiscountCode;
                _transDetails.cateringdiscount = cateringdiscountAmount;


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return PartialView("_BookingsAmountDuePartial", _transDetails);
        }



        [HttpGet]
        public ActionResult GetListofBookMenus(int transactionId)
        {
            PackageBookingViewModel pBooking = new PackageBookingViewModel();
            BookMenusViewModel bm = new BookMenusViewModel();

            pBooking.BookMenuses = bm.LisofMenusBook(transactionId).ToList();
            pBooking.transactionId = transactionId;


            return PartialView("_ListofBookMenus",pBooking);
        }




        [HttpGet]
        public ActionResult GetListofAddons(int transId)
        {

            // ViewBag.transId = transid;

            PackageBookingViewModel pBooking = new PackageBookingViewModel();
            AddonsViewModel addonsViewModel=new AddonsViewModel();

            pBooking.AddOns = addonsViewModel.ListofAddons().Where(x => x.TransId.Equals(transId)).OrderBy(o=>o.No).ToList();

            pBooking.transactionId = transId;


            return PartialView("_GetListofAddons",pBooking);

        }
        
        public ActionResult GetListofCourse(int transactionId)
        {
            BookMenusViewModel bookMenus=new BookMenusViewModel();

            bookMenus.transId = transactionId;

            return PartialView("_GetListofMainCourse", bookMenus);
        }

        //[ActionName("LoadCustomerByBookNo")]
        public ActionResult GetListofCourseforChange(int bookmenuNo)
        {

           var  bookMenus = (from bm in _dbcontext.Book_Menus
                join m in _dbcontext.Menus on bm.menuid equals m.menuid
                select new BookMenusViewModel()
                {
                    transId = (int) bm.trn_Id,
                    menuId = bm.menuid,
                    menu_name = m.menu_name,
                    menu_No = bm.No,
                    serving = (decimal) bm.serving

                }).FirstOrDefault(x => x.menu_No==bookmenuNo);

            return PartialView("_GetListofMainCourse_Change",bookMenus);
        }


        public ActionResult LoadListMenus()
        {
            var listofmainmenu = mainmenulistviewmodel.ListofMainMenu().ToList();

            return Json(new {data=listofmainmenu }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult AddMenusToPackage(BookMenusViewModel bookmenus)
        {
            bool isRecordExist = false;
            string _message = "";
            string menu_details = "";

            dynamic ShowErrMessageString = String.Empty;

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


                    if (isExistMenu==null)
                    {
                        TransactionDetailsViewModel td = new TransactionDetailsViewModel();

                        //=====check if selected course is a main menu
                        if (td.isSelectedMenuMainCourse(bookmenus.menuId) == true)
                        {
                                        var bookings = _dbcontext.Bookings.Find(bookmenus.transId);
                                        if (bookmenus.get_totalselectedMainMenus(bookmenus.transId) <
                                            bookMenusView.Get_PackageMainMenusInt(Convert.ToInt32(bookings.p_id)))
                                        {

                                            var bookMenu = new Book_Menus()
                                            {
                                                trn_Id = bookmenus.transId,
                                                menuid = bookmenus.menuId,
                                                serving = bookmenus.serving
                                            };

                                            _dbcontext.Book_Menus.Add(bookMenu);
                                            _dbcontext.SaveChanges();


                                            url = Url.Action("GetListofBookMenus", "Bookings",
                                                new {transactionId = bookMenu.trn_Id});
                                        }

                                        else // exceed on total number count of main menu
                                        {
                                            isRecordExist = true;
                                            _message = menu_details + "  already exceed on maximum main menu count";

                                            ShowErrMessageString = new
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
                                            serving = bookmenus.serving
                                        };

                                        _dbcontext.Book_Menus.Add(bookMenu);
                                        _dbcontext.SaveChanges();


                                        url = Url.Action("GetListofBookMenus", "Bookings", new { transactionId = bookMenu.trn_Id });
                        }


                    }
                    else
                    {
                        isRecordExist = true;
                        _message = menu_details + " already in the list";

                        ShowErrMessageString = new
                        {
                            param1 = 404,
                            param2 = _message
                        };
                    }

                }
                catch (Exception)
                {

                    throw;
                }


                return Json(new { isRecordExist = isRecordExist, ShowErrMessageString, url = url }, JsonRequestBehavior.AllowGet);
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
                   

                  
                    //var url = "";

                    TransactionDetailsViewModel td = new TransactionDetailsViewModel();
                    Book_Menus isExistMenu = _dbcontext.Book_Menus.AsNoTracking().FirstOrDefault(x => x.trn_Id == modifiedBookMenu.transId && x.menuid == modifiedBookMenu.menuId);

                var menu = _dbcontext.Menus.FirstOrDefault(x => x.menuid == modifiedBookMenu.menuId);

                    if (menu != null)
                    {
                        menu_details = menu.menu_name;
                    }


                    var bookMenus = _dbcontext.Book_Menus.FirstOrDefault(x => x.No == modifiedBookMenu.menu_No);

                    if (bookMenus != null)
                    {
                        //bookMenus.No = modifiedBookMenu.menu_No;
                        bookMenus.trn_Id = modifiedBookMenu.transId;
                        bookMenus.menuid = modifiedBookMenu.menuId;
                        bookMenus.serving = modifiedBookMenu.serving;
                    }


                if (isExistMenu != null) //menu selected was the same 
                        {

                                //check serving if not exceed to no of pax

                                        var noofPax = _dbcontext.Bookings.FirstOrDefault(x => x.trn_Id == modifiedBookMenu.transId) .noofperson;

                                        if (isExistMenu.serving != 0)
                                        {

                                                                        if (isExistMenu.serving < noofPax)
                                                                        {
                                                                                //_dbcontext.Book_Menus.Attach(modifiedbookMenu);
                                                                                //_dbcontext.Entry(modifiedbookMenu).State = EntityState.Modified;
                                                                                //_dbcontext.SaveChanges();


                                                                                url = Url.Action("GetListofBookMenus", "Bookings", new { transactionId = modifiedBookMenu.transId });

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

                                                //_dbcontext.Book_Menus.Attach(modifiedbookMenu);
                                                //_dbcontext.Entry(modifiedbookMenu).State = EntityState.Modified;
                                                _dbcontext.SaveChanges();


                                                url = Url.Action("GetListofBookMenus", "Bookings", new { transactionId = modifiedBookMenu.transId });

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

                        // _dbcontext.Book_Menus.Attach(modifiedbookMenu);
                        //_dbcontext.Entry(modifiedbookMenu).State = EntityState.Modified;
                        _dbcontext.SaveChanges();

                        url = Url.Action("GetListofBookMenus", "Bookings", new { transactionId = modifiedBookMenu.transId});

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

            
            return Json(new {success=modifysuccess, StatMessageString,url= url }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult AddOnsInformation(int transactionId)
        {

            AddonsViewModel addons = new AddonsViewModel
            {
                TransId = transactionId
            };

            return PartialView("_AddOnsInformation",addons);
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
               AddonAmount = addons.AddonAmount
            };

            _dbcontext.BookingAddons.Add(_addons);
            _dbcontext.SaveChanges();

            ModelState.Clear();

            var ReturnUrl = Url.Action("GetListofAddons", "Bookings", new { transId=addons.TransId });

            return Json(new {success=true,url=ReturnUrl }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveMenufromListofBookings(int bookmenuNo)
        {

            bool success = true;
            var returnUrl = "";

            Book_Menus bm=new Book_Menus();

            try
            {
                bm = _dbcontext.Book_Menus.Find(bookmenuNo);

                var transactionId = bm.trn_Id;

                if (bm != null)
                {
                    _dbcontext.Book_Menus.Remove(bm);
                    _dbcontext.SaveChanges();

                returnUrl = Url.Action("GetListofBookMenus", "Bookings", new {transactionId = transactionId});

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


            return Json(new {success=success,url=returnUrl }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ModifyAddOns(int addonItemNo)
        {

            string view = string.Empty;

            var addons = addonsviewmodel.GetAddonsViewModelbyitemNo(addonItemNo);



            if (addons != null)
            {
                if (addons.addonId==null)
                {
                    view = "_ModifyAddOns";
                  

                }
                else
                {

                    var selectedaddons = seladdons.GetSelectedAddons(addonItemNo);

                    //view = "GetSelectedAddons_Modify";


                    return PartialView("GetSelectedAddons_Modify", selectedaddons);
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
                No = modifyaddons.No,
                trn_Id = modifyaddons.TransId,
                Addondesc = modifyaddons.AddonsDescription,
                Note = modifyaddons.AddonNote,
                AddonAmount = modifyaddons.AddonAmount
            };

            _dbcontext.BookingAddons.Attach(_addons);
            _dbcontext.Entry(_addons).State = EntityState.Modified;

            _dbcontext.SaveChanges();

            ModelState.Clear();

            var ReturnUrl = Url.Action("GetListofAddons", "Bookings", new { transId = modifyaddons.TransId });

            return Json(new { success = true, url = ReturnUrl }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RemoveAddons(int addonNo)
        {


            BookingAddon ba=new BookingAddon();
            int transactionId;
            string returnUrl=String.Empty;
            bool success = true;

            try
            {
                ba = _dbcontext.BookingAddons.Find(addonNo);

                if (ba!= null)
                {
                    transactionId = Convert.ToInt32(ba.trn_Id);


                    _dbcontext.BookingAddons.Remove(ba);
                    _dbcontext.SaveChanges();


                    returnUrl= Url.Action("GetListofAddons", "Bookings", new { transId = transactionId });
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


            return Json(new {success=success,url=returnUrl}, JsonRequestBehavior.AllowGet);
        }


        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin)]
        [HttpGet]
        public ActionResult EditBooking(int transId)
        {

            Booking editBooking=new Booking();
            BookingsViewModel bookingviewmodel=new BookingsViewModel();
            CustomerViewModel cus=new CustomerViewModel();

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

                bookingviewmodel.pId =Convert.ToInt32(editBooking.p_id);
                bookingviewmodel.fullname = cus.get_CustomerFullname(Convert.ToInt32(editBooking.c_Id));

                bookingviewmodel.Servicetype_ListItems = booking.GetServiceType_SelectListItems();

                bookingviewmodel.selected_servicetype = _dbcontext.ServiceTypes
                    .Where(x => x.serviceId == editBooking.typeofservice).Select(s => s.servicetypedetails).Single();

                if (editBooking.p_id != null)
                {
                    bookingviewmodel.packagename = _dbcontext.Packages.Where(x => x.p_id == editBooking.p_id)
                        .Select(p => p.p_descripton).Single();

                   
                }



                if (editBooking.Package.p_type.Trim()!="vip")
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
                bookingviewmodel.b_updatedDate=DateTime.Now;
            }

            return View(bookingviewmodel);
        }



        [HttpPost,ValidateInput(true)]
        [ValidateAntiForgeryToken]
        public ActionResult EditBooking(BookingsViewModel bookingViewModel)
        {
            if (!ModelState.IsValid) return View(bookingViewModel);
                
            DateTime createdDate=DateTime.Now;
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
                updatedBooking.serve_stat = bookingViewModel.serve_status;
                updatedBooking.is_cancelled = bookingViewModel.iscancelled;
                updatedBooking.booktype = bookingViewModel.booktypecode;
                updatedBooking.p_amount = amountPax;
                updatedBooking.reference = bookingViewModel.refernce;
                updatedBooking.b_updatedDate = createdDate;
                updatedBooking.b_createdbyUser = User.Identity.GetUserId();
            }

          



            try
            {
               
                _dbcontext.SaveChanges();


                success = true;

                return Json(new { success = success, trnsId= updatedBooking.trn_Id}, JsonRequestBehavior.AllowGet);

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

            Booking booking=new Booking();
            Book_Discount bk = new Book_Discount();

            BookingAddon bookaddons=new BookingAddon();

            booking = _dbcontext.Bookings.Find(transId);
            IEnumerable<Book_Menus> bookmenustransList = _dbcontext.Book_Menus.Where(x => x.trn_Id == transId);
            IEnumerable<BookingAddon> bookingAddons = _dbcontext.BookingAddons.Where(x => x.trn_Id == transId);
            IEnumerable<Payment> paymentlist = _dbcontext.Payments.Where(x => x.trn_Id == transId);
            bk = _dbcontext.Book_Discount.Find(transId);


            try
            {
                if (booking != null)
                {
                    _dbcontext.Bookings.Remove(booking);

                    _dbcontext.Book_Menus.RemoveRange(bookmenustransList);
                    _dbcontext.BookingAddons.RemoveRange(bookingAddons);
                    _dbcontext.Payments.RemoveRange(paymentlist);

                    if (bk != null)
                    {
                        _dbcontext.Book_Discount.Remove(bk);
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

            return Json(new {success = success}, JsonRequestBehavior.AllowGet);
        }


        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin, UserPermessionLevelEnum.user)]
        public ActionResult ServeBookingStatus(int transactionNo)
        {
            var booking = _dbcontext.Bookings.FirstOrDefault(x => x.trn_Id == transactionNo);
            bool success = false;

            var datenow = DateTime.Now;

            try
            {
                if (booking!=null)
                {
                    if (booking.startdate <= datenow)
                    {
                        booking.serve_stat = true;

                        _dbcontext.Bookings.Attach(booking);
                        _dbcontext.Entry(booking).Property(x => x.serve_stat).IsModified = true;
                        _dbcontext.SaveChanges();

                        success = true;
                    }
                    else
                    {
                        return Json(new {success = false},JsonRequestBehavior.AllowGet);
                    }

                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return Json(new {success=success}, JsonRequestBehavior.AllowGet);
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
                isrefundable = cancelBookingView.isRefundable?true:false

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

            return Json(new {success=true}, JsonRequestBehavior.AllowGet);

        }

        public ActionResult PrintContractOption(int transId)
        {
            var p = new PrintOptionViewModel()
            {
                Id = transId,
            };
            return View(p);

        }

        public ActionResult PrintContract(int transId, string selprintopt)
        {
            var pOption = new PrintOptionViewModel()
            {
                Id = transId,
                selPrintOpt = selprintopt

            };
            
            //var contractReciept=new ContractReceiptViewModel();

            //contractReciept = cr.getContractReciept(pOption.Id);

            //return View("~/Views/Bookings/PrintContractForm.cshtml", contractReciept);

            return View("~/Views/Shared/ReportContainer.cshtml", pOption);

        }

        public ActionResult PrintDistribution(int transId)
        {
            var pOption = new PrintOptionViewModel()
            {
                Id = transId,
                selPrintOpt ="distribution"

            };


            return View("~/Views/Shared/ReportContainer.cshtml",pOption);
        }


        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin)]
        [HttpGet]
        public ActionResult AddBookingDiscount(int transactionId)
        {
            DiscountCodeViewModel dc = new DiscountCodeViewModel();

            ViewBag.HeadTitle = "Create New Discount Code";

            var discountView = new DiscountCodeViewModel()
            {
                transId = transactionId,
                DiscountSelectlist =dc.GetListofDiscount()
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

                url = @Url.Action("getPartialView_AmountDue", "Bookings", new {transId = newdiscount.transId});

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

              
           
            return Json(new {success=true,url=url }, JsonRequestBehavior.AllowGet);
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

           
            return Json(new {success=success,url=url}, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetDiscountDetails(int discountId)
        {

            var discountdetails = (from d in _dbcontext.Discounts where d.disc_Id==discountId
                select new
                {
                    discType=d.disctype,
                    discountAmount=d.discount1,
                    discStart=d.discStartdate,
                    discEnd=d.discEnddate
                }).FirstOrDefault();

            return Json(new {discountdetails}, JsonRequestBehavior.AllowGet);
        }

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
                    fullname = Utilities.getfullname(customer.lastname,customer.firstname,customer.middle),
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

                var reservation=new Reservation();
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

            return Json(new {success= success }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get_AddonsandUpgrades(int transactionId)
        {

            var addonsupgradesbookregister =
                new AddonsUpgrade_BookRegisterViewModel
                {
                    selectlistAddonCat = addupgradereg.Get_SelectListAddonCat(),
                    BookTransId = transactionId
                };


            return PartialView(addonsupgradesbookregister); 
        }

        public ActionResult Get_AddonsandUpgradesByCat(int addonCatId)
        {

            var listofaddons = addonsviewmodel.GetListofAddonDetails().Where(catid => catid.addoncatId == addonCatId) .ToList();

            return Json(new {data= listofaddons}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSelectedAddons(int selectedaddonId,int bookId)
        {

            var seladdonsviewmodel = new SelectedAddonsViewModel();
            var addons = _dbcontext.AddonDetails.Find(selectedaddonId);

            if (addons != null)
            {
                seladdonsviewmodel = new SelectedAddonsViewModel()
                {
                    addonId = addons.addonId,
                    bookingNo = bookId,
                    addondetails = addons.addondescription,
                    unit = addons.unit,
                    amount =  (decimal) addons.amount
                };
            }


            return PartialView(seladdonsviewmodel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveSelectedAddon(SelectedAddonsViewModel selectedaddons)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("GetSelectedAddons", selectedaddons);
            }

            var success = false;
            var ReturnUrl = string.Empty;

           var isaddonsexist =
                _dbcontext.BookingAddons.Any(x => x.addonId == selectedaddons.addonId &&
                                                  x.trn_Id == selectedaddons.bookingNo);

            if (isaddonsexist == false)
            {
                var _addons = new BookingAddon
                {
                    trn_Id = selectedaddons.bookingNo,
                    addonId = selectedaddons.addonId,
                    Addondesc = selectedaddons.addondetails + " (" + selectedaddons.unit + " )",
                    Note = selectedaddons.orderQty + " @ " + selectedaddons.amount,
                    addonQty = selectedaddons.orderQty,
                    AddonAmount = selectedaddons.amount * selectedaddons.orderQty
                };

                _dbcontext.BookingAddons.Add(_addons);
                _dbcontext.SaveChanges();

                ModelState.Clear();

                success = true;
                ReturnUrl = Url.Action("GetListofAddons", "Bookings", new { transId = selectedaddons.bookingNo });

            }

            

            return Json(new { success = success, url = ReturnUrl }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifySelectedAddon(SelectedAddonsViewModel modifyselectedaddons)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("GetSelectedAddons_Modify", modifyselectedaddons);
            }

            var success = false;
            var ReturnUrl = string.Empty;

            //var isaddonsexist =
            //    _dbcontext.BookingAddons.Any(x => x.addonId == modifyselectedaddons.addonId &&
            //                                        x.trn_Id == modifyselectedaddons.bookingNo);

            //if (isaddonsexist == true)
            //{

            //}

            var _addons = new BookingAddon
            {
                No = modifyselectedaddons.No,
                trn_Id = modifyselectedaddons.bookingNo,
                addonId = modifyselectedaddons.addonId,
                Addondesc = modifyselectedaddons.addondetails + " (" + modifyselectedaddons.unit + " )",
                Note = modifyselectedaddons.orderQty + " @ " + modifyselectedaddons.amount,
                addonQty = modifyselectedaddons.orderQty,
                AddonAmount = modifyselectedaddons.amount * modifyselectedaddons.orderQty
            };

            _dbcontext.BookingAddons.Attach(_addons);
            _dbcontext.Entry(_addons).State = EntityState.Modified;
            _dbcontext.SaveChanges();

            ModelState.Clear();

            success = true;
            ReturnUrl = Url.Action("GetListofAddons", "Bookings", new { transId = modifyselectedaddons.bookingNo });




            return Json(new { success = success, url = ReturnUrl }, JsonRequestBehavior.AllowGet);

        }

        protected override void Dispose(bool disposing)
        {
            _dbcontext.Dispose();
        }

    }
}