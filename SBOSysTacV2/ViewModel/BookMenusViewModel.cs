using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using Microsoft.Ajax.Utilities;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class BookMenusViewModel
    {
        public int menu_No { get; set; }
        public int transId { get; set; }
        [Required]
        public string menuId { get; set; }
        [Required(ErrorMessage = "Menu description Required")]
        public string menu_name { get; set; }
        public int courseid { get; set; }
        public string coursename { get; set; }
        public decimal serving { get; set; }
        public string servingstringpax { get; set; }
        public string dept { get; set; }
        public string menuImageFilename { get; set; }
        public string oldMenuId { get; set; }

        public IEnumerable<BookMenusViewModel> LisofMenusBook(int transid)
        {
            IOrderedEnumerable<BookMenusViewModel> bookMenusList;

            var _dbentities = new PegasusEntities();

            try
            {
                var bookmenus = (from bkm in _dbentities.Book_Menus select bkm).Where(t => t.trn_Id == transid).ToList();

                //join m in _dbentities.Menus on bkm.menuid equals m.menuid
                //select bkm);

                bookMenusList = (from bm in bookmenus
                    select new BookMenusViewModel()
                    {
                        menu_No = bm.No,
                        transId = (int)bm.trn_Id,
                        menuId = bm.menuid,
                        menu_name = bm.Menu.menu_name,
                        courseid = (int)bm.Menu.CourserId,
                        coursename = bm.Menu.CourseCategory.Course,
                        dept = bm.Menu.Department.deptName,
                        menuImageFilename = bm.Menu.image,
                        servingstringpax = get_servingstrpax(bm.No)
                    }).ToList().OrderBy(x => x.courseid);

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            _dbentities.Dispose();

            return bookMenusList;
        }

        //get serving count in bookmenus and display as string per number of pax
        public string get_servingstrpax(int bmNo)
        {
            string servingperpax = string.Empty;

            var dbcontext=new PegasusEntities();
          
            var bookMenus = dbcontext.Book_Menus.Find(bmNo);
            if (bookMenus != null)
            {
                //get booking no of pax
               
                if (bookMenus.serving != null && bookMenus.serving > 0)
                {
                    servingperpax = string.Format("{0} pax", bookMenus.serving);

                }
                else
                {
                    servingperpax = " ";
                }
            }
            return servingperpax;
        }

        public int get_totalselectedMainMenus(int transactionId)
        {
            int i = 0;

            var dbEntities=new PegasusEntities();
            try
            {
                var bookmenus = (from bm in dbEntities.Book_Menus
                    join m in dbEntities.Menus on bm.menuid equals m.menuid
                    join cc in dbEntities.CourseCategories on m.CourserId equals cc.CourserId 
                    where bm.trn_Id == transactionId && cc.Main_Bol==true
                    select new { sel_mainmenus=cc.Course }).ToList();

                i = bookmenus.Select(x => x.sel_mainmenus).Count();


            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return i;
        }

        public int Get_PackageMainMenusInt(int packageId)
        {
            int x = 0;
            var dbentities=new PegasusEntities();

            var list = (from p in dbentities.PackageBodies
                join cc in dbentities.CourseCategories on p.courseId equals cc.CourserId
                where p.p_id == packageId && cc.Main_Bol==true
                select new {mainmenus=cc.Course}).ToList();


            x = list.Select(c => c.mainmenus).Count();

            return x;

        }

        //public IEnumerable<BookMenusViewModel> GetLackingMenus(int transId)
        //{
        //    List<BookMenusViewModel> list=new List<BookMenusViewModel>();

        //    var dbentities=new PegasusEntities();

        //    return list;
        //}


        public static int GetTotalLackingMenus(int pid,int transid, PegasusEntities dbcontext)
        {
            //var dbcontext=new PegasusEntities();

            var packagemenucount = (from p in dbcontext.Packages where p.p_id==pid
                join pb in dbcontext.PackageBodies on p.p_id equals pb.p_id
                select new
                {
                    courseid = pb.courseId
                }).Count();


            var intmenusselected = (from bm in dbcontext.Book_Menus
                                    where bm.trn_Id == transid
                                    join m in dbcontext.Menus on bm.menuid equals m.menuid
                                    select new
                                    {
                                        _menu = bm.menuid
                                    }).Count();

            int count = packagemenucount - intmenusselected;

            if (count < 0) count = 0;

            return count=count<0?0:count;
        }
    }
}