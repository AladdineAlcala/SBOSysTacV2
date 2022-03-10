using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.ViewModel;
using SBOSysTacV2.Models;
using SBOSysTacV2.ServiceLayer;

namespace SBOSysTacV2.Controllers
{
    public class HomeController : Controller
    {
        private PegasusEntities _dbcontext;
        private BookingPaymentsViewModel bookingPayments = new BookingPaymentsViewModel();
        static Func<int, decimal> _getBookingAmount = BookingsService.Get_TotalAmountBook;
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
                join c in _dbcontext.CourseCategories on m.courseId equals c.courseId
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
                        cusfullname = Utilities.Getfullname_nonreverse(l.Customer.lastname, l.Customer.firstname, l.Customer.middle),
                        occasion = l.occasion,
                        venue = l.venue,
                        bookdatetime = l.startdate,
                        package = l.Package.p_descripton,
                        packageDue = _getBookingAmount(l.trn_Id),
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

        
        /// <summary>
        /// Get All Images in the Content/Images/RandomImages Folder and Display in the main form
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [OutputCache(Duration = 60, VaryByParam = "none")]
        public ActionResult GetImages()
        {
            var physicalPath = Server.MapPath("~/Content/dist/img/RandomImages/");
            string[] pictureFiles =  Directory.GetFiles(physicalPath, "*.jpg");


            return Json(new {data= pictureFiles.Select(t => Path.GetFileName(t)).ToArray()}, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            _dbcontext.Dispose();
        }

    }   

   
}