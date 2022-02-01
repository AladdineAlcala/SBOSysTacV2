﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SBOSysTacV2.ViewModel
{
    public class UserUpdatePassViewModel
    {
        public string userId { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string oldPass { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string newPass { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string confirmnewPass { get; set; }

    }
}