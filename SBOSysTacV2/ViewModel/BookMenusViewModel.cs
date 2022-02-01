using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web;
using Microsoft.Ajax.Utilities;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class BookMenusViewModel
    {
        public int? menu_No { get; set; }
        public int transId { get; set; }
        [Required]
        public string menuId { get; set; }
        [Required(ErrorMessage = "Menu description Required")]
        public string menu_name { get; set; }
        public int course_id { get; set; }
        public string coursename { get; set; }
        public decimal? serving { get; set; }
        public int servingpax { get; set; }
        public string dept { get; set; }
        public string menuImageFilename { get; set; }
        public string oldMenuId { get; set; }

        public decimal price { get; set; }=0;
        public DateTime createdDate { get; set; }=DateTime.UtcNow;
        public DateTime updatedDate { get; set; }=DateTime.UtcNow;



        public IEnumerable<BookMenusViewModel> ListOfMenusBook(int trnId)
        {
            IEnumerable<BookMenusViewModel> bookMenusList;

            try
            {

                var bookmenus = Get_Menu_on_PackageByTransId(trnId).ToList();

                bookMenusList = bookmenus.Select(bm => new BookMenusViewModel()
                {
                    transId = (int) bm.transId,
                    menu_No = bm.menu_No,
                    menuId = bm.menuId,
                    menu_name = bm.menu_name,
                    course_id = (int) bm.course_id,
                    coursename = bm.coursename,
                    dept = bm.dept,
                    menuImageFilename = bm.menuImageFilename,
                    serving = bm.serving,
                    servingpax = bm.servingpax,
                    price = bm.price
                }).ToList();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return bookMenusList;
        }


        public IEnumerable<BookMenusViewModel> ListOfMenusBook(PackageBookingViewModel pb)
        {
            List<BookMenusViewModel> bookMenusList = new List<BookMenusViewModel>();

            try
            {

                if (pb != null)
                {

                    foreach (var item in pb.PackageBody)
                    {
                        //Check caourseId if present in BookMenus Table
                        var bm = this.GetBookMenusByCourseId(item.courseId,pb.transactionId);


                        bookMenusList.Add(bm != null
                            ? this.GetBookMenusByMenuId(pb.Booking, bm.menuid)
                            : this.GetCourseByPackage(pb.Booking, item.courseId));
                    }




                }



            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return bookMenusList;
        }


        public BookMenusViewModel GetBookMenusByMenuId(Booking packageBooking,string menuid)
        {

            var bookmenu = new BookMenusViewModel();

            try
            {
                using (var _dbentities = new PegasusEntities())
                {
                    bookmenu =(from m in _dbentities.Menus
                        join bm in _dbentities.Book_Menus on m.menuid equals bm.menuid
                        where m.menuid==menuid
                        select new BookMenusViewModel()
                        {
                            transId = packageBooking.trn_Id,
                            menu_No = bm.No,
                            menuId = menuId,
                            menu_name = m.menu_name,
                            course_id = (int)m.courseId,
                            coursename = m.CourseCategory.Course,
                            dept = m.Department.deptName,
                            menuImageFilename = m.image,
                            serving = bm.serving,
                            servingpax = (int)packageBooking.noofperson,
                            price = m.price ?? 0

                        }).FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return bookmenu;
        }



        public BookMenusViewModel GetCourseByPackage(Booking packageBooking,int? courseid)
        {
            var bookmenu = new BookMenusViewModel();
            try
            {
                using (var _dbentities = new PegasusEntities())
                {
                    bookmenu = (from m in _dbentities.PackageBodies
                        join c in _dbentities.CourseCategories on m.courseId equals c.courseId
                        where m.courseId==courseid
                        select new BookMenusViewModel()
                        {
                            transId = packageBooking.trn_Id,
                            menu_No =null,
                            menuId = String.Empty,
                            menu_name = String.Empty,
                            course_id = (int)m.courseId,
                            coursename = m.CourseCategory.Course,
                            dept = String.Empty,
                            menuImageFilename = String.Empty,
                            serving =null,
                            servingpax = 0,
                            price =price

                        }).FirstOrDefault();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return bookmenu;

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

        public int get_totalselectedMainMenus(int trnId)
        {
            int i = 0;

            var dbEntities=new PegasusEntities();
            try
            {
                var bookmenus = (from bm in dbEntities.Book_Menus
                    join m in dbEntities.Menus on bm.menuid equals m.menuid
                    join cc in dbEntities.CourseCategories on m.courseId equals cc.courseId 
                    where bm.trn_Id == trnId && cc.Main_Bol==true
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
                join cc in dbentities.CourseCategories on p.courseId equals cc.courseId
                where p.p_id == packageId && cc.Main_Bol==true
                select new {mainmenus=cc.Course}).ToList();


            x = list.Select(c => c.mainmenus).Count();

            return x;

        }



        public static int GetTotalLackingMenus(int pid,int trnId, PegasusEntities dbcontext)
        {
            //var dbcontext=new PegasusEntities();

            var packagemenucount = (from p in dbcontext.Packages where p.p_id==pid
                join pb in dbcontext.PackageBodies on p.p_id equals pb.p_id
                select new
                {
                    courseid = pb.courseId
                }).Count();


            var intmenusselected = (from bm in dbcontext.Book_Menus
                                    where bm.trn_Id == trnId
                                    join m in dbcontext.Menus on bm.menuid equals m.menuid
                                    select new
                                    {
                                        _menu = bm.menuid
                                    }).Count();

            int count = packagemenucount - intmenusselected;

            if (count < 0) count = 0;

            return count <=0 ? 0:count;
        }


        public List<BookMenusViewModel> Get_Menu_on_PackageByTransId(int trnId)
        {
            List<BookMenusViewModel> listmenus = new List<BookMenusViewModel>();

            var dbEntities = new PegasusEntities();

            listmenus = dbEntities.Database.SqlQuery<BookMenusViewModel>("exec GetPackageBookMenus @trId ", new SqlParameter("@trId", trnId)).ToList();

            dbEntities.Dispose();


            return listmenus;
        }


        public List<BookMenusViewModel> Get_Menu_on_SnackPackageByTransId(int trnId)
        {
            List<BookMenusViewModel> listmenus = new List<BookMenusViewModel>();

            using (var dbEntities=new PegasusEntities())
            {
                var bookmenus = dbEntities.Book_Menus.Where(t => t.trn_Id == trnId).ToList();

                if (bookmenus.Count > 0) listmenus = dbEntities.Database.SqlQuery<BookMenusViewModel>("exec [dbo].[GetPackageSnacksBookMenus] @trId ", new SqlParameter("@trId", trnId)).ToList();



            }

            return listmenus;
        }

        public decimal ComputeAmountForSnacksByTransId(int trnId)
        {
            var menulist = this.Get_Menu_on_SnackPackageByTransId(trnId);

            return (decimal)menulist.Select(m => m.price * m.servingpax).Sum();
        }

        public Book_Menus GetBookMenusByCourseId(int? courseId,int transid)
        {
            var dbEntities = new PegasusEntities();

            var bookmenus = dbEntities.Book_Menus.FirstOrDefault(t => t.courseId == courseId && t.trn_Id==transid);

            dbEntities.Dispose();

            return bookmenus;
        }



    }
}