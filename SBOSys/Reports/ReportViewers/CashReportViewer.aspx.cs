﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using SBOSys.HtmlHelperClass;
using SBOSys.ViewModel;

namespace SBOSys.Reports.ReportViewers
{
    public partial class CashReportViewer : System.Web.UI.Page
    {
      
        private CollectionReportViewModel collRep=new CollectionReportViewModel();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                try
                {

                    var paramfilterdatefrom = Request["filterdatefrom"].Trim();

                    var paramfilterdateTo = Request["filterdateto"].Trim();

                    DateTime datefrom = Convert.ToDateTime(paramfilterdatefrom);
                    DateTime dateTo = Convert.ToDateTime(paramfilterdateTo);



                    ReportDocument cryRep = new ReportDocument();
                    TableLogOnInfos tbloginfos = new TableLogOnInfos();
                    ConnectionInfo crConinfo = new ConnectionInfo();

                    string reportName = "ReportCollection";

                    string report = Utilities.ReportPath(reportName);

                    cryRep.Load(report);

                    SqlConnectionStringBuilder cnstrbuilding = new SqlConnectionStringBuilder(Utilities.DBGateway());


                    crConinfo.ServerName = cnstrbuilding.DataSource;
                    crConinfo.DatabaseName = cnstrbuilding.InitialCatalog;
                    crConinfo.UserID = cnstrbuilding.UserID;
                    crConinfo.Password = cnstrbuilding.Password;

                    var cryTables = cryRep.Database.Tables;

                    foreach (CrystalDecisions.CrystalReports.Engine.Table cryTable in cryTables)
                    {
                        var tbloginfo = cryTable.LogOnInfo;
                        tbloginfo.ConnectionInfo = crConinfo;
                        tbloginfo.ConnectionInfo.IntegratedSecurity = true;
                        cryTable.ApplyLogOnInfo(tbloginfo);
                    }


                   
                    CRViewerCashReport.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

                    // ReportContractFunction repcontractfunction = new ReportContractFunction();

                    //filter data

                    var collectionreport = collRep.GetAllCollection()
                        .Where(p => p.payDate.Date >= datefrom.Date && p.payDate.Date <= dateTo.Date)
                        .OrderBy(x => x.transId).ToList();



                    //repcontract.SetDataSource(conDetails);

                    cryRep.Database.Tables[0].SetDataSource(collectionreport);
                    //cryRep.Database.Tables[1].SetDataSource(conBookMenus);
                    //cryRep.Database.Tables[2].SetDataSource(addons);




                    CRViewerCashReport.ReportSource = cryRep;
                    CRViewerCashReport.RefreshReport();


                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }
        }
    }
}