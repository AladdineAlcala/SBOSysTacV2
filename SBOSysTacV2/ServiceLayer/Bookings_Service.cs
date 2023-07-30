using SBOSysTacV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.ViewModel;
using System.Data.Entity;
using System.Threading;
using SBOSysTacV2.HtmlHelperClass;

namespace SBOSysTacV2.ServiceLayer
{

    public class BookingsService:IDisposable
    {
        private static PegasusEntities _dbcontext = new PegasusEntities();
       

        public BookingsService()
        {
            PegasusEntities _dbcontext = new PegasusEntities();
        }

        public Booking GetBookingByTransaction(int _transId)
        {
            var booking = new Booking();

            booking = (_dbcontext.Bookings.Where(c => c.trn_Id == _transId)).Include(c => c.Customer)
                .Single();

            return booking;

        }

        //get totalPackageAmount
        public static decimal Get_TotalAmountBook(int transId)
        {
            decimal totalAmount = 0;
            
            List<BookingAddon> addonsList = new List<BookingAddon>();
            Func<Booking, List<ICollection<BookAddonsDetail>>> getAddonDetails = BookingAddonDetailsViewModel.GetAddonDetails;

            var booking = _dbcontext.Bookings.FirstOrDefault(t => t.trn_Id == transId);

            try
            {
          
                decimal addons = 0;
                decimal discount = 0;
                decimal packageAmount = 0;
                decimal bookOtherCharge = 0;

                int noofpax = 0;

                decimal hasLocationExtendedCharge = 0;

                var packageType = booking.Package.p_type.ToString().Trim();

                if (packageType=="regular" || packageType=="vip")
                {
                    var bookingdetails = (from books in _dbcontext.Bookings
                                          join packages in _dbcontext.Packages on books.p_id equals packages.p_id
                                          where books.trn_Id == transId
                                          select new
                                          {
                                              packageAmount = packages.p_amountPax,
                                              no_of_pax = books.noofperson,
                                              //addons =(from ba in books.BookingAddons join bad in _dbcontext.BookAddonsDetails on ba.bookaddonNo equals bad.bookaddonNo select new { _addAmount = bad.amount * bad.qty}).FirstOrDefault()._addAmount,
                                              addons=books,
                                              bookothercharge =(from b in books.Book_OtherCharge where b.trn_Id==transId select b.amount).Sum()
                                          }).FirstOrDefault();


                    if (bookingdetails != null)
                    {
                        packageAmount = Convert.ToDecimal(bookingdetails.packageAmount);
                        noofpax = bookingdetails.no_of_pax??0;
                        addons = AddonsViewModel.AddonsTotal(getAddonDetails(bookingdetails.addons));
                        bookOtherCharge = bookingdetails.bookothercharge ?? 0;
                    }


                    totalAmount = (packageAmount * noofpax) + addons + bookOtherCharge;

                    discount = getBookingTransDiscount(transId, totalAmount);

                    totalAmount = discount > 0 ? (totalAmount - discount) : totalAmount;

                    hasLocationExtendedCharge = booking.Package.p_type!=PackageType.vip.ToString() ? Get_extendedAmountLoc(transId):0;

                    if (hasLocationExtendedCharge > 0)
                    {
                        totalAmount = totalAmount + (hasLocationExtendedCharge * noofpax);
                    }


                    var hasCateringdiscounted = TransactionDetailsViewModel.GetCateringdiscountByPax(noofpax);

                    if (hasCateringdiscounted > 0)
                    {
                        totalAmount = totalAmount - (hasCateringdiscounted * noofpax);
                    }


                }
                else if(packageType == "pm")
                {
                    var bookingdetails = (from books in _dbcontext.Bookings
                                          join packages in _dbcontext.Packages on books.p_id equals packages.p_id
                                          where books.trn_Id == transId
                                          select new
                                          {
                                              packageAmount = packages.p_amountPax,
                                              no_of_pax = books.noofperson,
                                              //addons =(from ba in books.BookingAddons join bad in _dbcontext.BookAddonsDetails on ba.bookaddonNo equals bad.bookaddonNo select new { _addAmount = bad.amount * bad.qty}).FirstOrDefault()._addAmount,
                                              addons = books,
                                              bookothercharge = (from b in books.Book_OtherCharge where b.trn_Id == transId select b.amount).Sum()
                                          }).FirstOrDefault();


                    if (bookingdetails != null)
                    {
                        packageAmount = Convert.ToDecimal(bookingdetails.packageAmount);
                        noofpax = bookingdetails.no_of_pax ?? 0;
                        addons = AddonsViewModel.AddonsTotal(getAddonDetails(bookingdetails.addons));
                        bookOtherCharge = bookingdetails.bookothercharge ?? 0;
                    }

                    totalAmount = (packageAmount * noofpax) + addons + bookOtherCharge;

                }

                else
                {

                    totalAmount = new BookMenusViewModel().ComputeAmountForSnacksByTransId(transId);

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return totalAmount;
        }


        public static decimal getBookingTransDiscount(int transId, decimal amountdue)
        {
            decimal discountedAmount = 0;

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


            return Convert.ToDecimal(discountedAmount);
        }

        public static List<Booking> GetBookingReport(DateTime dateFrom,DateTime dateTo)
        {


            return _dbcontext.Bookings.Where(t =>
                DbFunctions.TruncateTime(t.startdate) >= DbFunctions.TruncateTime(dateFrom) &&
                DbFunctions.TruncateTime(t.startdate) <= DbFunctions.TruncateTime(dateTo)).ToList();
        }

        public static decimal Get_extendedAmountLoc(int transId)
        {

            decimal extAmt = 0;

            var booking = _dbcontext.Bookings.FirstOrDefault(x => x.trn_Id == transId);


            if (booking.Package.p_type.Trim() != PackageType.vip.ToString() && booking.extendedAreaId != null)
            {
                try
                {
                    var list = (from b in _dbcontext.Bookings
                        join p in _dbcontext.Packages on b.p_id equals p.p_id
                        join pa in _dbcontext.PackageAreaCoverages on p.p_id equals pa.p_id
                        where pa.p_id == booking.p_id && pa.aID == booking.extendedAreaId
                        select new
                        {
                            extLocAmount = pa.ext_amount

                        }).FirstOrDefault();


                    if (list != null)
                    {

                        extAmt = Convert.ToDecimal(list.extLocAmount);
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return extAmt;
        }

        public static decimal GetOtherCharge(int transId)
        {
            return (from b in _dbcontext.Book_OtherCharge
                where b.trn_Id == transId
                select b.amount).Sum() ?? 0;
        }

        public void Dispose()
        {
            _dbcontext.Dispose();
        }
    }
}