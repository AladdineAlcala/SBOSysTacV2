using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using SBOSysTacV2.Models;
using SBOSysTacV2.ServiceLayer;
using SBOSysTacV2.ViewModel;

namespace SBOSysTacV2.Controllers
{
    public class BookingRefundsController : Controller
    {
        private PegasusEntities dbEntities;
        private RefundsViewModel _refundsView=new RefundsViewModel();
        private TransRecievablesViewModel _transRecievablesView = new TransRecievablesViewModel();
        private BookingPaymentsViewModel bookingPayments = new BookingPaymentsViewModel();
        private BookingsViewModel bookingsViewModel = new BookingsViewModel();
        private TransactionDetailsViewModel transdetails = new TransactionDetailsViewModel();
        private BookingRefundViewModel refundbook=new BookingRefundViewModel();
        static Func<int, decimal> _getBookingAmount = BookingsService.Get_TotalAmountBook;

        public BookingRefundsController()
        {
            dbEntities = new PegasusEntities();
        }
        // GET: BookingRefunds
        public ActionResult Index()
        {
            //var refundslist = _refundsView.GetAllRefundsList().ToList();

            return View();
        }

        [HttpGet]
        public JsonResult Get_RefundableBookings()
        {
            List<RefundsViewModel> refundableList = new List<RefundsViewModel>();
            //refundableList = _refundsView.GetAllRefundsList().Where(x => x.RefundAmount > 0|| (x.isCancelled && x.PaymemntAmount > 0)).ToList();
            refundableList = _refundsView.GetAllRefundsList().ToList();

            return Json(new { data = refundableList }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CheckRefundEntry(int transId)
        {
        
            bool hasrecordexist = dbEntities.Refunds.ToList().Any(x => x.trn_Id.Equals(transId));

            return Json(new {recordexist= hasrecordexist }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BookingRefundEntry(int transId)
        {
            var bookrefundinfo = new BookingRefundViewModel();
            //var refunndableAccount = _refundsView.GetAllRefundsList().FirstOrDefault(x => x.TransId == transId);

            var booking = (from b in dbEntities.Bookings select b).FirstOrDefault(x=>x.trn_Id==transId);


            decimal totalbookAmount = _getBookingAmount(transId);

            decimal totalAmountPay = (decimal) (from p in dbEntities.Payments select p).Where(s => s.trn_Id == transId).Sum(x => x.amtPay);

            bookrefundinfo.transId = transId;
            bookrefundinfo.refundDeduction = 0;

            if ((bool) booking.is_cancelled)
            {
                decimal cancellationsdeduction = 10m / 100m;
                int no_of_daysfreecancel = 5;

                decimal cancellationAmt = totalbookAmount * cancellationsdeduction;

                DateTime booktransdate = Convert.ToDateTime(booking.startdate);

                 
                bookrefundinfo.refundAmount = totalAmountPay;
                int x = Convert.ToInt16((booktransdate.Date-DateTime.Now.Date).TotalDays);
                bookrefundinfo.refundDeduction = no_of_daysfreecancel>x ? cancellationAmt :0;
                bookrefundinfo.refundNet = no_of_daysfreecancel > x ? totalAmountPay - cancellationAmt : totalAmountPay;

            }
            else
            {
                decimal refundAmount =  totalAmountPay - totalbookAmount;

                bookrefundinfo.refundAmount = refundAmount;
                bookrefundinfo.refundDeduction = 0;
                bookrefundinfo.refundNet = refundAmount;
            }
           
          

            return PartialView("_CreateRefundEntry",bookrefundinfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookingRefundEntry(BookingRefundViewModel bookingRefund)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateRefundEntry",bookingRefund);
            }

            var bookrefund = new Refund()
            {
                trn_Id = bookingRefund.transId,
                rfDate = bookingRefund.rfDate,
                rf_Reason = bookingRefund.refundReason,
                rf_Amount = bookingRefund.refundAmount,
                rfDeduction = bookingRefund.refundDeduction,
                rfNetAmount = bookingRefund.refundNet,
                rf_Stat = 0
            };

            dbEntities.Refunds.Add(bookrefund);
            dbEntities.SaveChanges();

            return Json(new {success=true}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetRefundEntry(int transId)
        {
            RefundEntryViewModel refundEntry = new RefundEntryViewModel();
            
              //decimal totalAmount = 0;
            refundEntry.transId = transId;
            refundEntry.t_amtBooking = _getBookingAmount(transId);
            refundEntry.t_amountPayment = (decimal)(from p in dbEntities.Payments select p)
                .Where(s => s.trn_Id == transId).Sum(x => x.amtPay);

            refundEntry.Bookings = bookingsViewModel.GetListofBookings(transId);
            refundEntry.bookingrefund = refundbook.GetBookingRefund(transId);


            return View(refundEntry);
        }

        [HttpGet]
        public ActionResult RefundLedger(int transId)
        {
            var t_rifID = (from re in dbEntities.Refunds select re).FirstOrDefault(x => x.trn_Id == transId);

            var refundTransViewModel = new RefundTransViewModel()
            {
                transId = transId,
                BookingRefund = refundbook.GetBookingRefund(transId),
                PostRefundEntry =t_rifID!=null? dbEntities.RefundEntries.Where(x => x.Rf_id == t_rifID.Rf_id).ToList():null

             };

          return PartialView("_refundledger",refundTransViewModel);
        }

        

        [HttpGet]
        public ActionResult PayRefundAccount(int transId)
        {
            var postrefundentry = new PostRefundEntryViewModel();

            var refund = dbEntities.Refunds.FirstOrDefault(x => x.trn_Id == transId);

            if (refund != null)
            {
                 postrefundentry = new PostRefundEntryViewModel()
                {
                    rfID = refund.Rf_id,
                    transId =transId
                
                };

                return PartialView("_PayRefundAccount", postrefundentry);
            }


            return PartialView("_PayRefundAccount", postrefundentry);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostCreditRefundEntry(PostRefundEntryViewModel p_entry)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_PayRefundAccount");
            }

            var refundentry = new RefundEntry()
            {   
                Rf_id = p_entry.rfID,
                Particular = p_entry.refundentrydetail,
                Amount = p_entry.EntryAmount
            };

            dbEntities.RefundEntries.Add(refundentry);
            dbEntities.SaveChanges();

            var url=@Url.Action("RefundLedger","BookingRefunds",new {transId=p_entry.transId});
            return Json(new {success=true,url=url}, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            dbEntities.Dispose();
        }
    }
}