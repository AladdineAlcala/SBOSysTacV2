using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class CourseCategoryViewModel
    {
        [Display(Name = "COURSE ID:")]
        public int? courseId { get; set; }

        [Display(Name = "COURSE:")]
        [Required(ErrorMessage = "Course name Required")]
        public string Coursename { get; set; }
        [Display(Name = "NOTE:")]
        public string Note { get; set; }
        public bool Main_Bol { get; set; }

        //=== check if has transaction on list of book menus
        public bool isCoursehasBooking(int courseId)
        {
            bool hasexistingbook = false;

            using (var dbentities=new PegasusEntities())
            {
                var list = (from b in dbentities.Bookings
                    join bm in dbentities.Book_Menus on b.trn_Id equals bm.trn_Id
                    join m in dbentities.Menus on bm.menuid equals m.menuid
                    join c in dbentities.CourseCategories on m.courseId equals c.courseId
                    where c.courseId == courseId
                    select new
                    {
                        courseId=c.courseId
                    }).ToList();


                if (list.Any())
                {
                    hasexistingbook = true;
                }
              

            }

            return hasexistingbook;
        }

        //check course if has menus
        public List<CourseMenuViewModel> isCourseHasMenus(int courseId)
        {

       

            List<CourseMenuViewModel> list = new List<CourseMenuViewModel>();
            using (var dbentities=new PegasusEntities())
            {

                 list = (from m in dbentities.Menus
                    join c in dbentities.CourseCategories on m.courseId equals c.courseId where c.courseId== courseId
                    select new CourseMenuViewModel()
                    {
                        menu_Id = m.menuid,
                        courseId= c.courseId

                    }).ToList();
            }

            return list;

        }


        public IEnumerable<CourseCategory> GetCourseByPackageId(int packageId)
        {
            List<CourseCategory> list = new List<CourseCategory>();

            using (var _dbcontext = new PegasusEntities())
            {
                list = (from pb in _dbcontext.PackageBodies
                        join c in _dbcontext.CourseCategories on pb.courseId equals c.courseId
                    where pb.p_id == packageId
                    select c).ToList();

            }

            return list;
        }
    }
}