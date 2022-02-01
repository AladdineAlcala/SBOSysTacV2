using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class AreaDetailsViewModel
    {
        public int? areaId { get; set; }
        [Display(Name = "Package Area:")]
        [Required(ErrorMessage = "Package Area Required")]
        public string areaDetails { get; set; }

        private PegasusEntities dbEntities = new PegasusEntities();

        public IEnumerable<AreaDetailsViewModel> GetAreaDetailsList()
        {
            List<AreaDetailsViewModel> areadetails=new List<AreaDetailsViewModel>();

            try
            {

                

                areadetails = (from a in dbEntities.Areas
                    select new AreaDetailsViewModel()
                    {
                        areaId = a.aID,
                        areaDetails = a.AreaDetails
                    }).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return areadetails.OrderBy(x => x.areaId);
        }

    }
}