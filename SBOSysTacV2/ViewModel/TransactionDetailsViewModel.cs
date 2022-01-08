using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class TransactionDetailsViewModel
    {
        public int transactionId { get; set; }
        public Customer Customer { get; set; }
        public BookingsViewModel Booking_Trans { get; set; }
        public Package Package_Trans { get; set; }
        public decimal book_discounts { get; set; } = 0;
        public string bookdiscountdetails { get; set; }
        public List<Book_OtherCharge> BookOtherCharges { get; set; }
        public decimal PackageAmount { get; set; } = 0;
        public decimal TotaAddons { get; set; } = 0;
        public decimal TotaMiscCharge { get; set; } = 0;
        public decimal TotaBelowMinPax { get; set; } = 0;
        public decimal TotaDp { get; set; } = 0;
        public decimal extLocAmount { get; set; } = 0;
        public decimal cateringdiscount { get; set; } = 0;
        public decimal fullpaymnt { get; set; } = 0;


        private PegasusEntities _dbEntities;
        private BookingsViewModel bvm;

        public TransactionDetailsViewModel()
        {
            _dbEntities = new PegasusEntities();
            bvm = new BookingsViewModel();
        }


        public IEnumerable<TransactionDetailsViewModel> GetTransactionDetails()
        {

            List<TransactionDetailsViewModel> _list = new List<TransactionDetailsViewModel>();

            List<BookingsViewModel> _bookingsViewModel = new List<BookingsViewModel>();

            // _dbEntities.Configuration.ProxyCreationEnabled = false;

            _bookingsViewModel = bvm.GetListofBookings().ToList();  // 1.  from 


            try
            {


                _list = (from b in _bookingsViewModel
                         join c in _dbEntities.Customers on b.c_Id equals c.c_Id
                         join p in _dbEntities.Packages on b.pId equals p.p_id
                         select new TransactionDetailsViewModel()
                        {
                            transactionId = b.trn_Id,
                            Booking_Trans = b,
                            Customer = c,
                            Package_Trans = p
                        }).ToList();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return _list;

        }

        public TransactionDetailsViewModel GetTransactionDetailsById(int transId)
        {
            var bookings = bvm.GetListofBookings(transId);

            var bookingsList = new TransactionDetailsViewModel();
            try
            {

                bookingsList = new TransactionDetailsViewModel()
                {
                    transactionId = transId,
                    Booking_Trans = bookings,
                    Customer =(from c in _dbEntities.Customers where c.c_Id==bookings.c_Id select c).Single(),
                    BookOtherCharges = (from ot in _dbEntities.Book_OtherCharge where ot.trn_Id == transId select ot).ToList(),
                    Package_Trans = (from p in _dbEntities.Packages where  p.p_id==bookings.pId select p).Single()
                };

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return bookingsList;

        }


        public decimal GetTotalBookingAmount(int transId)
        {
            decimal totalpackage = 0;
            decimal totalAddons = 0;
            decimal package = 0;

            try
            {
                var booktrans = _dbEntities.Bookings.FirstOrDefault(t => t.trn_Id == transId);
                if (booktrans != null)
                {
                    var pax = booktrans.noofperson;
                    var heads = booktrans.Package.p_amountPax;
                    package = Convert.ToDecimal(heads) * Convert.ToDecimal(pax);

                }

                var addonslist = _dbEntities.BookingAddons.Where(x => x.trn_Id == transId).ToList();

                //totalAddons = addonslist.Sum(y => Convert.ToDecimal(y.AddonAmount));

                totalpackage = package + totalAddons;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return totalpackage;
        }



        public decimal GetAccountTotalByTransId(int transId)
        {
            decimal packageTota = 0;

            var booktrans = _dbEntities.Bookings.FirstOrDefault(t => t.trn_Id == transId);

            if (booktrans.Package.p_type != "sd")
            {
                if (booktrans != null)
                {
                    var pax = booktrans.noofperson;
                    var heads = booktrans.Package.p_amountPax;
                    packageTota = Convert.ToDecimal(heads) * Convert.ToDecimal(pax);

                }
            }
           

            return packageTota;
        }

        public decimal GetBelowMinPaxAmount(int no_of_Pax)
        {

            decimal amt_added = 0;
            _dbEntities.Configuration.ProxyCreationEnabled = false;
            //List<PackagesRangeBelowMin> packagerangebelowmin=new List<PackagesRangeBelowMin>();
            PackagesRangeBelowMin packagerangebelowmin = new PackagesRangeBelowMin();

            packagerangebelowmin =
                _dbEntities.PackagesRangeBelowMins.First(x => x.pMax >= no_of_Pax && x.pMin <= no_of_Pax);


            if (packagerangebelowmin != null)
            {
                amt_added = Convert.ToDecimal(packagerangebelowmin.Amt_added);
            }

            return amt_added;
        }

        public decimal GetTotalDownPayment(int transId)
        {
            decimal dpAmount = 0;

            try
            {

                dpAmount = Convert.ToDecimal(_dbEntities.Payments.Where(p => p.trn_Id == transId && p.payType == 0)
                    .Sum(x => x.amtPay));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return dpAmount;

        }


        public decimal Get_extendedAmountLoc(String packageType,Booking booking)
        {
            decimal extAmt = 0;

            // var booking = _dbEntities.Bookings.FirstOrDefault(x => x.trn_Id == transId);


            if (packageType.Trim() != "vip" && booking.extendedAreaId.ToString() != null)
            {
                try
                {
                    var list = (from b in _dbEntities.Bookings
                        join p in _dbEntities.Packages on b.p_id equals p.p_id
                        join pa in _dbEntities.PackageAreaCoverages on p.p_id equals pa.p_id
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

        public decimal Get_extendedAmountLoc(int transId)
        {
            decimal extAmt = 0;

            var booking = _dbEntities.Bookings.FirstOrDefault(x => x.trn_Id == transId);


            if (booking.Package.p_type.Trim() != "vip" && booking.extendedAreaId != null)
            {
                try
                {
                    var list = (from b in _dbEntities.Bookings
                                join p in _dbEntities.Packages on b.p_id equals p.p_id
                                join pa in _dbEntities.PackageAreaCoverages on p.p_id equals pa.p_id
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


        public decimal GetFullPayment(int transId)
        {
            decimal fp = 0;

            try
            {
                fp = Convert.ToDecimal(_dbEntities.Payments.Where(p => p.trn_Id == transId)
                    .Sum(x => x.amtPay));

            }
            catch (Exception)
            {

                throw;
            }

            return fp;
        }


        public decimal GetTotalPaymentByTrans(int transId)
        {
            decimal totaPayment = 0;
            try
            {
                totaPayment = Convert.ToDecimal(_dbEntities.Payments.Where(t => t.trn_Id == transId)
                    .Sum(x => x.amtPay));

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return totaPayment;
        }


        public decimal Get_bookingDiscountbyTrans(int transId, decimal subtotal)
        {
            decimal totaldisc = 0;

            try
            {

                var hasdiscount = _dbEntities.Book_Discount.FirstOrDefault(x => x.trn_Id == transId);

                if (hasdiscount != null)
                {
                    var bookdiscount = _dbEntities.Discounts.FirstOrDefault(d => d.disc_Id == hasdiscount.disc_Id);

                    if (bookdiscount != null)
                    {
                        if (bookdiscount.disctype == "percentage")
                        {
                            var percentage = bookdiscount.discount1 / 100;

                            totaldisc = subtotal * Convert.ToDecimal(percentage);

                        }
                        else
                        {
                            totaldisc = Convert.ToDecimal(bookdiscount.discount1);
                        }
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return totaldisc;
        }


        public string Get_bookingDiscounDetailstbyTrans(int transId)
        {
           string discount_details=String.Empty;

            try
            {

                var hasdiscount = _dbEntities.Book_Discount.FirstOrDefault(x => x.trn_Id == transId);

                if (hasdiscount != null)
                {
                    var bookdiscount = _dbEntities.Discounts.FirstOrDefault(d => d.disc_Id == hasdiscount.disc_Id);

                    if (bookdiscount != null)
                    {
                        if (bookdiscount.disctype == "percentage")
                        {

                            discount_details = bookdiscount.discCode + ' ' + "pcnt " + bookdiscount.discount1 + '%';
                        }
                        else
                        {
                            discount_details = bookdiscount.discCode + ' ' + "amt " + bookdiscount.discount1;
                        }
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return discount_details;
        }



        //public int get_totalNoofMainMenus_on_booking(int transId)
        //{
        //    int i = 0;

        //    try
        //    {
        //        var noofMainMenu = (from b in _dbEntities.Bookings
        //                            join pb in _dbEntities.PackageBodies on b.p_id equals pb.p_id
        //                            where b.trn_Id == transId
        //                            select new
        //                            {
        //                                noofmainmenu = pb.mainCourse
        //                            }).FirstOrDefault();

        //        if (noofMainMenu != null) i = Convert.ToInt32(noofMainMenu.noofmainmenu);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;

        //    }

        //    return i;
        //}


        public bool CheckpackagehasCourse(int transId,string menuId)
        {
            bool has_coursemenuexist = false;

            var booking = _dbEntities.Bookings.Find(transId);

            if (booking != null)
            {

                //get all courses in packagebody by trans
                var this_bookingpackagebody =
                    (from pb in _dbEntities.PackageBodies select pb).Where(x => x.p_id == booking.p_id).ToList();

                        if (this_bookingpackagebody != null)
                        {
                                
                                    //get course of selected menu

                            var courseId = _dbEntities.Menus.FirstOrDefault(x => x.menuid == menuId).courseId;

                            has_coursemenuexist = this_bookingpackagebody.Any(m => m.courseId == courseId);

                            

                        }

            }

            return has_coursemenuexist;

        }

        public bool isSelectedMenuMainCourse(string menuId)
        {

            var courseMenu = (from m in _dbEntities.Menus
                join cc in _dbEntities.CourseCategories on m.courseId equals cc.courseId
                where m.menuid == menuId
                select new
                {
                    ismainMenu = cc.Main_Bol
                }).FirstOrDefault();



            return Convert.ToBoolean(courseMenu.ismainMenu);
        }

        public bool hasDiscountEnable(int transId)
        {
            var discountAplied = (from bookdiscount in _dbEntities.Book_Discount
                where bookdiscount.trn_Id == transId
                select bookdiscount).Any();

            return discountAplied;
        }

        public decimal GetCateringdiscountByPax(int paxcount)
        {
            decimal amount = 0;
            var cateringdiscount = _dbEntities.CateringDiscounts.FirstOrDefault(x => x.DiscPaxMin <= paxcount && x.DiscPaxMax >= paxcount);

            if (cateringdiscount != null)
            {
                amount = (decimal) cateringdiscount.Amount;
            }

            return amount;
        }


        public TransactionDetailsViewModel GetTransactionStatementAccountById(int transid)
        {
            return new TransactionDetailsViewModel
            {
                transactionId = transid,
                PackageAmount = this.GetAccountTotalByTransId(transid)

            };
        }

    }
}