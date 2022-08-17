using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using SBOSysTacV2.Models;
using SBOSysTacV2.HtmlHelperClass;

namespace SBOSysTacV2.ViewModel
{
    public class CourseMenuViewModel
    {
        private PegasusEntities _dbEntities = new PegasusEntities();

        [Display(Name = "Menu ID:")]
        public string menu_Id { get; set; }
        [AllowHtml]
        [Display(Name = "Menu Descripton:")]
        [Required(ErrorMessage = "Menu description Required")]
        public string menudesc { get; set; }
        [AllowHtml]
        public string coursecategory { get; set; }
        [Display(Name = "Note:")]
        public string Note { get; set; }
        public DateTime? dateAdded { get; set; }

        [Display(Name = "Price per Serve:")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal? price { get; set; } =0;

        [Display(Name = "Course Category:")]
        [Required(ErrorMessage = "Course Category Required")]
        public int? courseId { get; set; }

        public string picUrl { get; set; }

        [AllowExtension(Extensions = "png,jpg,jpeg", ErrorMessage = "Please select only Supported Files .png |.jpg .jpeg")]
        public HttpPostedFileBase UploadImage { get; set; }

        public IEnumerable<SelectListItem> coursecategory_list { get; set; }

        [Display(Name = "Dept.-Incharge:")]
        [Required(ErrorMessage = "Department Incharge Required")]
        public int? deptId { get; set; }
        public string departmentincharge { get; set; }
        public IEnumerable<SelectListItem> deptincharge_list { get; set; }


        //=============================================================================

       


        public IEnumerable<SelectListItem> Get_CourseListItems()
        {

            var listofcoursecategories = _dbEntities.CourseCategories.AsEnumerable().Select(x => new SelectListItem
            {
                Value = x.courseId.ToString(),
                Text = x.Course
            }).ToList();

            return new SelectList(listofcoursecategories, "Value", "Text");
        }


        public IEnumerable<CourseMenuViewModel> GetListofCourseMenu()
        {
            IEnumerable<CourseMenuViewModel> coursemenuviewmodelList;

            try
            {
                coursemenuviewmodelList = (from m in _dbEntities.Menus
                    select new CourseMenuViewModel()
                    {
                        menu_Id = m.menuid,
                        menudesc = m.menu_name,
                        departmentincharge = m.Department.deptName,
                        coursecategory = m.CourseCategory.Course,
                        price = m.price,
                        Note = m.note,
                        dateAdded = m.date_added,
                        courseId = m.courseId,
                        deptId = m.deptId,
                        picUrl = m.image
                    }).ToList();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return coursemenuviewmodelList;
        }


        public bool is_Menu_hasBooking(string menuId)
        {
            bool has_existingbook = false;

            using (var dbentities = new PegasusEntities())
            {
                var list = (from b in dbentities.Bookings
                    join bm in dbentities.Book_Menus on b.trn_Id equals bm.trn_Id
                    join m in dbentities.Menus on bm.menuid equals m.menuid
                    where m.menuid==menuId
                    select new
                    {
                        menuId =m.menuid
                    }).ToList();


                if (list.Any())
                {

                    has_existingbook = true;
                }


            }

            return has_existingbook;
        }

    }
}