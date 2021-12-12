using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.Models;

namespace SBOSysTacV2.ViewModel
{
    public class BookAddonSelectedViewModel
    {
        public BookAddonsDetail BookingAddonsDetail  { get; set; }
        public SelectedAddonsViewModel SelectedBookAddonsDetail { get; set; }
    }
}