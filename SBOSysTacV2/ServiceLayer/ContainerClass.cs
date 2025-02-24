﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SBOSysTacV2.ViewModel;

namespace SBOSysTacV2.ServiceLayer
{
    public static class ContainerClass
    {
        //public static List<CateringReportViewModel> CateringReport=new List<CateringReportViewModel>();

        public static List<CateringReportViewModel> CateringList { get; set; }

        public static List<TransRecievablesViewModel> TransRecievablesAccn = new List<TransRecievablesViewModel>();

        public static List<AddonsReportViewModel> AddonsReport = new List<AddonsReportViewModel>();

        public static void CateringReport(List<CateringReportViewModel> _list)
        { 
            CateringList = _list;
        }

        public static void GetAddonsReport(List<AddonsReportViewModel> _addonsReport)
        {
            AddonsReport = _addonsReport;
        }
    }
}