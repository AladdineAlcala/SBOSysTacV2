using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.Models;
using SBOSysTacV2.ViewModel;

namespace SBOSysTacV2.ServiceLayer
{
    
    public class IncentivesService
    {
        public static DateTime DateFrom { get; set; }
        public static DateTime DateTo { get; set; }

        public PegasusEntities dbEntities;
        static Func<Booking, List<ICollection<BookAddonsDetail>>> _getAddonDetails = BookingAddonDetailsViewModel.GetAddonDetails;
        static Func<int, decimal> _getBookingAmount = BookingsService.Get_TotalAmountBook;
        public IncentivesService()
        {
            dbEntities = new PegasusEntities();
        }

        public static IEnumerable<CateringReportViewModel> GetCateringReport(IEnumerable<Booking> bookings)
        {
            //init_addons();

            return bookings.Select(x => new CateringReportViewModel()
            {

                EventDate = (DateTime)x.startdate,
                cId = x.Customer.c_Id,
                Client = Utilities.Getfullname(x.Customer.lastname, x.Customer.firstname, x.Customer.middle),
                Occasion = x.occasion,
                p_type = x.Package.p_type.TrimEnd() == PackageEnum.packageType.vip.ToString() || x.Package.p_type.TrimEnd() == PackageEnum.packageType.regular.ToString() ? "cat" : x.Package.p_type.TrimEnd(),
                Venue = x.venue,
                noofPax = (int)x.noofperson,
                PackageRate = x.Package.p_type.Trim() != "vip" ? (decimal)x.Package.p_amountPax - TransactionDetailsViewModel.GetCateringdiscountByPax((int)x.noofperson) : (decimal)x.Package.p_amountPax,
                Addons = x.BookingAddons.Any() ? string.Join(", ", x.BookingAddons.Select(t => t.Addondesc)) : String.Empty,
                AddonsTotal = x.BookingAddons.Any() ? AddonsViewModel.AddonsTotal(_getAddonDetails(x)) : 0,
                AmountPaid = x.Payments.Any() ? x.Payments.Select(t => Convert.ToDecimal(t.amtPay)).Sum() : 0,
                PaymentMode = x.Payments.Any() ? x.Payments.Select(t => t.pay_means).ToList().Aggregate((i, j) => i + "," + j != i ? j : "") : "---",
                Status = x.Payments.Any() ? "pd" : "unpd",
                iscancelled = (bool)x.is_cancelled,
                isDeletedTran = (bool)x.is_deleted


            }).Where(t => t.iscancelled == false && t.isDeletedTran == false && t.p_type == PackageEnum.packageType.cat.ToString() || t.p_type == PackageEnum.packageType.pm.ToString()).OrderBy(o => o.p_type);
        }

        public void GetIncentivesReport(DateTime _dateFrom, DateTime _dateTo)
        {
            DateFrom = _dateFrom;
            DateTo = _dateTo;

            var bookinglist = ShowBookingsFullyPaid(Paymentlist(), Pcondition);

            ContainerClass.CateringList = GetCateringReport(bookinglist).ToList();

        }
        public IEnumerable<Booking> ShowBookingsFullyPaid(List<PaymentLogDetailsViewModel> list, Func<PaymentLogDetailsViewModel, bool> condition)
        {
            var data = list.Where(condition);

            var bookings = dbEntities.Bookings.ToList()
                .Where(t => data.Select(x => x.transId).Contains(t.trn_Id));

            //dbEntities.Dispose();

            return bookings;
        }

        private Func<PaymentLogDetailsViewModel, bool> Pcondition = e => GetAmount(e) == true;

        public static bool GetAmount(PaymentLogDetailsViewModel paymentlog)
        {
            return (paymentlog.totalPayment - paymentlog.totalBookingsAmount == 0);
        }

        public IEnumerable<Payment> GetPaymentList()
        {
            return dbEntities.Payments.Where(CheckPaymentDate);
        }


        readonly Func<Payment, bool> CheckPaymentDate = t =>
            t.dateofPayment.Value.Date >= DateFrom.Date && t.dateofPayment.Value.Date <= DateTo.Date;

        public List<PaymentLogDetailsViewModel> Paymentlist()
        {
            List<Payment> listofpaymentLogs = new List<Payment>();
            var l = new IncentivesService();

            var recordedList = l.GetPaymentList();

            foreach (var item in recordedList)
            {
               listofpaymentLogs.Add(new Payment()
               {
                   trn_Id = (int)item.trn_Id,
                   dateofPayment = (DateTime) item.dateofPayment,
                   amtPay = item.amtPay
                   
               });
            }

            //create function with func paramter for transactions booking

            return PaymentLogs(listofpaymentLogs);

        }

        public static List<PaymentLogDetailsViewModel> PaymentLogs(List<Payment> paylist)
        {
            return (from p in paylist
                    group p by p.trn_Id
                    into g
                    orderby g.Key
                    select new PaymentLogDetailsViewModel()
                    {
                        transId = (int)g.Key,
                        paymentDate = (DateTime)g.Max(row => row.dateofPayment),
                        totalPayment = (decimal)g.Sum(row => row.amtPay),
                        totalBookingsAmount = _getBookingAmount((int)g.Key)

                    }).ToList();
        }

       

       
        
    }
}