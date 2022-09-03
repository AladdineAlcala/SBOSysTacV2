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
        public IEnumerable<PackageBody> PackageBody { get; set; }
        public IEnumerable<BookMenusViewModel> BookMenuses { get; set; }
        public IEnumerable<AddonsViewModel> BookAddOns { get; set; }
        //public string controllerFlag { get; set; }



        public IEnumerable<PackageBookingViewModel> GetBookingDetails()
        {
            var _dbEntities = new PegasusEntities();
            List<PackageBookingViewModel> bookingList = new List<PackageBookingViewModel>();

            _dbEntities.Configuration.ProxyCreationEnabled = false;

            bookingList =(from b in _dbEntities.Bookings join p in _dbEntities.Packages on b.p_id equals p.p_id join pb in _dbEntities.PackageBodies on b.p_id equals pb.p_id
                 select new PackageBookingViewModel
               {
                   transactionId = b.trn_Id,
                   Booking = b,
                   Package = p,
                   PackageBody = new List<PackageBody> {pb}
                  
               })
                .ToList();

            _dbEntities.Dispose();

            return bookingList;
        }


        public PackageBookingViewModel GetBookingDetailById(int transid)
        {
            var _dbEntities = new PegasusEntities();
            //_dbEntities.Configuration.ProxyCreationEnabled = false;
            var bookDetails = new PackageBookingViewModel();

            try
            {
                bookDetails = (from b in _dbEntities.Bookings
                    where b.trn_Id.Equals(transid)
                    select new PackageBookingViewModel
                    {
                        transactionId = b.trn_Id,
                        Booking = b,
                        Package =b.Package,
                        PackageBody =b.Package.PackageBodies.ToList()
                    }).FirstOrDefault();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                _dbEntities.Dispose();
            }
           

            return bookDetails;
        }


        public bool VerifyPackagehasBookings(int packageId)
        {
            //bool has_existingBookings = false;
            var _dbEntities = new PegasusEntities();

            var listofpackage = (from b in _dbEntities.Bookings
                join p in _dbEntities.Packages on b.p_id equals p.p_id
                where p.p_id == packageId
                select new
                {
                    packageId=p.p_id
                }).ToList();


            _dbEntities.Dispose();

            return (listofpackage.Any()?true:false);
        }


        public bool CheckCourseHasBar(string coursename)
        {
            return coursename.Contains(Utilities.Separator);
        }


        public string GetMenusForCourseHasBar(int trnId, string course,ref int? _menuNo)
        {
            var _dbEntities = new PegasusEntities();

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

                 _courselist = (from item in splitCourse select _course.FirstOrDefault(t => t.Course.ToLower().Contains(item.ToLower().Trim())) into c where c != null select new KeyValuePair<int, string>(c.courseId, c.Course)).ToList();

                var bookmenucourse = mainmenu_viewmodel.GetBookMenuCourseByTransId(trnId);

                var bookmenu= bookmenucourse.FirstOrDefault(li => _courselist.Any(l2 => li.course_id == l2.Key));

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

            _dbEntities.Dispose();

            return string.Empty;

        }


        public Package GetPackageByTransaction_Id(int transactionId)
        {
            var package = new Package();

            using (var _dbEntities = new PegasusEntities())
            {
                package = (from p in _dbEntities.Bookings where p.trn_Id == transactionId select p.Package).Single() as Package;
            }

            return package;


        }


    }


}