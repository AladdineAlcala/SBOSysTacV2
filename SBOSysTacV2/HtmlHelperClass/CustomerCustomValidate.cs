using System.ComponentModel.DataAnnotations;
using System.Linq;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.HtmlHelperClass
{
    public class CustomerCustomValidate :ValidationAttribute
    {
        private PegasusEntities dbEntities=new PegasusEntities();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value != null)
            {

                var bookviewmodel = (ViewModel.BookingsViewModel) validationContext.ObjectInstance;

                var cusId = bookviewmodel.c_Id;

                if (cusId == null)
                {

                    return new ValidationResult("Customer is not Registered");
                }
                else
                {


                    var is_customerinthelist = dbEntities.Customers.Any(x => x.c_Id == cusId);

                    if (is_customerinthelist==false)
                    {
                        return new ValidationResult("Customer is not Registered");
                    }
                    else
                    {
                        return ValidationResult.Success;
                    }
                }

            }
            else
            {
                return new ValidationResult("Customer is not Registered");
            }
        }
    }
}