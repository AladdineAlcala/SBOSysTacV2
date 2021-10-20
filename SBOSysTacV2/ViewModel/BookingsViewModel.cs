using SBOSysTacV2.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SBOSysTacV2.HtmlHelperClass;
using System.Data.Entity.Core;
using System.Web.Mvc;

namespace SBOSysTacV2.ViewModel
{
    public class BookingsViewModel
    {
        public int trn_Id { get; set; }
        public int? c_Id { get; set; }
        [Display(Name = "No. of Person:")]
        [Required(ErrorMessage = "No. of persons required")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Pls. enter valid number format")]
        public int? noofperson { get; set; }
        [Display(Name = "Event:")]
        [Required(ErrorMessage = "Event/Ocassion Name Required")]
        public string occasion { get; set; }
        [Display(Name = "Venue:")]
        //[Required(ErrorMessage = "Event Venue Required")]
        public string venue { get; set; }
        public int? typeofservice { get; set; }
        [Display(Name = "Date and Time:")]
        [Required(ErrorMessage = "Date Required")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:mm-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? startdate { get; set; }
        public DateTime? enddate { get; set; }
        public int pId { get; set; }
        [Required(ErrorMessage = "Package Required")]
        public string packagename { get; set; }
        public string packageType { get; set; }
        public bool? serve_status { get; set; }
        public bool iscancelled { get; set; }
        [Display(Name = "Event Motf:")]
        public string eventcolor { get; set; }
        [Display(Name = "Customer:")]
        //[CustomerCustomValidate]
        [Required(ErrorMessage = "Customer Name Required")]
        [Remote("IsCustomerRegistered","Bookings",HttpMethod ="Post",ErrorMessage ="Customer does not exist")]
        public string fullname { get; set; }
        public int? serviceId { get; set; }
        public string serviceType { get; set; }
        public string refernce { get; set; }
        public DateTime? transdate { get; set; }
        public bool apply_extendedAmount { get; set; }
        public string packagelocationapp { get; set; }
        public decimal amoutperPax { get; set; }
        public string b_createdbyUser { get; set; }
        public string b_createdbyUserName { get; set; }
        public DateTime b_updatedDate { get; set; }
        public IEnumerable<SelectListItem> Servicetype_ListItems { get; set; }
        public string selected_servicetype { get; set; }
        public int reservationId { get; set; }
       
        public int areaId { get; set; }
        public string area_desc { get; set; }
        [Display(Name = "Booking Type:")]
        [Required(ErrorMessage = "Booking Type Required")]
        public string booktypecode { get; set; }
        public string selectedbooktype { get; set; }
        public Dictionary<string,string> DictBooktype { get; set; }
        public int no_of_lackingMenus { get; set; }



        public List<BookingsViewModel> GetListofBookings()
        {
           var _dbcontext=new PegasusEntities();
            var app_user_context = new ApplicationUser.ApplicationDbContext();
            //  _entities.Configuration.ProxyCreationEnabled = false;

            //List<Booking> bookings = new List<Booking>();
            List<BookingsViewModel> bookingdetails = new List<BookingsViewModel>();

            try
            {
                

                var bookings = (from c in _dbcontext.Bookings where c.serve_stat==false && c.is_cancelled==false select c).ToList();
                var user = app_user_context.Users.ToList();


                bookingdetails = (from b in bookings
                                let _user = user.Find(x => x.Id==b.b_createdbyUser)
                                where _user!= null
                                join s in _dbcontext.ServiceTypes on b.typeofservice equals s.serviceId 
                                select new BookingsViewModel
                                {
                                    trn_Id = b.trn_Id,
                                    c_Id = b.c_Id,
                                    noofperson = b.noofperson,
                                    occasion = b.occasion,
                                    packagename = b.Package.p_descripton,
                                    packageType = b.Package.p_type.Trim(),
                                    amoutperPax =Convert.ToDecimal(b.Package.p_amountPax),
                                    venue = b.venue,
                                    typeofservice = b.typeofservice,
                                    startdate = b.startdate,
                                    enddate = b.enddate,
                                    transdate = b.transdate,
                                    serve_status = b.serve_stat,
                                    serviceType = s.servicetypedetails,
                                    eventcolor = b.eventcolor,
                                    pId = Convert.ToInt32(b.p_id),
                                    fullname = Utilities.getfullname(b.Customer.lastname, b.Customer.firstname, b.Customer.middle),
                                    b_createdbyUser = b.b_createdbyUser,
                                    b_createdbyUserName = _user.UserName,
                                    refernce = b.reference,
                                    apply_extendedAmount = (bool) b.apply_extendedAmount,
                                    b_updatedDate = Convert.ToDateTime(b.b_updatedDate),
                                    iscancelled =Convert.ToBoolean(b.is_cancelled),
                                    booktypecode = !string.Equals(b.booktype, null, StringComparison.Ordinal) ?b.booktype:"",
                                    no_of_lackingMenus = BookMenusViewModel.GetTotalLackingMenus(Convert.ToInt32(b.p_id),b.trn_Id,_dbcontext)

                                }).ToList();


            //}).Where(x=>x.serve_status==false).OrderBy(d => d.startdate).ToList();

            }
            catch (EntityCommandExecutionException eceex)
            {
                if (eceex.InnerException != null)
                {
                    throw eceex.InnerException;
                }
                throw;
            }

            app_user_context.Dispose();
            _dbcontext.Dispose();
            

            return bookingdetails;


        }

        public BookingsViewModel GetListofBookings(int _transId)
        {
            var _dbcontext = new PegasusEntities();
            var app_user_context = new ApplicationUser.ApplicationDbContext();


            //  _entities.Configuration.ProxyCreationEnabled = false;


            //List<Booking> bookings = new List<Booking>();
            BookingsViewModel bookingdetails = new BookingsViewModel();

            try
            {
               var booking = (from c in _dbcontext.Bookings where c.trn_Id == _transId select c).ToList();

                var user = app_user_context.Users.ToList();

                bookingdetails = (from b in booking
                                  let _user = user.Find(x => x.Id == b.b_createdbyUser)
                                  where _user != null
                                  join s in _dbcontext.ServiceTypes on b.typeofservice equals s.serviceId
                                  select new BookingsViewModel
                                  {
                                      trn_Id = b.trn_Id,
                                      c_Id = b.c_Id,
                                      noofperson = b.noofperson,
                                      occasion = b.occasion,
                                      packagename = b.Package.p_descripton,
                                      packageType = b.Package.p_type.Trim(),
                                      amoutperPax = Convert.ToDecimal(b.Package.p_amountPax),
                                      venue = b.venue,
                                      typeofservice = b.typeofservice,
                                      startdate = b.startdate,
                                      enddate = b.enddate,
                                      transdate = b.transdate,
                                      serve_status = b.serve_stat,
                                      serviceType = s.servicetypedetails,
                                      eventcolor = b.eventcolor,
                                      pId = Convert.ToInt32(b.p_id),
                                      fullname = Utilities.getfullname(b.Customer.lastname, b.Customer.firstname, b.Customer.middle),
                                      b_createdbyUser = b.b_createdbyUser,
                                      b_createdbyUserName = _user.UserName,
                                      refernce = b.reference,
                                      apply_extendedAmount = (bool)b.apply_extendedAmount,
                                      b_updatedDate = Convert.ToDateTime(b.b_updatedDate),
                                      iscancelled = Convert.ToBoolean(b.is_cancelled),
                                      booktypecode = !string.Equals(b.booktype, null, StringComparison.Ordinal) ? b.booktype : "",
                                      no_of_lackingMenus = BookMenusViewModel.GetTotalLackingMenus(Convert.ToInt32(b.p_id), b.trn_Id, _dbcontext)

                                  }).Single();


                //}).Where(x=>x.serve_status==false).OrderBy(d => d.startdate).ToList();

            }
            catch (EntityCommandExecutionException eceex)
            {
                if (eceex.InnerException != null)
                {
                    throw eceex.InnerException;
                }
                throw;
            }

            app_user_context.Dispose();
            _dbcontext.Dispose();

            return bookingdetails;


        }

        public IEnumerable<SelectListItem> GetServiceType_SelectListItems()
        {
            var _entities = new PegasusEntities();

            var servicetypeList = _entities.ServiceTypes.AsEnumerable().Select(x => new SelectListItem
            {
                Value = x.serviceId.ToString(),
                Text = x.servicetypedetails
            }).ToList();

            return new SelectList(servicetypeList, "Value", "Text");
        }

    


        public Dictionary<string, string> GetDictBookingType()
        {
           var dict=new Dictionary<string,string>()
           {
               {"Inside","ins" },
               {"Outside","out" }
           };

            return dict;
        }


        public void SetCancelBooking(int transId)
        {
            var dbcontext=new PegasusEntities();

            var booking = dbcontext.Bookings.FirstOrDefault(x => x.trn_Id == transId);
          

            try
            {
                if (booking != null)
                {
                    booking.is_cancelled = true;

                    dbcontext.Bookings.Attach(booking);
                    dbcontext.Entry(booking).Property(x => x.is_cancelled).IsModified = true;
                    dbcontext.SaveChanges();

                  
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

          
        }

    }


}