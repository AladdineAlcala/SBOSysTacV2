using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.Models;
using SBOSysTacV2.ViewModel;

namespace SBOSysTacV2.Controllers
{

    [Authorize]
    public class AuditLogController : Controller
    {

        private PegasusEntities dbEntities;
        private AuditLogViewModel au=new AuditLogViewModel();
        public AuditLogController()
        {
            dbEntities=new PegasusEntities();
        }

        // GET: AuditLog
        public ActionResult Index()
        {
            return View();
        }

        [UserPermissionAuthorized(UserPermessionLevelEnum.superadmin)]
        [HttpGet]
        public ActionResult LoadAuditLogData()
        {
            var auditlogList = au.AuditLogs().ToList();

            return Json(new {data=auditlogList },JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            dbEntities.Dispose();
        }
    }
}