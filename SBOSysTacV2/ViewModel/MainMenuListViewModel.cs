using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class MainMenuListViewModel
    {
        public string menuId { get; set; }
        public string menu_name { get; set; }
        public int courseid { get; set; }
        public string course { get; set; }
        public bool? isMainMenu { get; set; }
       

        [OutputCache(Duration = 3600,VaryByParam = "none")]
        public IEnumerable<MainMenuListViewModel> ListofMainMenu()
        {
            List<MainMenuListViewModel> mainmenulist=new List<MainMenuListViewModel>();

            var _dbentities=new PegasusEntities();

            try
            {

                mainmenulist = (from m in _dbentities.Menus
                    join c in _dbentities.CourseCategories
                    on m.CourserId equals c.CourserId
                    select new MainMenuListViewModel()
                    {
                        menuId = m.menuid,
                        menu_name = m.menu_name,
                        courseid=c.CourserId,
                        course = c.Course,
                        isMainMenu = c.Main_Bol

                    }).OrderBy(x=>x.course).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            _dbentities.Dispose();

            return mainmenulist;
        }

        public bool Check_BarCourseCategory(int id)
        {
            var _dbentities = new PegasusEntities();

            return _dbentities.CourseCategories.FirstOrDefault(x => x.CourserId == id).Course.ToLower().Contains("/");
        }


        public List<KeyValuePair<int,string>> MainMenuList_with_Bar(int id)
        {
            const char separator = '/';
            var list = new List<KeyValuePair<int, string>>();

            var _dbentities = new PegasusEntities();
            var coursecatList = _dbentities.CourseCategories.ToList();


            var coursecat = coursecatList.Single(t => t.CourserId == id);
           
            var removeList = coursecatList.FindAll(t => t.Course.Contains(separator));

            var coursecat_arr = coursecat.Course.ToLower().Split(separator);

            if (removeList != null)
            {
                coursecatList.RemoveAll(item => removeList.Contains(item));
            }


            // var courseCat = coursecatList.Where(t2 => coursecat_arr.Count(t1 => t2.Course.Contains(t1)) == 0);


            if (coursecat_arr.Length > 0)
            {
                foreach (var course in coursecat_arr)
                {
                    var _course = course.Trim();

                    //var courseCat=(from c in _dbentities.CourseCategories where c.Course.Contains(_course) select c).ToList();
                    var courseCat = coursecatList.FirstOrDefault(t => t.Course.ToLower().Contains(_course));

                    list.Add(new KeyValuePair<int, string>(courseCat.CourserId, courseCat.Course));

                }

            }

            _dbentities.Dispose();

            return list;
        }

    }
}