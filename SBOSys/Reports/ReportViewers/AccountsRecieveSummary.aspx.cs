﻿using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using SBOSys.HtmlHelperClass;
using SBOSys.ViewModel;

namespace SBOSys.Reports.ReportViewers
{
    public partial class AccountsRecieveSummary : System.Web.UI.Page
    {

        private AccnRecieveSummaryViewModel accnsum=new AccnRecieveSummaryViewModel();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                try
                {

                    ReportDocument cryRep = new ReportDocument();
                    TableLogOnInfos tbloginfos = new TableLogOnInfos();
                    ConnectionInfo crConinfo = new ConnectionInfo();

                    string reportName = "ReportAccnRecieveSummary";

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



                    CRViewerAccnRecievableSummary.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

                    List<AccnRecieveSummaryViewModel> accnsummary=new List<AccnRecieveSummaryViewModel>();

                    accnsummary = (from r in accnsum.GetAllAccnRecieve() select r).ToList();
                    cryRep.Database.Tables[0].SetDataSource(accnsummary);


                    CRViewerAccnRecievableSummary.ReportSource = cryRep;
                    CRViewerAccnRecievableSummary.RefreshReport();

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