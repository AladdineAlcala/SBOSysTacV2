﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SBOSysTacV2.HtmlHelperClass
{
    public class AllowExtensionAttribute :ValidationAttribute
    {
        public string Extensions { get; set; } = "png,jpg,jpeg,gif";

        public override bool IsValid(object value)
        {
            HttpPostedFileBase file = value as HttpPostedFileBase;
            bool isValid = true;

            // Settings.  
            List<string> allowedExtensions = this.Extensions.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            // Verification.  
            if (file != null)
            {
                // Initialization.  
                var fileName = file.FileName;

                // Settings.  
                isValid = allowedExtensions.Any(y => fileName.EndsWith(y));
            }

            // Info  
            return isValid;
        }
    }
}