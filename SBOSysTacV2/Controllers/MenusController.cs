using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using SBOSysTacV2.Models;
using SBOSysTacV2.ViewModel;
using SBOSysTacV2.HtmlHelperClass;
using System.Data.Entity;
using System.IO;
using System.Net;

namespace SBOSysTacV2.Controllers
{
    public class MenusController : Controller
    {

        private PegasusEntities _dbEntities;
        private CourseMenuViewModel _coursemenuViewModel=new CourseMenuViewModel();
        private DepartmentViewModel  dept=new DepartmentViewModel();

        public MenusController()
        {

            _dbEntities=new PegasusEntities();
        }
        // GET: Menus
        public ActionResult Index()
        {
            var courseMenu = new CourseMenuViewModel
            {

                coursecategory_list =this._coursemenuViewModel.Get_CourseListItems()
            };

            ViewBag.FormTitle = "Menus Details";

            return View(courseMenu);
        }

        [HttpPost]
        public ActionResult loadDatatoTable()
        {

            var draw = Request.Unvalidated.Form.GetValues("draw").FirstOrDefault();

            var start = Request.Unvalidated.Form.GetValues("start").FirstOrDefault();

            var length = Request.Unvalidated.Form.GetValues("length").FirstOrDefault();

            var sortColumn = Request.Unvalidated.Form
                .GetValues("columns[" + Request.Unvalidated.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]")
                .FirstOrDefault();
            var sortColumnDir = Request.Unvalidated.Form.GetValues("order[0][dir]").FirstOrDefault();

            var menu = Request.Unvalidated.Form.GetValues("columns[2][search][value]").FirstOrDefault();

            var courseCategoryid = Request.Unvalidated.Form.GetValues("columns[3][search][value]").FirstOrDefault();


            int pageSize = length != null ? Convert.ToInt32(length) : 0;

            int skip = start != null ? Convert.ToInt16(start) : 0;
            int recordsTotal = 0;


            var menus = _coursemenuViewModel.GetListofCourseMenu();

            //Searching menu name
            if (!(string.IsNullOrEmpty(menu.Trim())))
            {
                menus = menus.Where(x => x.menudesc.ToLower().Contains(menu.Trim().ToLower()));
            }

            //Filter Course
            if (!(string.IsNullOrEmpty(courseCategoryid.Trim())))
            {
                menus = menus.Where(c => c.courseId == Convert.ToInt32(courseCategoryid));
            }
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
            {

                menus = menus.OrderBy(sortColumn + " " + sortColumnDir);

            }

            else
            {
                menus = menus.OrderBy(x => x.menu_Id);
            }

            recordsTotal = menus.Count();

            var data = menus.Skip(skip).Take(pageSize).ToList();

            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data }, JsonRequestBehavior.AllowGet);

        }


        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin, UserPermessionLevelEnum.user)]
        [HttpGet]
        public ActionResult CreateMenus()
        {
            ViewBag.FormTitle = "Create New Menus";

            var newcCourseMenuViewModel = new CourseMenuViewModel
            {
                menu_Id = Utilities.MenusCode_Generator(),
                coursecategory_list = _coursemenuViewModel.Get_CourseListItems(),
                deptincharge_list = dept.Get_MenuDepartmentInchargeListItems(),
                dateAdded = Convert.ToDateTime(DateTime.Today)


            };


            return View(newcCourseMenuViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMenus(CourseMenuViewModel courseMenu)
        {

            if (ModelState.IsValid)
            {

                string filename=String.Empty;

                if (courseMenu.UploadImage != null && courseMenu.UploadImage.ContentLength > 0)
                {
                    //filename = Path.GetFileName(courseMenu.UploadImage.FileName);
                    var extension = Path.GetExtension(courseMenu.UploadImage.FileName);
                    var namefile = Guid.NewGuid().ToString();
                    filename = namefile + extension;

                    var path = Path.Combine(Server.MapPath("~/Content/UploadedImages"), filename);

                    courseMenu.UploadImage.SaveAs(path);

                }

                try
                {
                    var newMenu = new Menu
                    {
                        menuid = courseMenu.menu_Id,
                        courseId = Convert.ToInt32(courseMenu.courseId),
                        menu_name = courseMenu.menudesc,
                        deptId = Convert.ToInt32(courseMenu.deptId),
                        note = courseMenu.Note,
                        image = filename,
                        price = courseMenu.price??0,
                        date_added = courseMenu.dateAdded


                    };

                    _dbEntities.Menus.Add(newMenu);
                    _dbEntities.SaveChanges();

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;

                }

                //return RedirectToAction("Index");

                return Json(new {success=true}, JsonRequestBehavior.AllowGet);

            }
            else
            {
                courseMenu.menu_Id = Utilities.MenusCode_Generator();
                courseMenu.coursecategory_list = courseMenu.Get_CourseListItems();
                courseMenu.deptincharge_list = dept.Get_MenuDepartmentInchargeListItems();


                return View(courseMenu);
            }
           

            
        }


        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin)]
        [HttpPost]
        public ActionResult RemoveMenu(string menuId)
        {
            bool success = false;

            string removemenudesc = String.Empty;
            try
            {
                if (!string.IsNullOrEmpty(menuId))
                {
                    var removemenu = _dbEntities.Menus.Find(menuId);

                    if (removemenu != null)
                    {
                        bool hasExistingBooking = _coursemenuViewModel.is_Menu_hasBooking(menuId);

                        if (hasExistingBooking == false)
                        {
                            _dbEntities.Menus.Remove(removemenu);
                            _dbEntities.SaveChanges();

                            removemenudesc = removemenu.menu_name;

                            success = true;

                         
                        }

                        else
                        {
                           
                            return Json(new { deletedMenu = removemenu.menu_name, success = false },
                                JsonRequestBehavior.AllowGet);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                //throw new HttpException(404, "Page Not Found");
                Console.WriteLine(e);
                throw;
            }

            return Json(new { success = success, deletedMenu = removemenudesc },
                JsonRequestBehavior.AllowGet);

        }


        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin, UserPermessionLevelEnum.admin)]
        [HttpGet]
        public ActionResult ModifyMenu(string menuId = null)
        {
            try
            {
                if (menuId != null)
                {
                   
                    var list = _coursemenuViewModel.GetListofCourseMenu().FirstOrDefault(x => x.menu_Id.Equals(menuId));

              
                    list.coursecategory_list = _coursemenuViewModel.Get_CourseListItems();
                    list.deptincharge_list = dept.Get_MenuDepartmentInchargeListItems();

                    if (list.picUrl != null)
                    {
                        var path = Path.Combine(Server.MapPath("~/Content/UploadedImages/"), list.picUrl);
                        ViewData["filepath"] = path;
                    }
                  

                    return View("ModifyMenu", list);
                }

            }
            catch (Exception)
            {
                throw new HttpException(404, "Page Not Found");
            }

            return RedirectToAction("Index");




        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModifyMenu(CourseMenuViewModel editedCourseMenuModel)
        {
  
            if (ModelState.IsValid)
            {
                try
                {
                    var modifyMenu = _dbEntities.Menus.FirstOrDefault(m => m.menuid == editedCourseMenuModel.menu_Id);



                    string filename = String.Empty;

                    var menu =
                        _dbEntities.Menus.AsNoTracking().FirstOrDefault(
                            x => x.menuid == editedCourseMenuModel.menu_Id.Trim());

                    filename = menu.image;

                    if (editedCourseMenuModel.UploadImage != null &&
                        editedCourseMenuModel.UploadImage.ContentLength > 0)
                    {
                        string oldPicfile = String.Empty;
                        if (menu.image != null)
                        {
                            oldPicfile= Path.Combine(Server.MapPath("~/Content/UploadedImages"), menu.image);
                        }
                      


                        var extension = Path.GetExtension(editedCourseMenuModel.UploadImage.FileName);
                        var namefile = Guid.NewGuid().ToString();
                        filename = namefile + extension;

                        var path = Path.Combine(Server.MapPath("~/Content/UploadedImages"), filename);

                        editedCourseMenuModel.UploadImage.SaveAs(path);

                        if (System.IO.File.Exists(oldPicfile))
                        {
                            System.IO.File.Delete(oldPicfile);

                        }


                    }



                    if (modifyMenu != null)
                    {
                        modifyMenu.menuid = editedCourseMenuModel.menu_Id;
                        modifyMenu.courseId = Convert.ToInt32(editedCourseMenuModel.courseId);
                        modifyMenu.menu_name = editedCourseMenuModel.menudesc;
                        modifyMenu.deptId = Convert.ToInt32(editedCourseMenuModel.deptId);
                        modifyMenu.price = editedCourseMenuModel.price??0;
                        modifyMenu.note = editedCourseMenuModel.Note;
                        modifyMenu.image = filename;
                        //modifyMenu.date_added = editedCourseMenuModel.dateAdded;
                        modifyMenu.date_update=DateTime.Today;
                    }
                    
                    _dbEntities.SaveChanges();

                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    throw new HttpException(404, "Page Not Found");
                }

             
               
            }
            else
            {
                editedCourseMenuModel.menu_Id = Utilities.MenusCode_Generator();
                editedCourseMenuModel.coursecategory_list = _coursemenuViewModel.Get_CourseListItems();
                editedCourseMenuModel.deptincharge_list = dept.Get_MenuDepartmentInchargeListItems();

                return View(editedCourseMenuModel);

            }

        }

        [HttpGet]
        public ActionResult Get_Image(string menuid)
        {
            var image= "no-photo.png";

            var success = false;

            var imagefile = _dbEntities.Menus.FirstOrDefault(x => x.menuid == menuid);

            if (imagefile != null)
            {
              
                if (imagefile.image!=null) { image = imagefile.image; }
               
                
                success = true;
            }

            //var path = Server.MapPath("~/Content/UploadedImages/" + image);

            return Json(new {success=success, upimage = image }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
           _dbEntities.Dispose();
        }
    }
}