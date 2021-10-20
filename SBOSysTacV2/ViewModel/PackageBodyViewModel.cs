using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class PackageBodyViewModel
    {
        public int pbodyKey { get; set; }
        public int pbodycourseid { get; set; }
        public int package_Id { get; set; }
        public string package_name { get; set; }
        public string pbodycoursename { get; set; }

        public IEnumerable<PackageBodyViewModel> GetAllMenusinPackage()
        {
            var context=new PegasusEntities();

            var listofmenusselected = (from pb in context.PackageBodies join cc in context.CourseCategories on pb.courseId equals cc.CourserId 
                select new PackageBodyViewModel()
                {
                    package_Id = (int) pb.p_id,
                    pbodyKey = pb.No,
                    pbodycourseid = (int) pb.courseId,
                    pbodycoursename = cc.Course
                }).ToList();

            return listofmenusselected;
        }

    }
}