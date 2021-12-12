using SBOSysTacV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SBOSysTacV2.ViewModel
{
    public class BookingPaymentsViewModel
    {
        public int transId { get; set; }
        public decimal t_amtBooking { get; set; }
        public decimal t_addons { get; set; }
        public decimal cateringdiscount { get; set; }
        public decimal generaldiscount { get; set; }
        public decimal locationextcharge { get; set; }
        public BookingsViewModel Bookings { get; set; }
        public IEnumerable<PaymentsViewModel> PaymentList { get; set; }

        private TransactionDetailsViewModel transdetails = new TransactionDetailsViewModel();

        //get totalPackageAmount
        public decimal Get_TotalAmountBook(int transId)
        {
            decimal totalAmount = 0;
            var _dbcontext = new PegasusEntities();

            List<BookingAddon> addonsList = new List<BookingAddon>();

            try
            {
                decimal totalPackage_Amount = 0;
                decimal addons = 0;

                var bookingdetails = (from books in _dbcontext.Bookings
                                      join packages in _dbcontext.Packages on books.p_id equals packages.p_id
                                      //join bookaddons in _dbcontext.BookingAddons on books.trn_Id equals bookaddons.trn_Id
                                      where books.trn_Id == transId
                                      select new
                                      {
                                          packageAmount = packages.p_amountPax,
                                          no_of_pax = books.noofperson,
                                          addons = books.BookingAddons
                                      }).FirstOrDefault();

                totalPackage_Amount = (bookingdetails != null) ? Convert.ToDecimal(bookingdetails.packageAmount) * Convert.ToInt32(bookingdetails.no_of_pax) : 0;

                //addons = bookingdetails.addons.Count > 0 ? Convert.ToDecimal(bookingdetails.addons.Sum(x => x.AddonAmount)) : 0;

                // get transaction discount
                totalAmount = totalPackage_Amount + addons;

                var discount = this.getBookingTransDiscount(transId, totalAmount);

                totalAmount = discount > 0 ? (totalAmount - discount) : totalAmount;

                var hasLocationExtendedCharge = transdetails.Get_extendedAmountLoc(transId);

                if (hasLocationExtendedCharge > 0)
                {
                    totalAmount = totalAmount + (hasLocationExtendedCharge * Convert.ToInt32(bookingdetails.no_of_pax));
                }

                var hasCateringdiscounted = transdetails.GetCateringdiscountByPax(Convert.ToInt32(bookingdetails.no_of_pax));

                if (hasCateringdiscounted > 0)
                {
                    totalAmount = totalAmount - (hasCateringdiscounted * Convert.ToInt32(bookingdetails.no_of_pax));
                }

                //var belowminpax = transdetails.GetBelowMinPaxAmount(Convert.ToInt32(b.noofperson));

                //if (belowminpax > 0)
                //{
                //    var belowminpaxAmt = belowminpax * Convert.ToInt32(b.noofperson);

                //    totalAmount = totalAmount + belowminpaxAmt;
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            _dbcontext.Dispose();

            return totalAmount;
        }

        public List<BookingAddon> GetAllBookingsAddon(PegasusEntities _dbcontext, int transId)

        {
            return _dbcontext.BookingAddons.Where(x => x.trn_Id == transId)
                                                                 .ToList();
        }

        public decimal GetCateringDiscount(int transid)
        {
            var _dbcontext = new PegasusEntities();
            Booking bookings = new Booking();
            decimal discountedAmount = 0;

            bookings = _dbcontext.Bookings.FirstOrDefault(x => x.trn_Id == transid);

            if (bookings != null)
            {
                var noofpax = bookings.noofperson;

                var amount = transdetails.GetCateringdiscountByPax(Convert.ToInt32(noofpax));
                discountedAmount = Convert.ToDecimal(amount * noofpax);
            }

            _dbcontext.Dispose();

            return discountedAmount;
        }

        public decimal getTotalAddons(int transId)
        {
            var _dbcontext = new PegasusEntities();
            var addonsList = _dbcontext.BookingAddons.Where(x => x.trn_Id == transId).ToList();

            _dbcontext.Dispose();

            return Convert.ToDecimal(100);
        }

        public IEnumerable<PaymentsViewModel> GetPaymentDetaiilsBooking(int transId)
        {
            var _dbcontext = new PegasusEntities();
            List<PaymentsViewModel> paylist = new List<PaymentsViewModel>();

            try
            {
                paylist = (from p in _dbcontext.Payments
                           where p.trn_Id == transId
                           select new PaymentsViewModel()
                           {
                               PayNo = p.payNo,
                               transId = p.trn_Id,
                               dateofPayment = p.dateofPayment,
                               particular = p.particular,
                               payType = p.payType,
                               amtPay = p.amtPay,
                               pay_means = p.pay_means,
                               notes = p.notes
                           }).OrderBy(x => x.PayNo).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            _dbcontext.Dispose();

            return paylist;
        }

        public decimal getBookingTransDiscount(int transId, decimal amountdue)
        {
            decimal discountedAmount = 0;
            var _dbcontext = new PegasusEntities();

            try
            {
                var discount = (from bd in _dbcontext.Book_Discount
                                join tdisc in _dbcontext.Discounts on bd.disc_Id equals tdisc.disc_Id
                                select new
                                {
                                    trans_Id = bd.trn_Id,
                                    discountType = tdisc.disctype,
                                    discount = tdisc.discount1
                                }).ToList();

                var discountDetails = discount.FirstOrDefault(x => x.trans_Id == transId);

                if (discountDetails != null)
                {
                    //decimal discAmt = 0;

                    if (discountDetails.discountType == "percentage")
                    {
                        var percentagedisc = discountDetails.discount / 100;

                        discountedAmount = amountdue * Convert.ToDecimal(percentagedisc);
                    }
                    else
                    {
                        discountedAmount = Convert.ToDecimal(discountDetails.discount);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            _dbcontext.Dispose();

            return Convert.ToDecimal(discountedAmount);
        }
    }
}