using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SBOSysTacV2.Models;
using SBOSysTacV2.ServiceLayer;
using SBOSysTacV2.ViewModel;

namespace SBOSysTacV2.Controllers
{
    public class EventsController : Controller
    {
        // GET: Events

        public PegasusEntities _dbcontext;

        private EventsViewModel events=new EventsViewModel();
        private CustomerBookingsViewModel cb=new CustomerBookingsViewModel();
        private TransactionDetailsViewModel transactionDetails=new TransactionDetailsViewModel();

        public EventsController()
        {
            _dbcontext = new PegasusEntities();
        }
        public ActionResult Index()
        {
            ViewBag.FormTitle = "Calendar Events Index";
         
                return View();
           
           
        }
        

        [HttpGet]
        //[OutputCache(Duration = 60,VaryByParam = "none",Location = System.Web.UI.OutputCacheLocation.Server)]
        public ActionResult GetAllEvents()
        {
            List<EventsViewModel> listofevents=new List<EventsViewModel>();

            listofevents = events.MixedEvents().ToList();

            return Json(listofevents,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get_AllBookingEvents()
        {
            List<EventsViewModel> listofevents = new List<EventsViewModel>();

            listofevents = events.GetAllBookingEvents().ToList();

            return Json(listofevents, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Get_AllReservationEvents()
        {
            List<EventsViewModel> listofevents = new List<EventsViewModel>();

            listofevents = events.GetAllReservationEvents().ToList();

            return Json(listofevents, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetAllEventsDay(DateTime? eventdate)
        {
          //  List<CustomerBookingsViewModel> custbookingList=new List<CustomerBookingsViewModel>();
          
                EventSelectionViewModel eventsel = new EventSelectionViewModel()
                {
                    eventdateselected =Convert.ToDateTime(eventdate),
                    //eventlist = cb.GetCusBookings().Where(d => d.bookdatetime.Value.Date == eventdate.Value.Date).OrderByDescending(x => x.bookdatetime.Value.ToShortTimeString()).ToList()

                    eventlist = events.MixedEventsDayPreview().Where(d => d.bookdatetime.Value.Date == eventdate.Value.Date).OrderByDescending(x => x.bookdatetime.Value.ToShortTimeString()).ToList()
                };

            return PartialView("GetEventsDay", eventsel);
        }

        [HttpGet]
        public ActionResult GetAllEventsToday()
        {
            //  List<CustomerBookingsViewModel> custbookingList=new List<CustomerBookingsViewModel>();

            DateTime currDate=DateTime.Now;

            EventSelectionViewModel eventsel = new EventSelectionViewModel()
            {
                eventdateselected = Convert.ToDateTime(currDate),
                //eventlist = cb.GetCusBookings().Where(d => d.bookdatetime.Value.Date == eventdate.Value.Date).OrderByDescending(x => x.bookdatetime.Value.ToShortTimeString()).ToList()

                eventlist = events.MixedEventsDayPreview().Where(d => d.bookdatetime.Value.Date == currDate.Date).OrderBy(x => x.bookdatetime.Value.TimeOfDay).ToList()
            };




            return PartialView("GetEventsDay", eventsel);
        }


        public ActionResult Get_EventBookingDetails(int bookingId)
        {
            TransactionDetailsViewModel _transDetails = new TransactionDetailsViewModel();
            try
            {

                _transDetails = transactionDetails.GetTransactionDetails().FirstOrDefault(x => x.transactionId.Equals(bookingId));

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
                //addonsTotal = addonslist.Sum(y => Convert.ToDecimal(y.AddonAmount));
                extendedLocationAmount = BookingsService.Get_extendedAmountLoc(transId);

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

        protected override void Dispose(bool disposing)
        {
            _dbcontext.Dispose();
        }
    }

}