using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class ReservationViewModel
    {
        public int? reservationId { get; set; }
        public int customerId { get; set; }
        [Display(Name = "Customer:")]
        [Required(ErrorMessage = "Customer Name Required")]
        public string fullname { get; set; }
        [Display(Name = "Date Reserve:")]
        [Required(ErrorMessage = "Reservation Date Required")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MMM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime reserveDate { get; set; }
        [Display(Name = "No.of Pax:")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Pls. enter valid number format")]
        public int? noofperson { get; set; }
        [Display(Name = "Occassion:")]
        public string occasion { get; set; }
        [Display(Name = "Event Venue:")]
        public string eventVenue { get; set; }

        public bool resStat { get; set; }


        public IEnumerable<ReservationViewModel> GetAll_Reservations()
        {
            List<ReservationViewModel> listreservations=new List<ReservationViewModel>();
            PegasusEntities dbEntities=new PegasusEntities();

            try
            {
                var list = (from r in dbEntities.Reservations select r).ToList();

                listreservations = (from s in list
                    select new ReservationViewModel()
                    {
                        reservationId = s.resId,
                        customerId = s.c_Id,
                        fullname = Utilities.Getfullname(s.Customer.lastname,s.Customer.firstname,s.Customer.middle),
                        noofperson = s.noofPax,
                        occasion =s.occasion,
                        reserveDate = s.resDate,
                        eventVenue = s.eventVenue,
                        resStat = s.reserveStat

                    }).ToList();

            }
            catch (Exception)
            {

                throw;
            }


            return listreservations.OrderBy(x => x.reserveDate);
        }

    }
}