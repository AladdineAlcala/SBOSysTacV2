using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using SBOSysTacV2.Models;
using SBOSysTacV2.ViewModel;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.ServiceLayer;

namespace SBOSysTacV2.Controllers
{
    [Authorize]
    public class InquiryController : Controller
    {
        // GET: Inquiry

        private PegasusEntities _dbEntities;
        private CustomerBookingsViewModel cb;
        private BookingPaymentsViewModel bookingPayments;
        private readonly TransactionDetailsViewModel transactionDetails;
        private TransRecievablesViewModel tr;
        private PackageBookingViewModel packageBook;
        private readonly IncentivesService incentivesService;
        static Func<int, decimal> _getBookingAmount = BookingsService.Get_TotalAmountBook;

        public InquiryController()
        {
            _dbEntities=new PegasusEntities();

            incentivesService = new IncentivesService();
            cb = new CustomerBookingsViewModel();
            bookingPayments = new BookingPaymentsViewModel();
            transactionDetails = new TransactionDetailsViewModel();
            tr = new TransRecievablesViewModel();
            packageBook = new PackageBookingViewModel();
        }


        [HttpGet]
        public ActionResult CustomerBookingHistory() => View();


        [HttpGet]
        public ActionResult GetCustomerBookings() => PartialView("CustomerBookingsPartialView");


