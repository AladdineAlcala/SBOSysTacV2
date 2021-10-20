using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;
using SBOSysTacV2.HtmlHelperClass;

namespace SBOSysTacV2.ViewModel
{
    public class CustomerTransViewModel
    {
        public int transId { get; set; }
        public string customerfullname { get; set; }
        public string customeraddress { get; set; }
        public string booktypecode { get; set; }
        public string contactno { get; set; }
        public DateTime datetimesched { get; set; }
        public string event_name { get; set; }
        public int noofPax { get; set; }
        public string event_venue { get; set; }
        public string eventcolScheme { get; set; }
        public string typeofService { get; set; }
        public string packagedesc { get; set; }
        public decimal packageamount { get; set; }
        public decimal cateringdiscount { get; set; }
        public decimal dpA { get; set; }
        public decimal fpA { get; set; }

        private PegasusEntities dbEntities = new PegasusEntities();
      

        public IEnumerable<CustomerTransViewModel> GetContractDetails()
        {
            //  dbEntities.Configuration.ProxyCreationEnabled = false;




            IEnumerable<Booking> bookings = (from c in dbEntities.Bookings select c).ToList();
            List<CustomerTransViewModel> prn_Contract = new List<CustomerTransViewModel>();

            try
            {
                prn_Contract = (from booking in bookings
                    join sv in dbEntities.ServiceTypes on booking.typeofservice equals sv.serviceId
                    select new CustomerTransViewModel()
                    {
                        transId = booking.trn_Id,
                        customerfullname = Utilities.getfullname(booking.Customer.lastname, booking.Customer.firstname, booking.Customer.middle),
                        customeraddress = booking.Customer.address,
                        contactno = booking.Customer.contact1,
                        booktypecode = booking.booktype,
                        datetimesched = Convert.ToDateTime(booking.startdate),
                        event_name = booking.occasion,
                        noofPax = Convert.ToInt32(booking.noofperson),
                        event_venue = booking.venue,
                        eventcolScheme = booking.eventcolor,
                        typeofService = sv.servicetypedetails,
                        packagedesc = booking.Package.p_descripton,
                        packageamount = Convert.ToDecimal(booking.Package.p_amountPax)
                      

                    }).ToList();




            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return prn_Contract.ToList();
        }
    }
}