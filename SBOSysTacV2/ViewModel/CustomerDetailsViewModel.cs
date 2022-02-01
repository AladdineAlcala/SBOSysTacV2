using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SBOSysTacV2.ViewModel
{
    public class CustomerDetailsViewModel
    {
        public int? c_Id { get; set; }
        [Display(Name = "LASTNAME:")]
        [Required(ErrorMessage = "Lastname Required")]
        public string lastname { get; set; }
        [Display(Name = "FIRSTNAME:")]
        [Required(ErrorMessage = "Firstname Required")]
        public string firstname { get; set; }
        [Display(Name = "MI:")]
        public string middle { get; set; }
        [Display(Name = "ADDRESS:")]
        [Required(ErrorMessage = "Address Required")]
        public string address { get; set; }
        [Display(Name = "CONTACT(Cel No.)")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(09|\+639)\d{9}$", ErrorMessage = "Entered phone format is not valid.")]
        public string contact1 { get; set; }
        [Display(Name = "CONTACT(Tel)")]
        //[DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9]\d{2}-\d{3}-\d{4}$", ErrorMessage = "Not a valid phone number")]
        public string contact2 { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> datereg { get; set; }
        [Display(Name = "COMPANY:")]
        public string company { get; set; }
    }
}