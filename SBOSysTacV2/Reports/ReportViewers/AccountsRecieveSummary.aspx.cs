using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.Shared;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.ViewModel;

namespace SBOSysTacV2.Reports.ReportViewers
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

                    var accnsummary = accnsum.GetAllAccnRecievables().ToList();  //2.45 min


                    cryRep.Database.Tables[0].SetDataSource(accnsummary);


                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    try
                    {
                        cryRep.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "CollectionReport");
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        throw;
                    }

                    //CRViewerAccnRecievableSummary.ReportSource = cryRep;
                    //CRViewerAccnRecievableSummary.RefreshReport();

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