using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class PrintOptionViewModel
    {
        public int Id { get; set; }
        public string selPrintOpt { get; set; }
        public DateTime dateFrom { get; set; }
        public DateTime dateTo { get; set; }
        public Customer Customer { get; set; }
        public bool ex_unsetevent { get; set; }
        public IEnumerable<TransRecievablesViewModel> AccountRecievables { get; set; }
    }

    public enum printoption
    {
        contform,
        confunction
    }
}