using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class Course_PackageSelectionViewModel
    {
        public int courseId { get; set; }
        public string coursename { get; set; }
        public bool isSelected { get; set; }
        public int packageId { get; set; }

        public IEnumerable<Course_PackageSelectionViewModel> GetAllCoursesforPackage(int packageId)
        {
            var dbcontext=new PegasusEntities();

            int _packageid = packageId;


            var coursecat = (from cc in dbcontext.CourseCategories select cc).ToList();

            var listofcourses = (from cc in coursecat select new Course_PackageSelectionViewModel()
                                {
                                    courseId = cc.courseId,
                                    coursename = cc.Course,
                                    isSelected = CheckCourseisSelected(_packageid,cc.courseId)
                  
                                })
                                .ToList();

            dbcontext.Dispose();

            return listofcourses;

        }


        public bool CheckCourseisSelected(int packageId,int courseId)
        {
            var dbcontext = new PegasusEntities();

            int _pId;
            int _cId;
            bool is_selected = false;

            _pId = packageId;
            _cId = courseId;

            var course_exist = dbcontext.PackageBodies.Any(x => x.courseId == _cId && x.p_id == _pId);

            if (course_exist)
            {
                is_selected = true;
            }


            dbcontext.Dispose();

            return is_selected;
        }
    }
}