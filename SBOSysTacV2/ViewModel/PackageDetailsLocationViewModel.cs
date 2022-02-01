using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class PackageDetailsLocationViewModel
    {
        public int? PackageId { get; set; }
        public Package Packages { get; set; }
        public string packageApplicablepax { get; set; }
        public IEnumerable<PackageBodyViewModel> PBody { get; set; }
        public IEnumerable<PackageAreaDetailsViewModel> PackageAreaDetails { get; set; }
        public IPagedList<PackageAreaCoverage> PackageAreaCoverages { get; set; }

        
        private PackageAreaDetailsViewModel pareadetails=new PackageAreaDetailsViewModel();

        public IEnumerable<PackageDetailsLocationViewModel> GetPackageDetailsView()
        {
           // var package = new List<PackageDetailsLocationViewModel>();

            var _dbcontext = new PegasusEntities();

           return (from p in _dbcontext.Packages
                           select new
                           {
                               _pId = p.p_id,
                               _package = p,
                               _pbody = (from x in p.PackageBodies
                                         join cc in _dbcontext.CourseCategories on x.courseId equals cc.courseId
                                         select new PackageBodyViewModel()
                                         {
                                             pbodyKey = x.No,
                                             pbodycourseid = (int)x.courseId,
                                             pbodycoursename = cc.Course

                                         }).ToList().Take(4).OrderBy(c => c.pbodyKey),

                               _packagearea = (from pa in p.PackageAreaCoverages
                                               join area in _dbcontext.Areas on pa.aID equals area.aID
                                               select new PackageAreaDetailsViewModel()
                                               {
                                                   pId = (int)pa.p_id,
                                                   pAreaId = area.aID,
                                                   pAreaDetail = area.AreaDetails,
                                                   isExtended = pa.is_extended,
                                                   extendeAmt = pa.ext_amount
                                               }).ToList()

                                    }).ToList().Select(p => new PackageDetailsLocationViewModel()
                                    {
                                        PackageId = p._pId,
                                        Packages = p._package,
                                        //packageApplicablepax = p._packageAppPax.ToString(),
                                        PBody = p._pbody,
                                        PackageAreaDetails = p._packagearea
                                    }).OrderByDescending(order => order.PackageId);

           
        }


        public IEnumerable<PackageBodyViewModel> DisplayPackageBody()
        {
            var dbcontext = new PegasusEntities();

            var packagebody = (from pb in dbcontext.PackageBodies
                               join cc in dbcontext.CourseCategories on pb.courseId equals cc.courseId
                               select new PackageBodyViewModel()
                               {
                                   pbodyKey = pb.No,
                                   package_Id = (int) pb.p_id,
                                   pbodycoursename = cc.Course,
                                   pbodycourseid = (int) pb.courseId

                               }).ToList();

            return packagebody;
        }
    }

    
}