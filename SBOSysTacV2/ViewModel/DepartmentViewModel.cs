using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class DepartmentViewModel
    {
        private PegasusEntities _dbEntities=new PegasusEntities();

        public IEnumerable<SelectListItem> Get_MenuDepartmentInchargeListItems()
        {

            var listofdeptIncharge = _dbEntities.Departments.AsEnumerable().Select(x => new SelectListItem
            {
                Value = x.deptId.ToString(),
                Text = x.deptName
            }).ToList();

            return new SelectList(listofdeptIncharge, "Value", "Text");

        }
    }
}