using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class PackageBookingViewModel

    {
        public int transactionId { get; set; }
        public Booking Booking { get; set; }
        public Package Package { get; set; }
        public PackageBody PackageBody { get; set; }
        public IEnumerable<BookMenusViewModel> BookMenuses { get; set; }
        public IEnumerable<AddonsViewModel> AddOns { get; set; }

        private PegasusEntities _dbEntities=new PegasusEntities();
      
       

        public IEnumerable<PackageBookingViewModel> GetBookingDetails()
        {
            List<PackageBookingViewModel> bookingList = new List<PackageBookingViewModel>();

            _dbEntities.Configuration.ProxyCreationEnabled = false;

            bookingList =(from b in _dbEntities.Bookings join p in _dbEntities.Packages on b.p_id equals p.p_id join pb in _dbEntities.PackageBodies on b.p_id equals pb.p_id
                 select new PackageBookingViewModel
               {
                   transactionId = b.trn_Id,
                   Booking = b,
                   Package = p,
                   PackageBody = pb
                  
               })
                .ToList();

            return bookingList;
        }


        public bool VerifyPackagehasBookings(int packageId)
        {
            //bool has_existingBookings = false;

            var listofpackage = (from b in _dbEntities.Bookings
                join p in _dbEntities.Packages on b.p_id equals p.p_id
                where p.p_id == packageId
                select new
                {
                    packageId=p.p_id
                }).ToList();

            //if (listofpackage.Any())
            //{
            //    has_existingBookings = true;
            //}

            return (listofpackage.Any()?true:false);
        }

       
    }


}