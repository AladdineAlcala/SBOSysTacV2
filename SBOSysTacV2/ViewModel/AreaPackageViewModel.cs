using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;
using Area = CrystalDecisions.CrystalReports.Engine.Area;

namespace SBOSysTacV2.ViewModel
{
    public class AreaPackageViewModel
    {

        public int? areaId { get; set; }
        public int? packageId { get; set; }
        public string packagedetails { get; set; }
        public decimal? amountperPax { get; set; }
        public string pType { get; set; }
        public int noPaxId { get; set; }
        public bool isActive { get; set; }
        public bool? is_extended { get; set; }

        private PegasusEntities _dbcontext=new PegasusEntities();
       

        public IEnumerable<AreaPackageViewModel> GetAreasByPackages()
        {
            var areapackage = new List<AreaPackageViewModel>();

            areapackage = (from a in _dbcontext.PackageAreaCoverages
                join p in _dbcontext.Packages on a.p_id equals p.p_id where p.p_type.Trim().Equals("regular") && p.isActive==true
                           select new AreaPackageViewModel
                {
                    areaId = a.aID,
                    packageId = p.p_id,
                    packagedetails = p.p_descripton,
                    amountperPax = p.p_amountPax,
                    isActive = p.isActive,
                    is_extended=a.is_extended

                }).ToList();


            return areapackage;

        }

        public IEnumerable<AreaPackageViewModel> GetPackageByType()
        {

            var package = (from p in _dbcontext.Packages select p).ToList();

            return (from pa in package where pa.isActive==true
                select new AreaPackageViewModel
                {

                    packageId = pa.p_id,
                    packagedetails = pa.p_descripton,
                    amountperPax = pa.p_amountPax,
                    isActive = pa.isActive,
                    pType = pa.p_type.Trim(),
                    noPaxId =(int) pa.nopax_id,
                    is_extended =isextended(pa.p_id)

                }).ToList();



        }

        public bool isextended(int packageid)
        {
            bool hasext = false;
            var areapackage = _dbcontext.PackageAreaCoverages.FirstOrDefault(x => x.p_id == packageid);
            if (areapackage != null)
            {
                hasext = (bool) areapackage.is_extended;
            }
            return hasext;
        }

        
    }
}