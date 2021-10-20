using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class DiscountBookingsViewModel
    {
        public int transId { get; set; }
        public int discountId { get; set; }
        public string discode { get; set; }
        public string disctype { get; set; }
        public decimal discount{ get; set; }
        public decimal discountedActualAmount { get; set; }

        private PegasusEntities _dbEntities=new PegasusEntities();

        public IEnumerable<DiscountBookingsViewModel> GetBookingDiscount()
        {
            List <DiscountBookingsViewModel> listofregamuont= new List<DiscountBookingsViewModel>();


            try
            {
                listofregamuont = (from b in _dbEntities.Book_Discount
                    join d in _dbEntities.Discounts on b.disc_Id equals d.disc_Id
                    select new DiscountBookingsViewModel()
                    {

                    }).ToList();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return listofregamuont.ToList();
        }
    }
}