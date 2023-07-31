using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SBOSysTacV2.HtmlHelperClass
{
    public enum PackageType
    {
        [Display(Name = "Regular")]
        regular,

        [Display(Name = "VIP")]
        vip,

        [Display(Name = "PackMeals")]
        pm,


        [Display(Name = "Snacks and Drinks")]
        sd,

        [Display(Name = "Wedding")]
        wedding,

        [Display(Name = "SpecialRate")]
        sprate,

        [Display(Name = "Premier")]
        premier,


        cat


    }


}