using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class MainMenuListViewModel
    {
        public int menuNo { get; set; }
        public string menuId { get; set; }
        public string menu_name { get; set; }
        public int courseid { get; set; }
        public string course { get; set; }
        public decimal price { get; set; }
        public bool? isMainMenu { get; set; }
       

        //[OutputCache(Duration = 3600,VaryByParam = "none")]
        public IEnumerable<MainMenuListViewModel> ListofMainMenu()
        {
            List<MainMenuListViewModel> mainmenulist=new List<MainMenuListViewModel>();

            var _dbentities=new PegasusEntities();

            try
            {

                mainmenulist = (from m in _dbentities.Menus
                    join c in _dbentities.CourseCategories
                    on m.courseId equals c.courseId
                    select new MainMenuListViewModel()
                    {
                        menuId = m.menuid,
                        menu_name = m.menu_name,
                        courseid=c.courseId,
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

            return _dbentities.CourseCategories.FirstOrDefault(x => x.courseId == id).Course.ToLower().Contains("/");
        }


        public List<KeyValuePair<int,string>> MainMenuList_with_Bar(int id)
        {
        
            var list = new List<KeyValuePair<int, string>>();

            var _dbentities = new PegasusEntities();
            var coursecatList = _dbentities.CourseCategories.ToList();


            var coursecat = coursecatList.Single(t => t.courseId == id);
           
            var removeList = coursecatList.FindAll(t => t.Course.Contains(Utilities.Separator));

            var coursecat_arr = coursecat.Course.ToLower().Split(Utilities.Separator);
             
            if (removeList.Count > 0)
            {
                coursecatList.RemoveAll(item => removeList.Contains(item));
            }


            if (coursecat_arr.Length > 0)
            {
                list.AddRange(from item in coursecat_arr select coursecatList.FirstOrDefault(t => t.Course.ToLower().Trim().Contains(item.ToLower().Trim())) into m where m != null select new KeyValuePair<int, string>(m.courseId, m.Course));

                //list.AddRange(from course in coursecat_arr select course.Trim() into _course select coursecatList.FirstOrDefault(t => t.Course.ToLower().Contains(_course.ToLower().Trim())) into courseCat select new KeyValuePair<int, string>(courseCat.courseId, courseCat.Course));
            }

            //if (list.Count <= 0)
            //{
            //    //var m = _dbentities.CourseCategories.ToList().Find(t => t.courseId == id);

            //    list.AddRange(new[] {new KeyValuePair<int, string>(, m.Course)});

            //}

            return list;
        }


        public IEnumerable<MainMenuListViewModel> GetBookMenuCourseByTransId(int transId)
        {
            var bookmenucourseList = new List<MainMenuListViewModel>();


            try
            {
                using (var _dbentities = new PegasusEntities())
                {
                    bookmenucourseList = (from bm in _dbentities.Book_Menus
                        join m in _dbentities.Menus on bm.menuid equals m.menuid
                        join c in _dbentities.CourseCategories
                            on m.courseId equals c.courseId
                        where bm.trn_Id == transId
                        select new MainMenuListViewModel()
                        {
                            menuNo = bm.No,
                            menuId = m.menuid,
                            menu_name = m.menu_name,
                            courseid = c.courseId,
                            course = c.Course,
                            isMainMenu = c.Main_Bol

                        }).OrderBy(x => x.course).ToList();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return bookmenucourseList;
        }


        public IEnumerable<MainMenuListViewModel> GetAllMenu_By_Course( List<CourseCategory> courseList)
        {
            var listofMenu = new List<MainMenuListViewModel>();

            if (courseList.Count > 0)
            {
                using (var _dbentities = new PegasusEntities())
                {

                    //var courses = _dbentities.CourseCategories.ToList().FindAll(t2 => courseList.Any(t1=>t1.courseId==t2.courseId));

                    listofMenu = (from c in courseList
                        join m in _dbentities.Menus on c.courseId equals m.courseId
                        select new MainMenuListViewModel()
                        {
                            menuId = m.menuid,
                            menu_name = m.menu_name,
                            courseid = c.courseId,
                            course = c.Course,
                            isMainMenu = c.Main_Bol

                        }).OrderBy(order => order.menu_name).ToList();

                }
            }

           

            return listofMenu;
        }

        

    }
}