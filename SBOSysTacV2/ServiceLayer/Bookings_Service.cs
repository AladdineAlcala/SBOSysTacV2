using SBOSysTacV2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.ViewModel;

namespace SBOSysTacV2.ServiceLayer
{

    public class BookingsService
    {
     



        //get totalPackageAmount
        public static decimal Get_TotalAmountBook(int transId)
        {
            decimal totalAmount = 0;
            var _dbcontext = new PegasusEntities();

            List<BookingAddon> addonsList = new List<BookingAddon>();

            try
            {
                decimal totalPackage_Amount = 0;
                decimal addons = 0;
                decimal discount = 0;
                int noofpax = 0;

                decimal hasLocationExtendedCharge = 0;


                var package = new PackageBookingViewModel().GetPackageByTransaction_Id(transId);

                if (package.p_type.TrimEnd() != "sd")
                {
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

                    noofpax = (int)(bookingdetails.no_of_pax);

                    totalPackage_Amount = (bookingdetails != null) ? Convert.ToDecimal(bookingdetails.packageAmount) * noofpax : 0;

                    // get transaction discount
                    totalAmount = totalPackage_Amount + addons;

                    discount = getBookingTransDiscount(transId, totalAmount);

                    totalAmount = discount > 0 ? (totalAmount - discount) : totalAmount;

                    hasLocationExtendedCharge = new TransactionDetailsViewModel().Get_extendedAmountLoc(transId);

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

            _dbcontext.Dispose();

            return totalAmount;
        }


        public static decimal getBookingTransDiscount(int transId, decimal amountdue)
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