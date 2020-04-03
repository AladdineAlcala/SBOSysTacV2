﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SBOSysTac.HtmlHelperClass;
using SBOSysTac.ViewModel;
using SBOSysTac.Models;

namespace SBOSysTac.Controllers
{
    public class HomeController : Controller
    {
        private PegasusEntities _dbcontext;
        private BookingPaymentsViewModel bookingPayments = new BookingPaymentsViewModel();

        public HomeController()
        {
            _dbcontext = new PegasusEntities();
        }
        public ActionResult Index()
        {
            
            if (Request.IsAuthenticated)
            {
                if (User.IsInRole("admin") || (User.IsInRole("superadmin")))
                {
                    return RedirectToAction("DashBoard", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Events");
                }
              
            }
            else
            {
                return View("~/Views/Home/Unsigned.cshtml");
            }
        }

        public ActionResult IndexUnsigned()
        {
            return View();
        }
        public ActionResult DashBoard()
        {
            ViewBag.FormTitle = "DashBoard";


            IndexViewModel indexView = new IndexViewModel();


            return View(indexView);
        }

        public ActionResult UnauthorizedAccess()
        {


            return View();
        }

        public ActionResult GetDataToChart(int thisYear)
        {
         

            var bookings = (from b in _dbcontext.Bookings
                group b by new
                {
                    year = b.transdate.Value.Year,
                    month = b.transdate.Value.Month
                }
                into g
                select new
                {
                    _year = g.Key.year,
                    _month = g.Key.month,
                    _count = g.Count()
                }).AsEnumerable().Select(g => new
            {
                Year = g._year,
                Period = g._month,
                Count = g._count
            }).Where(x => x.Year == thisYear).OrderBy(x => x.Period).ToList();

            //foreach (var b in bookings)
            //{
            //    datapointlist.Add(b);
            //}



            return Json(bookings, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetOrderCountSelected()
        {

            var menusOrderCount = (from b in _dbcontext.Book_Menus
                join m in _dbcontext.Menus on b.menuid equals m.menuid
                join c in _dbcontext.CourseCategories on m.CourserId equals c.CourserId
                group new {b, m, c} by new {b.menuid, m.menu_name, c.Course}
                into g
                select new
                {
                    MenuId = g.Key.menuid,
                    Course = g.Key.Course,
                    MenuName = g.Key.menu_name,
                    CountMenuOrder = g.Count()
                }).OrderByDescending(x => x.CountMenuOrder).Take(10).ToList();

            menusOrderCount.RemoveAll(x => x.Course.Contains("Rice"));
            menusOrderCount.RemoveAll(x => x.Course.Contains("Pineapple"));
            menusOrderCount.RemoveAll(x => x.Course.Contains("Soda"));

            return Json(menusOrderCount, JsonRequestBehavior.AllowGet);
        }

        [HttpGet,ChildActionOnly]
        public ActionResult BookingsTodayPartial()
        {


            return PartialView("_bookingsPartial");
        }


        public ActionResult GetBookingsToday()
        {
            List<CustomerBookingsViewModel> lst = new List<CustomerBookingsViewModel>();
            List<Booking> listbookings = new List<Booking>();

            DateTime dtoday=DateTime.Now;

            //var bookviewmodel=new BookingsViewModel();

            //listofBookingsToday = bookviewmodel.GetListofBookings().ToList()
            //    .Where(x => DbFunctions.TruncateTime(x.transdate) ==DbFunctions.TruncateTime(dtoday)).ToList();

            List<CustomerBookingsViewModel> cusbooking_Schedule = new List<CustomerBookingsViewModel>();

            try
            {

             
             
                listbookings = _dbcontext.Bookings.Where(
                    b =>DbFunctions.TruncateTime(b.startdate) == DbFunctions.TruncateTime(dtoday)).ToList();

                lst = (from l in listbookings
                    select new CustomerBookingsViewModel()
                    {
                        transId = l.trn_Id,
                        cusId = l.c_Id,
                        cusfullname = Utilities.getfullname_nonreverse(l.Customer.lastname, l.Customer.firstname, l.Customer.middle),
                        occasion = l.occasion,
                        venue = l.venue,
                        bookdatetime = l.startdate,
                        package = l.Package.p_descripton,
                        packageDue = bookingPayments.Get_TotalAmountBook(l.trn_Id),
                        isServe = Convert.ToBoolean(l.serve_stat)

                    }).ToList();

               


            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                throw;
            }
            



            return Json(new {data=lst}, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            _dbcontext.Dispose();
        }

    }

   
}