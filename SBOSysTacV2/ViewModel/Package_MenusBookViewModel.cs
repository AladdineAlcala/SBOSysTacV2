using SBOSysTacV2.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SBOSysTacV2.ViewModel
{
    public class Package_MenusBookViewModel
    {
        public int trn_Id { get; set; }
        public int courseId { get; set; }
        public string menuid { get; set; }

        public List<Package_MenusBookViewModel> Get_Menu_on_PackageByTransId(int transId)
        {
            List <Package_MenusBookViewModel> listmenus= new List<Package_MenusBookViewModel>();

            var dbEntities = new PegasusEntities();

            listmenus = dbEntities.Database.SqlQuery<Package_MenusBookViewModel>("exec GetPackageBookMenus @trId ", new SqlParameter("@trId", transId)).ToList();

            dbEntities.Dispose();


            return listmenus;
        }

    }


}