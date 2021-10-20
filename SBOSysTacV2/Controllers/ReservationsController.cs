using SBOSysTacV2.Models;
using SBOSysTacV2.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SBOSysTacV2.HtmlHelperClass;
using System.Data.Entity.Validation;

namespace SBOSysTacV2.Controllers
{
    public class ReservationsController : Controller
    {
        // GET: Reservations
        private PegasusEntities dbEntities;
        private ReservationViewModel rV = new ReservationViewModel();
        public ReservationsController()
        {
            dbEntities = new PegasusEntities();
        }

        // GET: Reservation
        public ActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public ActionResult Get_ReservationsList()
        {
            List<ReservationViewModel> listofReservations = new List<ReservationViewModel>();
            listofReservations = rV.GetAll_Reservations().Where(x => x.resStat == false).ToList();


            return Json(new { data = listofReservations }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult CreateReservation()
        {
            //var newBooking = new ReservationViewModel()
            //{
            //    reserveDate = DateTime.Now

            //};

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReservation(ReservationViewModel bookingreserve)
        {
            if (!ModelState.IsValid) return View(bookingreserve);
            //bool success = false;

            try
            {
                var reservation = new Reservation()
                {
                    c_Id = bookingreserve.customerId,
                    resDate = bookingreserve.reserveDate,
                    noofPax =Convert.ToInt32(bookingreserve.noofperson),
                    occasion = bookingreserve.occasion,
                    eventVenue =bookingreserve.eventVenue,
                    reserveStat = false

                };


                dbEntities.Reservations.Add(reservation);
                dbEntities.SaveChanges();
                
                //success = true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            //return View("Index");
           return Json(new { success = true },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Remove_Reservation(int reservationId)
        {
            bool success = false;

            var Sel_reservation = dbEntities.Reservations.Find(reservationId);


            try
            {
                if (Sel_reservation != null)
                {
                    dbEntities.Reservations.Remove(Sel_reservation);
                    dbEntities.SaveChanges();

                    success = true;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Json(new {success=success}, JsonRequestBehavior.AllowGet);
        }
      

        [HttpGet]
        public ActionResult ModifyReservation(int reservationId)
        {
          
         
            var reservation = dbEntities.Reservations.Find(reservationId);

            var reservationViewModel = new ReservationViewModel();
            Customer customer = new Customer();
            customer = dbEntities.Customers.FirstOrDefault(x => x.c_Id == reservation.c_Id);

            if (reservation != null)
            {
                reservationViewModel = new ReservationViewModel()
                {
                    reservationId = reservation.resId,
                    customerId = reservation.c_Id,
                    fullname =Utilities.getfullname(customer.lastname,customer.firstname,customer.middle),
                    reserveDate = reservation.resDate,
                    noofperson = reservation.noofPax,
                    occasion = reservation.occasion,
                    eventVenue = reservation.eventVenue

                };

            }
           

            return View(reservationViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifyReservation(ReservationViewModel modifyreserrvation)
        {

            if (!ModelState.IsValid) return View(modifyreserrvation);
            bool success = false;

            try
            {
                var reservation = new Reservation()
                {
                    resId = Convert.ToInt32(modifyreserrvation.reservationId),
                    c_Id = modifyreserrvation.customerId,
                    resDate = modifyreserrvation.reserveDate,
                    noofPax = Convert.ToInt32(modifyreserrvation.noofperson),
                    occasion = modifyreserrvation.occasion,
                    eventVenue = modifyreserrvation.eventVenue,
                    reserveStat = false

                };
                dbEntities.Reservations.Attach(reservation);
                dbEntities.Entry(reservation).State = EntityState.Modified;
                dbEntities.SaveChanges();

                success = true;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Json(new {success = success}, JsonRequestBehavior.AllowGet);
            //return View("Index");
        }

        protected override void Dispose(bool disposing)
        {
            dbEntities.Dispose();
        }
    }
}