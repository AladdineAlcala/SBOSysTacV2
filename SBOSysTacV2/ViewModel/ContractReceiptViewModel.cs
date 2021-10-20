using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class ContractReceiptViewModel
    {
        public int transId { get; set; }
        public DateTime dateofTrans { get; set; }
        public string fullname { get; set; }
        public string address { get; set; }
        public string occassion { get; set; }
        public string venue { get; set; }
        public string colorscheme { get; set; }
        public string typeofservice { get; set; }
        public string contact { get; set; }
        public DateTime dateofSched { get; set; }
        public int no_ofPax { get; set; }
        public decimal amountperPax { get; set; }
        public decimal dpAmount { get; set; }
        public decimal fpAmount { get; set; }
        //public IEnumerable<BookMenusViewModel> book_menus { get; set; }
        //public IEnumerable<AddonsViewModel> book_addons { get; set; }


        private PegasusEntities dbEntities=new PegasusEntities();

        public ContractReceiptViewModel getContractReciept(int _transId)
        {
            ContractReceiptViewModel contract_report=new ContractReceiptViewModel();

            IEnumerable<Booking> bookings = (from booking in dbEntities.Bookings select booking).ToList();


            contract_report = (from b in bookings where b.trn_Id==_transId
                select new ContractReceiptViewModel()
                {
                    transId = b.trn_Id,
                    fullname = Utilities.getfullname(b.Customer.lastname,b.Customer.firstname,b.Customer.middle),
                    dateofTrans =Convert.ToDateTime(b.transdate),
                    address = b.Customer.address,
                    occassion = b.occasion,
                    venue = b.venue


                }).FirstOrDefault();

            return contract_report;
        }
    }
}