using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBOSysTacV2.ViewModel
{
    public class AuditChangesLog
    {
        public string columnName { get; set; }
        public string OrigValue { get; set; }
        public string NewValue { get; set; }

    }
}