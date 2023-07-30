using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.Models;
using SBOSysTacV2.ServiceLayer;
using SBOSysTacV2.ViewModel;

namespace SBOSysTacV2.Controllers
{


    [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin,UserPermessionLevelEnum.cashier)]
    public class PaymentsController : Controller
    {


        private BookingsViewModel bookingsViewModel;
        private PaymentsViewModel paymentsViewModel;
        private BookingPaymentsViewModel bookingPayments;
        private TransRecievablesViewModel tr;
        private PaymentsViewModel pv;
        private BookingOtherChargeViewModel bookOtherCharge;
        private TransactionDetailsViewModel transdetails;
        private PegasusEntities _dbcontext;
        static readonly Func<int, decimal> _getBookingAmount = BookingsService.Get_TotalAmountBook;
        readonly Func<Booking, List<ICollection<BookAddonsDetail>>> getAddonDetails = BookingAddonDetailsViewModel.GetAddonDetails;
        public PaymentsController()
        {
            _dbcontext = new PegasusEntities();
            bookingsViewModel = new BookingsViewModel();
            paymentsViewModel = new PaymentsViewModel();
            bookingPayments = new BookingPaymentsViewModel();
            tr = new TransRecievablesViewModel();
            pv = new PaymentsViewModel();
            bookOtherCharge = new BookingOtherChargeViewModel();
            transdetails = new TransactionDetailsViewModel();
        }

        [HttpGet]
        public ActionResult GetBooking_Payments(int transactionId)
        {

            try
            {
                var bookings = _dbcontext.Bookings.Find(transactionId);

                decimal totalAmount = 0;
                bookingPayments.transId = transactionId;

                bookingPayments.Bookings = bookingsViewModel.GetListofBookings(transactionId);
                //totalAmount = _getBookingAmount(transactionId);
                bookingPayments.t_amtBooking = _getBookingAmount(transactionId); ;
                bookingPayments.t_addons = AddonsViewModel.AddonsTotal(getAddonDetails(bookings));
                bookingPayments.t_otherCharge = bookOtherCharge.GetTotalOtherCharges(bookings.Book_OtherCharge.ToList());
                bookingPayments.locationextcharge = BookingsService.Get_extendedAmountLoc(transactionId);

                //Discounts 
                if (bookings.Package.p_type.Trim() != "pm")
                {
                    bookingPayments.generaldiscount = BookingsService.getBookingTransDiscount(transactionId, totalAmount);
                    bookingPayments.cateringdiscount = bookingPayments.GetCateringDiscount(transactionId);
                }
               



                bookingPayments.PaymentList =this.bookingPayments.GetPaymentDetaiilsBooking(transactionId);

               // bookpay.Bookings.startdate = DateTime.Now;


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return View(bookingPayments);
        }

        public ActionResult LoadPaymentList(int transactionId)
        {
           List<PaymentsViewModel>listpayment=new List<PaymentsViewModel>();

            try
            {

                //listpayment = paymentsViewModel.GetPaymentsList().Where(p=>p.transId==transactionId).ToList();

                listpayment = paymentsViewModel.GetPaymentsListByClient(transactionId).ToList();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return Json(new {data=listpayment }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult Add_PaymentPartialView(int transactionId,decimal balance)
        {

            var paymentVM = new PaymentsViewModel()
            {
                transId = transactionId,
                dateofPayment = DateTime.Now,
                payType = 1,
                amtPay = balance,
                createdByUserId = User.Identity.GetUserId(),
                createdByUserName = User.Identity.GetUserName()

        };


            return PartialView(paymentVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SavePayment(PaymentsViewModel paymentviewmodel)
        {
            if (!ModelState.IsValid) return PartialView("Add_PaymentPartialView", paymentviewmodel);

          //  bool success = false;

            var url = "";
            var success = false;

            int persist = 0;

            try
            {
                var newPayment=new Payment
                {
                    payNo = Utilities.Generate_PaymentId(),
                    trn_Id = paymentviewmodel.transId,
                    dateofPayment = paymentviewmodel.dateofPayment,
                    particular = paymentviewmodel.particular,
                    payType = paymentviewmodel.payType,
                    amtPay = paymentviewmodel.amtPay,
                    pay_means = paymentviewmodel.pay_means,
                    checkNo = paymentviewmodel.checkNo,
                    notes = paymentviewmodel.notes,
                    p_createdbyUser =paymentviewmodel.createdByUserId,
                    p_createdDate = paymentviewmodel.p_createdDate,
                    p_updatedDate = paymentviewmodel.p_updateDate

                };

               _dbcontext.Payments.Add(newPayment);

                persist = _dbcontext.SaveChanges();

                url = Url.Action("GetPaymentList", "Payments", new {transId = paymentviewmodel.transId });

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            if (persist == 1)
            {
                success = true;
            }

            return Json(new {success,url}, JsonRequestBehavior.AllowGet);

        }

        //[UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin, UserPermessionLevelEnum.cashier)]

        
        [HttpPost]
        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin)]
        public ActionResult RemovePayment(int pmtNo)
        {
            Payment paymt=new Payment();
            bool success = false;
            var url = "";
           
            try
            {
                paymt = _dbcontext.Payments.Find(pmtNo);

                int t_Id =Convert.ToInt32(paymt.trn_Id);

             
                    _dbcontext.Payments.Remove(paymt);

                    _dbcontext.SaveChanges();


                    url = Url.Action("GetPaymentList", "Payments", new { transId =t_Id});
                    success = true;
               
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Json(new {success=success,url=url}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Update_PaymentPartialView(string pymtId)
        {
            Payment pmt=new Payment();
            PaymentsViewModel pmtViewModel = new PaymentsViewModel();

           
                pmt = _dbcontext.Payments.FirstOrDefault(x => x.payNo == pymtId);

                if (pmt != null)
                {
                     pmtViewModel = new PaymentsViewModel()
                    {
                        PayNo = pmt.payNo,
                        transId = pmt.trn_Id,
                        dateofPayment = pmt.dateofPayment,
                        particular = pmt.particular,
                        payType = pmt.payType,
                        amtPay = pmt.amtPay,
                        pay_means = pmt.pay_means.Trim(),
                        checkNo = pmt.checkNo,
                        notes = pmt.notes

                    };

                }

            return PartialView(pmtViewModel);
        }

        [HttpPost]
        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin, UserPermessionLevelEnum.cashier)]
        [ValidateAntiForgeryToken]
        public ActionResult Update_PaymentPartialView(PaymentsViewModel updatedPayment)
        {
            if (!ModelState.IsValid) return PartialView("Update_PaymentPartialView", updatedPayment);

            bool success = false;

            var url = "";
            try
            {

                var modifiedPayment = _dbcontext.Payments.FirstOrDefault(pay => pay.payNo == updatedPayment.PayNo);

                if (modifiedPayment != null)
                {
                    modifiedPayment.payNo = updatedPayment.PayNo;
                    modifiedPayment.trn_Id = updatedPayment.transId;
                    modifiedPayment.dateofPayment = updatedPayment.dateofPayment;
                    modifiedPayment.particular = updatedPayment.particular;
                    modifiedPayment.payType = updatedPayment.payType;
                    modifiedPayment.amtPay = updatedPayment.amtPay;
                    modifiedPayment.pay_means = updatedPayment.pay_means;
                    modifiedPayment.checkNo = updatedPayment.checkNo;
                    modifiedPayment.notes = updatedPayment.notes;
                    modifiedPayment.p_createdbyUser = User.Identity.GetUserId();

                    url = Url.Action("GetPaymentList", "Payments", new { transId = modifiedPayment.trn_Id });
                }

            
                _dbcontext.SaveChanges();

               
                success = true;


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Json(new {success=success,url=url}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetPaymentList(int transId)
        {
            var paytrans = new PaymentTransViewModel();
            try
            {
                paytrans.Booking = _dbcontext.Bookings.ToList().Find(t => t.trn_Id == transId);  //863ms

                paytrans.transId = transId;
                paytrans.Payments = pv.GetPaymentsListByClient(transId);   //804ms

                paytrans.TransRecievables = tr.GetRecieveDetailsByTransaction(paytrans.Booking);  //48,672 ms

                paytrans.refundentry = _dbcontext.Refunds.FirstOrDefault(x => x.trn_Id == transId);


            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return PartialView("_PaymentsList", paytrans);
        }

        [HttpGet]
        public ActionResult GetPaymentVoucherForm()
        {

            return PartialView("_GetPaymentVoucherForm");
        }

        [HttpPost]
        public ActionResult PrintVoucherReport(PrintVoucher printVoucher)
        {
            if (!ModelState.IsValid) return PartialView("_GetPaymentVoucherForm", printVoucher);

            var success = false;

            var isvalidPmtNo = _dbcontext.Payments.Any(t => t.payNo == printVoucher.PaymentNo);
          

            if (isvalidPmtNo)
            {
                success = true;
            }
            else
            {
                success = false;
                ModelState.AddModelError(String.Empty, "No Payment reference found in the database");
                //return PartialView("_GetPaymentVoucherForm", printVoucher);
            }

            return Json(new {success= success,data=printVoucher.PaymentNo, errorList= ModelState.Values.Where(t => t.Errors.Count > 0) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PrintPaymentDetails(int transId)
        {
            var pOption = new PrintOptionViewModel()
            {
                Id = transId,
                selPrintOpt ="printaccnRcv"

            };

            //var contractReciept=new ContractReceiptViewModel();

            //contractReciept = cr.getContractReciept(pOption.Id);

            //return View("~/Views/Bookings/PrintContractForm.cshtml", contractReciept);

            return View("~/Views/Shared/ReportContainer.cshtml", pOption);

        }

        public ActionResult ReturnPrintVoucher(string paymentNo)
        {
            return View("~/Views/Shared/ReportContainer.cshtml",new PrintOptionViewModel()
            {
                pmtNo = paymentNo,
                selPrintOpt = "printaccnRcv"
            });
        }


        protected override void Dispose(bool disposing)
        {
            _dbcontext.Dispose();
        }
    }
}