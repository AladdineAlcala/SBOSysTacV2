using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.ServiceLayer;

namespace SBOSysTacV2.Reports.ReportViewers
{
    public partial class AdminAddonsReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    var paramfilterdatefrom = Request["filterdatefrom"].Trim();
                    var paramfilterdateTo = Request["filterdateto"].Trim();

                    ReportDocument cryRep = new ReportDocument();
                    TableLogOnInfos tbloginfos = new TableLogOnInfos();
                    ConnectionInfo crConinfo = new ConnectionInfo();

                    string reportName = "Admin_AddonsReport";

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

                    CRViewerAdminAddonsReport.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

                    cryRep.Database.Tables[0].SetDataSource(ContainerClass.AddonsReport.ToDataTableList());

                    cryRep.SetParameterValue("dateFrom", Convert.ToDateTime(paramfilterdatefrom).ToString("MM-dd-yyyy"));
                    cryRep.SetParameterValue("DateTo", Convert.ToDateTime(paramfilterdateTo).ToString("MM-dd-yyyy"));

                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    try
                    {
                        cryRep.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false,
                            "Admin_AddonsReport");
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        throw;
                    }

                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }


            };
        }
    }
}