using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.Models;
using SBOSysTacV2.ViewModel;

namespace SBOSysTacV2.Controllers
{


    [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin,UserPermessionLevelEnum.cashier)]

    public class PaymentsController : Controller
    {


        private BookingsViewModel bookingsViewModel=new BookingsViewModel();
        private PaymentsViewModel paymentsViewModel=new PaymentsViewModel();
        private BookingPaymentsViewModel bookingPayments=new BookingPaymentsViewModel();
        private TransRecievablesViewModel tr = new TransRecievablesViewModel();
        private PaymentsViewModel pv = new PaymentsViewModel();
        private TransactionDetailsViewModel transdetails=new TransactionDetailsViewModel();
        private PegasusEntities _dbcontext;

        public PaymentsController()
        {
            _dbcontext = new PegasusEntities();
        }

        [HttpGet]
        public ActionResult GetBooking_Payments(int transactionId)
        {
            var bookpay=new BookingPaymentsViewModel();

         //  BookingsViewModel bbViewModel=new BookingsViewModel();
            try
            {
                decimal totalAmount = 0;
                bookpay.transId = transactionId;

                //.SingleOrDefault(x => x.trn_Id == transactionId)

                bookpay.Bookings = bookingsViewModel.GetListofBookings(transactionId);

                totalAmount = bookingPayments.Get_TotalAmountBook(transactionId);
                bookpay.t_amtBooking = totalAmount;
                bookpay.t_addons = bookingPayments.getTotalAddons(transactionId);
                bookpay.cateringdiscount = bookingPayments.GetCateringDiscount(transactionId);
                bookpay.locationextcharge = transdetails.Get_extendedAmountLoc(transactionId);
                bookpay.generaldiscount = bookingPayments.getBookingTransDiscount(transactionId, totalAmount);

                bookpay.PaymentList = bookingPayments.GetPaymentDetaiilsBooking(transactionId);

               // bookpay.Bookings.startdate = DateTime.Now;


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return View(bookpay);
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
        public ActionResult Add_PaymentPartialView(int transactionId)
        {
            PaymentsViewModel pay=new PaymentsViewModel();

            //pay.particular = Utilities.EventSlip_Generator();
            pay.transId = transactionId;
            pay.dateofPayment=DateTime.Now;
            pay.payType = 1;

            return PartialView(pay);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SavePayment(PaymentsViewModel paymentviewmodel)
        {
            if (!ModelState.IsValid) return PartialView("Add_PaymentPartialView", paymentviewmodel);

          //  bool success = false;

            var url = "";
            try
            {
                Payment newPayment=new Payment()
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
                    p_createdbyUser = User.Identity.GetUserId()

                };

                _dbcontext.Payments.Add(newPayment);
                _dbcontext.SaveChanges();

                url = Url.Action("GetPaymentList", "Payments", new {transId = paymentviewmodel.transId });

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Json(new {success = true,url=url}, JsonRequestBehavior.AllowGet);

        }

        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin, UserPermessionLevelEnum.cashier)]
        [HttpPost]
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

        protected override void Dispose(bool disposing)
        {
            _dbcontext.Dispose();
        }
    }
}