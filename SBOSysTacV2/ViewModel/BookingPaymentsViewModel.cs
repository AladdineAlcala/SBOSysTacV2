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
        public decimal t_otherCharge { get; set; }
        public decimal cateringdiscount { get; set; }
        public decimal generaldiscount { get; set; }
        public decimal locationextcharge { get; set; }
        public BookingsViewModel Bookings { get; set; }
        public IEnumerable<PaymentsViewModel> PaymentList { get; set; }

        private static TransactionDetailsViewModel transdetails;

        private static PackageBookingViewModel package_book_vm;

        private static BookMenusViewModel book_menus_vm;

        public BookingPaymentsViewModel()
        {
            package_book_vm = new PackageBookingViewModel();
            transdetails = new TransactionDetailsViewModel();
            book_menus_vm = new BookMenusViewModel();
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

                var amount = TransactionDetailsViewModel.GetCateringdiscountByPax(Convert.ToInt32(noofpax));
                discountedAmount = Convert.ToDecimal(amount * noofpax);
            }

            _dbcontext.Dispose();

            return discountedAmount;
        }

        public static decimal getTotalAddons(int transId)
        {

            decimal addAmount = 0;

             var _dbcontext = new PegasusEntities();
             var addons = (from ba in _dbcontext.BookingAddons
                 join bad in _dbcontext.BookAddonsDetails on ba.bookaddonNo equals bad.bookaddonNo
                 where ba.trn_Id == transId
                 select new
                 {
                     _addAmount = bad.qty * bad.amount
                 }).FirstOrDefault();


             if (addons?._addAmount != null)
             {
                 addAmount = (decimal)addons._addAmount;
             }
             _dbcontext.Dispose();
            return Convert.ToDecimal(addAmount);
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

       
    }
}