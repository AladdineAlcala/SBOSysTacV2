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
        public IEnumerable<AddonsViewModel> BookAddOns { get; set; }

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


        public bool CheckCourseHasBar(string coursename)
        {
            return coursename.Contains(Utilities.Separator);
        }


        public string GetMenusForCourseHasBar(int trnId, string course,ref int? _menuNo)
        {

            var mainmenu_viewmodel = new MainMenuListViewModel();
            var _courselist=new List<KeyValuePair<int, string>>();
            _menuNo =0;

            try
            {
                var splitCourse = course.ToLower().Split(Utilities.Separator);
                var _course = _dbEntities.CourseCategories.ToList();

                var removeList = _course.FindAll(t => t.Course.Contains(Utilities.Separator));

                //    .Where(t1 => splitCourse.Any(t2 => t1.Course.ToLower() == t2.ToLower().Trim())).ToList();
                _ = _course.RemoveAll(item => removeList.Contains(item));

                 _courselist = (from item in splitCourse select _course.FirstOrDefault(t => t.Course.ToLower().Contains(item.ToLower().Trim())) into c where c != null select new KeyValuePair<int, string>(c.CourserId, c.Course)).ToList();

                var bookmenucourse = mainmenu_viewmodel.GetBookMenuCourseByTransId(trnId);

                var bookmenu= bookmenucourse.FirstOrDefault(li => _courselist.Any(l2 => li.courseid == l2.Key));

                if (bookmenu != null)
                {
                    _menuNo = bookmenu.menuNo;
                    return bookmenu.menu_name;

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return string.Empty;

        }
    }


}