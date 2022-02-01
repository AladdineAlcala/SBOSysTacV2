using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SBOSysTacV2.ViewModel
{
    public class UserProfileViewModel
    {
        public string uId { get; set; }
        public string username { get; set; }
        public string fullname { get; set; }
        public string emailAdd { get; set; }
        public string roles { get; set; }
        public UserUpdatePassViewModel UserUpdatePassViewModel { get; set; }


        public bool CheckPasswordValidity(string userPass)
        {
          
       

            return true;
        }

    }
}