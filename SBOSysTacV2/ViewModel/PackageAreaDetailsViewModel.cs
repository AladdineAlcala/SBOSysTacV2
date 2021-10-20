using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class PackageAreaDetailsViewModel
    {
        public int pId { get; set; }
        public int pAreaId { get; set; }
        public string pAreaDetail { get; set; }
        public bool? isExtended { get; set; }
        public decimal? extendeAmt { get; set; }


        public IEnumerable<PackageAreaDetailsViewModel> GetListofAreaDetailsPackage()
        {
            var listofareabyPackage=new List<PackageAreaDetailsViewModel>();

            using (var _dbcontext=new PegasusEntities())
            {
                listofareabyPackage = (from pa in _dbcontext.PackageAreaCoverages
                    join a in _dbcontext.Areas on pa.p_id equals a.aID
                    select new PackageAreaDetailsViewModel()
                    {
                        pId =Convert.ToInt32(pa.p_id),
                        pAreaId = a.aID,
                        pAreaDetail = a.AreaDetails,
                        isExtended = pa.is_extended,
                        extendeAmt = pa.ext_amount

                    }).ToList();


            }

            return listofareabyPackage;

        }
    }
}