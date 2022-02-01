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
using SBOSysTacV2.ViewModel;

namespace SBOSysTacV2.Reports.ReportViewers
{
    public partial class AccnRecvPayment : System.Web.UI.Page
    {

        private PrintContractDetails condetails = new PrintContractDetails();
        private PrintRcvPaymentDetails pmtRcvDetails=new PrintRcvPaymentDetails();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    var paramTransId = Request["transactionId"].Trim();


                    List<PrintContractDetails> conDetails = new List<PrintContractDetails>();
                    List<PrintRcvPaymentDetails> prntDetails=new List<PrintRcvPaymentDetails>();



                    ReportDocument cryRep = new ReportDocument();
                    TableLogOnInfos tbloginfos = new TableLogOnInfos();
                    ConnectionInfo crConinfo = new ConnectionInfo();

                    string reportName = "ReportAccnRecievableDetails";

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



                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    CRViewerAccnRecvPayment.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;


                    conDetails = (from c in condetails.GetContractDetails() select c).ToList();

                    conDetails = conDetails.Where(x => x.transId == Convert.ToInt32(paramTransId)).ToList();

                    prntDetails = pmtRcvDetails.GetPaymentsList().ToList();

                    cryRep.Database.Tables[0].SetDataSource(conDetails);
                    cryRep.Database.Tables[1].SetDataSource(prntDetails);

                    try
                    {
                        cryRep.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true,
                            "AccnRcvPayment");
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
            }
        }
    }
}