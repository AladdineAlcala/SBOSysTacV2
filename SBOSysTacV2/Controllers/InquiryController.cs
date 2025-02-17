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
using System.Linq.Dynamic;
using System.Data;
using ClosedXML.Excel;
using System.Threading.Tasks;

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

        public ActionResult CollectionReportByPaymentType()
        {
            return View();
        }
        public ActionResult GetCollectionReportByPaymentType(DateTime? DateFrom, DateTime? DateTo)
        {
            if (!DateFrom.HasValue || !DateTo.HasValue)
            {
                return new HttpStatusCodeResult(400, "Date range is required.");
            }
            try
            {
                var CollectionSummaries = _dbEntities.Database.SqlQuery<CollectionReportSummaryResponse>("EXEC GetPaymentSummary @DateFrom, @DateTo",
                                new SqlParameter("@DateFrom", DateFrom.Value),
                                new SqlParameter("@DateTo", DateTo.Value)).ToList();
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Collection Report");

                    // Add Title/Heading
                    string reportTitle = $"Collection Report Summary (Dated from {DateFrom:yyyy-MM-dd} to {DateTo:yyyy-MM-dd})";
                    worksheet.Cell(1, 1).Value = reportTitle;
                    worksheet.Range("A1:B1").Merge();  // Merge cells A1 and B1 for the title
                    worksheet.Cell(1, 1).Style.Font.Bold = true;
                    worksheet.Cell(1, 1).Style.Font.FontSize = 14;  // Larger font for title
                    worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


                    // Add Headers (Shifted to row 3)
                    worksheet.Cell(3, 1).Value = "Payment Type";
                    worksheet.Cell(3, 2).Value = "Total Amount";
                    worksheet.Range("A3:B3").Style.Font.Bold = true;
                    var headerRange = worksheet.Range("A3:B3");
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightSteelBlue;
                    headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Loop through CollectionSummaries and add data
                    int row = 4;
                    foreach (var summary in CollectionSummaries)
                    {
                        worksheet.Cell(row, 1).Value = summary.PaymentType;
                        worksheet.Cell(row, 2).Value = summary.TotalAmount;
                        worksheet.Cell(row, 2).Style.NumberFormat.Format = "#,##0.00"; // Currency formatting
                        row++;
                    }

                    // Add a total row
                    worksheet.Cell(row, 1).Value = "Total";
                    worksheet.Cell(row, 1).Style.Font.Bold = true;
                    worksheet.Cell(row, 2).FormulaA1 = $"=SUM(B2:B{row - 1})";
                    worksheet.Cell(row, 2).Style.Font.Bold = true;
                    worksheet.Cell(row, 2).Style.NumberFormat.Format = "#,##0.00";

                    // Apply Borders ONLY for the Total Row
                    var totalRowRange = worksheet.Range($"A{row}:B{row}");
                    totalRowRange.Style.Border.TopBorder = XLBorderStyleValues.Thin;      // Single line on top
                    totalRowRange.Style.Border.BottomBorder = XLBorderStyleValues.Double; // Double line on bottom

                    // Auto-adjust column width
                    worksheet.Columns().AdjustToContents();

                    // Return the Excel file as a download
                    using (var stream = new System.IO.MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        stream.Position = 0;

                        return File(stream.ToArray(),
                           "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                           $"CollectionSummaryReport_{DateFrom?.ToString("yyyy-MM-dd")}-{DateTo?.ToString("yyyy-MM-dd")}.xlsx");
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                return new HttpStatusCodeResult(500, $"Database Error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(500, $"Unexpected Error: {ex.Message}");
            }

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

        //[HttpGet,ActionName("GetBookingDetailsPartial_Inquiry")]
        //public ActionResult BookingDetails_Inquiry(int transId) => PartialView("_BookingDetails_Inquiry", transactionDetails.GetTransactionDetailsById(transId));


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
            return PartialView("_BookingTransLog",logs);
        }

        [HttpGet]
        public ActionResult AdminReportsMonthlyCash_Index()
        {

            return View();

        }


        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin)]
        [HttpGet]
        public async Task<ActionResult> MonthlyCashReportView(DateTime datefrom,DateTime dateto,bool w_unsettle)
        {
            const string ReportName = "cateringreport";
            const string Status = "pd";
            var pOption = new PrintOptionViewModel()
            {
                selPrintOpt = ReportName,
                dateFrom = Convert.ToDateTime(datefrom),
                dateTo = Convert.ToDateTime(dateto),
                ex_unsetevent = w_unsettle
            };
            var bookings = await BookingsService.GetBookingReport(pOption.dateFrom, pOption.dateTo);
            var list = await Task.Run(() =>
                        IncentivesService.GetCateringReport(bookings, t =>
                        !t.iscancelled && !t.isDeletedTran &&
                        (t.p_type == PackageType.cat.ToString() ||
                         t.p_type == PackageType.pm.ToString() ||
                         t.p_type == PackageType.premier.ToString()))
                        );

            if (pOption.ex_unsetevent == true)
            {
                list = list.Where(t => t.Status == Status);
            }
            ContainerClass.CateringList = list.OrderBy(d=>d.EventDate).ToList();
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

            var addon_lechon = list.Where(t => t.addoncat_id == (int)AddonsEnum_Lechon.baka || t.addoncat_id==(int)AddonsEnum_Lechon.baboy).ToList();

            ContainerClass.GetAddonsReport(addon_lechon);

            return View("~/Views/Shared/ReportContainer.cshtml", pOption);
        }

        protected override void Dispose(bool disposing)
        {
            _dbEntities.Dispose();
        }

      
    }
}