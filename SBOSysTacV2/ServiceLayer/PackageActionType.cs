using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SBOSysTacV2.ServiceLayer
{
    public class PackageActionType
    {
        public static string pactype { get; set; }
        public static void Getpackagecontroller(string controller)
        {
            pactype = controller;

        }
    }


    public enum bookAction
    {
        GetBookMenusPartial,
        GetSnacksPartial
    }

}   