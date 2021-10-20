using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class PrintContractDetailsMenuAddonsViewModel
    {
        public int transId  { get; set; }
        public string menu { get; set; }
        public string addOns { get; set; }
       

        public IEnumerable<PrintContractDetailsMenuAddonsViewModel> GetAllMenuAddons(int transId)
        {
            PegasusEntities dbEntities=new PegasusEntities();

            List<PrintContractDetailsMenuAddonsViewModel> list=new List<PrintContractDetailsMenuAddonsViewModel>();

            var listdummy = (from b in dbEntities.Bookings
                join m in dbEntities.Book_Menus on b.trn_Id equals m.trn_Id
                join ad in dbEntities.BookingAddons on b.trn_Id equals ad.trn_Id
                join ma in dbEntities.Menus on m.menuid equals ma.menuid
                where b.trn_Id==transId
                select new
                {
                    tId=b.trn_Id,
                    menu=ma.menu_name,
                    addOns=ad.Addondesc

                }).ToList();


            foreach (var item in listdummy)
            {
                
                list.Add(new PrintContractDetailsMenuAddonsViewModel
                {
                   transId =item.tId,
                   menu = item.menu,
                   addOns = item.addOns
                });
            }


            return list;
        }
    }
}