using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.Models;
using SBOSysTacV2.ViewModel;

namespace SBOSysTacV2.Controllers
{
    [Authorize]
    public class RecievablesController : Controller
    {
        private PegasusEntities _dbEntities;

        private TransRecievablesViewModel tr=new TransRecievablesViewModel();
        public RecievablesController()
        {
            _dbEntities=new PegasusEntities();
        }

        // GET: Recievables
        public ActionResult Index()
        {


            return View();
        }

        [HttpGet]
        public ActionResult GetPaymentListView(int cusId)
        {

            var customers = (from c in _dbEntities.Customers select c).ToList();


            var customerviewmodel = (from cus in customers
                where cus.c_Id == cusId
                select new CustomerViewModel()
                {
                    customerId = cus.c_Id,
                    fullname = Utilities.getfullname(cus.lastname, cus.firstname, cus.middle)

                }).FirstOrDefault();

            return PartialView("GetPaymentListView",customerviewmodel);
        }



        [HttpGet]
        public ActionResult LoadBookingsByCustomer(int cusId)
        {
            List<TransRecievablesViewModel> recievablesList=new List<TransRecievablesViewModel>();

            try
            {
                var bookinglist = (from b in _dbEntities.Bookings.AsNoTracking() where b.c_Id == cusId select b).ToList();
                    
                    
                recievablesList = tr.GetAllRecievables(bookinglist).ToList();

                // recievablesList = (from r in list where r.cusId == cusId select r) as List<TransRecievablesViewModel>;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Json(new {data= recievablesList }, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
           _dbEntities.Dispose();
        }
    }
}