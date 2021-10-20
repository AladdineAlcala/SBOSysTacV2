using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class AuditLogViewModel
    {
        public int? logId { get; set; }
        public DateTime? dateLog { get; set; }
        public string username { get; set; }
        public string audit_operation { get; set; }
        public string tablename { get; set; }
        public string audit_Data { get; set; }

        private PegasusEntities dbEntities=new PegasusEntities();

        public IEnumerable<AuditLogViewModel> AuditLogs()
        {
            List<AuditLogViewModel> auditList=new List<AuditLogViewModel>();

            var auditloglist = (from audt in dbEntities.AuditLogs select audt).ToList();


            auditList = (from a in auditloglist
                         select new AuditLogViewModel()
                            {
                                logId = a.AuditLogId,
                                dateLog = a.EventDateUTC,
                                username = a.UserName,
                                audit_operation = a.AuditOperation,
                                tablename = a.TableName,
                                audit_Data =JsonExtractorHelper.GetJson(a.AuditData)

                            }).OrderByDescending(x => x.logId).ToList();

            return auditList;
        }



    }
}