        [HttpGet]
       // [ActionName("LoadBookingsByCustomer")]
        public ActionResult LoadBookingsByCustomer(int cusId)
        {
            List<CustomerBookingsViewModel> cusbookings = new List<CustomerBookingsViewModel>();

            try
            {
                cusbookings = cb.GetCusBookingsById(cusId).ToList();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            //Thread.Sleep(3000);

            return Json(new { data = cusbookings }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BookingScheduleIndex() => View();

        [HttpGet]
        public ActionResult GetBookingScheduleFilter()=> PartialView("GetBookingScheduleFilter_Partial");


        public ActionResult GetBookingScheduleDataTableList(DateTime startDate, DateTime endDate,string filter)
        {

            List<CustomerBookingsViewModel> cusbooking_Schedule = new List<CustomerBookingsViewModel>();

            try
            {

                List<CustomerBookingsViewModel> lst = new List<CustomerBookingsViewModel>();
                List<Booking> listbookings = new List<Booking>();
                listbookings = _dbEntities.Bookings.Where(
                    b => DbFunctions.TruncateTime(b.startdate) >= DbFunctions.TruncateTime(startDate) &&
                         DbFunctions.TruncateTime(b.startdate) <= DbFunctions.TruncateTime(endDate)).ToList();

                lst = (from l in listbookings
                    select new CustomerBookingsViewModel()
                    {
                        transId = l.trn_Id,
                        cusId = l.c_Id,
                        cusfullname = Utilities.Getfullname_nonreverse(l.Customer.lastname, l.Customer.firstname, l.Customer.middle),
                        occasion = l.occasion,
                        venue = l.venue,
                        bookdatetime = l.startdate,
                        package = l.Package.p_descripton,
                        packageDue = _getBookingAmount(l.trn_Id),
                        isServe = Convert.ToBoolean(l.serve_stat)

                    }).ToList();

                if (filter == "serve")
                {
                    cusbooking_Schedule = (from c in lst where c.isServe==true select c).ToList();

                    
                }
                else if (filter == "unserve")
                {
                    cusbooking_Schedule = (from c in lst where c.isServe == false select c).ToList();
                }
                else
                {
                    cusbooking_Schedule = (from c in lst select c).ToList();
                }


            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                throw;
            }
            Thread.Sleep(3000);

           

            return Json(new {data= cusbooking_Schedule}, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AccountsRecievableReportIndex()
        {
            return View();
        }

        public ActionResult SalesSummaryIndex()
        {
            return View();
        }

        public ActionResult RecievableprintOption(int? cusId,string selprintopt)
        {
            string url = null;
            var dbcontext = new PegasusEntities();

            var pOption = new PrintOptionViewModel()
            {
                Id = Convert.ToInt32(cusId),
                selPrintOpt = selprintopt,
                Customer = (from c in dbcontext.Customers select c).FirstOrDefault(c=>c.c_Id==cusId)
            };

            if (selprintopt == "accnrecievesummary")
            {
                url = "~/Views/Shared/ReportContainer.cshtml";

               
            }
            else
            {
                ViewBag.CusId = pOption.Id;

                //return PartialView("_ClientRecievablesPartialView");
                url = "AccountRecieveCustomer";
            }

         
            return View(url, pOption);
        }


         
       

        public ActionResult GetSalesSummary()
        {

            return PartialView("_SalesSummaryListPartial");
        }

        public ActionResult GetSalesSummaryData()
        {
            IEnumerable<SalesSummaryViewModel> salessummarylist=new List<SalesSummaryViewModel>();
            TransactionDetailsViewModel transdetails = new TransactionDetailsViewModel();
            Func<int, decimal> _getTotalPaymentByBooking = TransactionDetailsViewModel.GetTotalPaymentByTrans;
            //var s=new SalesSummaryViewModel();

            try
            {

                IEnumerable<Booking> bookings = (from booking in _dbEntities.Bookings where booking.is_cancelled!=true
                                                 //where DbFunctions.TruncateTime(booking.transdate) >= DbFunctions.TruncateTime(startDate) &&
                                                 //      DbFunctions.TruncateTime(booking.transdate) <= DbFunctions.TruncateTime(endDate)
                                                 select booking).ToList();

                //IEnumerable<Booking> bookings = (from booking in _dbEntities.Bookings
                //                                 where booking.transdate >= startDate &&
                //                                      booking.transdate <= endDate
                //                                 select booking).ToList();

                salessummarylist = (from b in bookings
                    //join p in _dbEntities.Payments on b.trn_Id equals p.trn_Id into bp
                    //from p in  bp.DefaultIfEmpty()
                    select new SalesSummaryViewModel()
                    {
                        transId = b.trn_Id,
                        accountname = Utilities.Getfullname(b.Customer.lastname, b.Customer.firstname, b.Customer.middle),
                        dateTrans = Convert.ToDateTime(b.startdate),
                        //reference =p==null?"No reference" :p.particular,
                        particular = b.Package.p_descripton + " @ " + b.Package.p_amountPax +" /pax x " + b.noofperson ,
                        CashSales = _getTotalPaymentByBooking(b.trn_Id),
                        OnAccount = transdetails.GetTotalBookingAmount(b.trn_Id) - _getTotalPaymentByBooking(b.trn_Id)


                    }).Where(t=>t.OnAccount>0).ToList();


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

           

            return Json(new {data=salessummarylist}, JsonRequestBehavior.AllowGet);
        }


        public ActionResult CashSalesReportIndex()
        {

            return View();
        }

        public ActionResult CashReportView(DateTime datefrom,DateTime dateto)
        {
            var pOption = new PrintOptionViewModel()
            {
               
                selPrintOpt = "cashreport",
                dateFrom = Convert.ToDateTime(datefrom),
                dateTo = Convert.ToDateTime(dateto)

            };

            return View("~/Views/Shared/ReportContainer.cshtml", pOption);
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


                addonslist = _dbEntities.BookingAddons.Where(x => x.trn_Id == transId).ToList();
                //addonsTotal = addonslist.Sum(y => Convert.ToDecimal(y.AddonAmount));


                if ((bool) _transDetails.Booking_Trans.apply_extendedAmount) // check if location extended charge is true otherwise extended location will be zero value
                {
                    extendedLocationAmount = BookingsService.Get_extendedAmountLoc(transId);
                }


                //belowminPax = packageType.Trim() == "vip" ? 0 : transactionDetails.GetBelowMinPaxAmount(no_of_pax);

                dpAmount = transactionDetails.GetTotalDownPayment(transId);
                fpAmount = transactionDetails.GetFullPayment(transId);
                cateringdiscountAmount = packageType.Trim() == "vip" ? 0 : TransactionDetailsViewModel.GetCateringdiscountByPax(no_of_pax);

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
                _transDetails.fullpaymnt = fpAmount;
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
        public ActionResult GetPackageonBooking(int? transId)
        {
            PackageBookingViewModel pBooking = new PackageBookingViewModel();
            BookMenusViewModel bm = new BookMenusViewModel();

            pBooking = packageBook.GetBookingDetails().FirstOrDefault(x => x.transactionId == transId);


            return View(pBooking);
        }

        public ActionResult GetAllCancelledBookings()
        {
            return View("CancelledBookings");
        }

        [HttpGet]
        public JsonResult Load_CancelledBookingList()
        {
            var dbcontext = new PegasusEntities();

           
           List<CancelBookingViewModel> cancelledList=new List<CancelBookingViewModel>();

            var list = (from c in _dbEntities.CancelledBookings select c).ToList();

            cancelledList = (from l in list join b in dbcontext.Bookings on l.trn_Id equals b.trn_Id
                select new CancelBookingViewModel()
                {
                    TransId =(int) l.trn_Id,
                    CustomerFullname = Utilities.Getfullname(l.Booking.Customer.lastname, l.Booking.Customer.firstname, l.Booking.Customer.middle),
                    ReasonforCancel = l.reasoncancelled,
                    CancelDate =Convert.ToDateTime(l.cancelledDated)

                }).ToList();

            return Json(new {data = cancelledList}, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult LoadAccoundReceivablesByCustomer(int cusId)
        {
            List<TransRecievablesViewModel> recievablesList = new List<TransRecievablesViewModel>();

            try
            {
                IQueryable<Booking> bookinglist = (from b in _dbEntities.Bookings where b.c_Id == cusId select b);


                recievablesList = tr.GetAllRecievables(bookinglist).ToList();

                //recievablesList = tr.GetAllRecievables().Where(x => x.cusId == cusId && x.balance>0).ToList();
                ContainerClass.TransRecievablesAccn = recievablesList.Where(t=>t.balance>0).ToList();

                // recievablesList = (from r in list where r.cusId == cusId select r) as List<TransRecievablesViewModel>;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Json(new { data = ContainerClass.TransRecievablesAccn }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PrintSoa(int cusId)
        {
            var pOption = new PrintOptionViewModel()
            {
                Id = cusId,
                selPrintOpt = "accnrecievepercustomer",
                //AccountRecievables =Utilities.TransRecievablesAccn
            };

            return View("~/Views/Shared/ReportContainer.cshtml", pOption);
        }

        [HttpGet]
        public ActionResult BookingTransactionHistory(int transId)
        {
            // var booking = _dbEntities.Bookings.Find(transId);
         
            var customerbookingHistory = new CustomerBookingHistoryViewModel();

            var firstOrDefault = (from b in _dbEntities.Bookings where b.trn_Id == transId select b).FirstOrDefault();
            if (firstOrDefault != null)
            {
                customerbookingHistory = new CustomerBookingHistoryViewModel()
                {
                    TransId = transId,
                    Customer = firstOrDefault.Customer
                };
            }

            return View(customerbookingHistory);
        }

        [HttpGet,ChildActionOnly]
        public ActionResult LogHistoryTransactionTable(int transId)
        {
            var bookaudit = _dbEntities.AuditLogs.Where(x => x.TableName == "Booking").ToList();
            var logs = JsonExtractorHelper.BookingLogsHistory(bookaudit, transId);

            //var list = logs.GroupBy(d => DbFunctions.TruncateTime(d.Logdate)).OrderBy(g => g.Key).Select(g => g.Select(v=>v.BookingEventList).ToList()).ToList();


            return PartialView("_BookingTransLog",logs);
        }

        [HttpGet]
        public ActionResult AdminReportsMonthlyCash_Index()
        {

            return View();

        }


        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin)]
        [HttpGet]
        public ActionResult MonthlyCashReportView(DateTime datefrom,DateTime dateto,bool w_unsettle)
        {
            var pOption = new PrintOptionViewModel()
            {

                selPrintOpt = "cateringreport",
                dateFrom = Convert.ToDateTime(datefrom),
                dateTo = Convert.ToDateTime(dateto),
                ex_unsetevent = w_unsettle

            };

           

            var list = IncentivesService.GetCateringReport(BookingsService.GetBookingReport(pOption.dateFrom,pOption.dateTo), t => t.iscancelled == false && t.isDeletedTran == false && t.p_type == PackageType.cat.ToString() || t.p_type == PackageType.pm.ToString());

            if (pOption.ex_unsetevent == true)
            {
                list = list.Where(t => t.Status == "pd").ToList();
               // list=list.Where(t=>t.)
            }

            ContainerClass.CateringList = list.ToList();

            return View("~/Views/Shared/ReportContainer.cshtml", pOption);
        }

        [HttpGet]
        public ActionResult AdminReportsIncentives_Index() => View();


       
        [HttpGet]
        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin)]
        public ActionResult Admin_IncentivesReport(DateTime dateFrom,DateTime dateTo)
        {
            var pOption = new PrintOptionViewModel()
            {

                selPrintOpt = "incentivesreport",
                dateFrom = Convert.ToDateTime(dateFrom),
                dateTo = Convert.ToDateTime(dateTo)
 

            };

            //IncentivesService.DateFrom = DateTime.Parse("07-01-2021",CultureInfo.InvariantCulture);
            //IncentivesService.dateTo = DateTime.Parse("07-30-2021", CultureInfo.InvariantCulture);

            //var list= IncentivesService.Paymentlist();

            var incentivesService = new IncentivesService();

            incentivesService.GetIncentivesReport(pOption.dateFrom,pOption.dateTo);


            return View("~/Views/Shared/ReportContainer.cshtml", pOption);
        }
        
        [HttpGet]
        public ActionResult AddonsReport_Index() => View();

        public ActionResult Admin_AddonsReport(DateTime dateFrom, DateTime dateTo)
        {
            var pOption = new PrintOptionViewModel()
            {

                selPrintOpt = "addons_report",
                dateFrom = Convert.ToDateTime(dateFrom),
                dateTo = Convert.ToDateTime(dateTo)


            };

            var list = _dbEntities.Database.SqlQuery<AddonsReportViewModel>("exec [dbo].[Addons] @datestart,@date_end",
                new SqlParameter("@datestart",pOption.dateFrom),
                new SqlParameter("@date_end",pOption.dateTo)).ToList();

            ContainerClass.GetAddonsReport(list);

            return View("~/Views/Shared/ReportContainer.cshtml", pOption);
        }

        protected override void Dispose(bool disposing)
        {
            _dbEntities.Dispose();
        }

      
    }
}