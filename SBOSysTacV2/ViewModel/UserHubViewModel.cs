using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBOSysTacV2.ViewModel
{
    public class UserHubViewModel
    {
        public string username { get; set; }
        public HashSet<string> connectionId { get; set; }

    }
}