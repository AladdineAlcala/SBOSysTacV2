using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class PackageAreaLocationViewModel
    {
        private PegasusEntities _dbEntities = new PegasusEntities();
        public int? p_areaNo { get; set; }
       
        public int p_id { get; set; }
        public string areadeatails { get; set; }
        [Display(Name = "Area")]
        [Required(ErrorMessage = "Area Required")]
        public int aID { get; set; }
        public int nopaxId { get; set; }
        public bool is_extended { get; set; }
        [Display(Name = "Extended Amount per Area")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal ext_amount { get; set; }

        public IEnumerable<SelectListItem> AreasSelectList { get; set; }
        public IEnumerable<SelectListItem> PackageTypeSelectList { get; set; }
        public IEnumerable<SelectListItem> ApplicablePaxSelectList { get; set; }


        public IEnumerable<SelectListItem> AreaByPackageDistinct { get; set; }

        public IEnumerable<SelectListItem> GetArea_SelectListItems()
        {
            var listofAreas = _dbEntities.Areas.AsEnumerable().Select(x => new SelectListItem
            {
                Value=x.aID.ToString(),
                Text = x.AreaDetails
            }).ToList().OrderBy(x=>x.Text);

            return new SelectList(listofAreas,"Value","Text");
        }

        public IEnumerable<SelectListItem> GetApplicablePax_SelectListItems()
        {
            var listappPax = _dbEntities.Packages_No_Pax_applicable.AsEnumerable().Select(x => new SelectListItem
            {
                Value = x.nopax_id.ToString(),
                Text = x.package_n_pax
            });
            return new SelectList(listappPax, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetPackageByAreaDistinct()
        {

            var distinctPackageAreas = (from p in _dbEntities.PackageAreaCoverages
                join a in _dbEntities.Areas on p.aID equals a.aID
                select new SelectListItem
                {
                    Value = p.aID.ToString(),
                    Text = a.AreaDetails
                }).Distinct();

            return new SelectList(distinctPackageAreas, "Value", "Text");
        }


       
        public List<Select2AreaViewModel> GetSelect2AreaViewModels()
        {
            var itemList = new List<Select2AreaViewModel>();

            var areas = _dbEntities.Areas.ToList();

            itemList = (from area in areas
                select new Select2AreaViewModel
                {
                    text = area.AreaDetails,
                    id = area.aID.ToString()
                }).ToList();

            return itemList;
        }
        
    }
}