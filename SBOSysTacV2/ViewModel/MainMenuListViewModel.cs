using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class MainMenuListViewModel
    {
        public string menuId { get; set; }
        public string menu_name { get; set; }
        public string course { get; set; }
        public bool? isMainMenu { get; set; }
       

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
                        course = c.Course,
                        isMainMenu = c.Main_Bol

                    }).OrderBy(x=>x.course).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return mainmenulist;
        }

    }
}