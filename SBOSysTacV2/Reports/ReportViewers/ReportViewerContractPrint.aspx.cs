using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.ViewModel;

namespace SBOSysTacV2.Reports.ReportViewers
{
    public partial class ReportViewerContractPrint : Page
    {
        private PrintContractDetails condetails = new PrintContractDetails();
        private BookMenusViewModel bmv = new BookMenusViewModel();




        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                try
                {
                    var paramTransId = Request["transactionId"].Trim();


                    List<PrintContractDetails> conDetails = new List<PrintContractDetails>();
                    List<BookMenusViewModel> conBookMenus = new List<BookMenusViewModel>();
                    TransactionDetailsViewModel tdvm = new TransactionDetailsViewModel();


                    //var paramPrint_Option = Request["reportOption"].Trim();

                    var cryRep = new ReportDocument();
                    //TableLogOnInfos tbloginfos = new TableLogOnInfos();
                    

                    string reportName = "ReportContract2Details";

                    string report = Utilities.ReportPath(reportName);

                    cryRep.Load(report);

                    SqlConnectionStringBuilder cnstrbuilding = new SqlConnectionStringBuilder(Utilities.DBGateway());

                    ConnectionInfo crConinfo = new ConnectionInfo();

                    crConinfo.ServerName = cnstrbuilding.DataSource;
                    crConinfo.DatabaseName = cnstrbuilding.InitialCatalog;
                    crConinfo.UserID = cnstrbuilding.UserID;
                    crConinfo.Password = cnstrbuilding.Password;


                    var reportSections = cryRep.ReportDefinition.Sections;

                    foreach (Section section in reportSections)
                    {
                        var crReportObjects = section.ReportObjects;

                        foreach (ReportObject crreportObject in crReportObjects)
                        {

                            if (crreportObject.Kind != ReportObjectKind.SubreportObject)
                                continue;

                            var crSubreportObject = (SubreportObject)crreportObject;
                            var crsubReportDocument = crSubreportObject.OpenSubreport(crSubreportObject.SubreportName);

                            var crDatabase = crsubReportDocument.Database;
                            var crTables = crDatabase.Tables;

                            //var tbloginfos = new TableLogOnInfos();


                            foreach (Table crTable in crTables)
                            {

                                var crTableLogOnInfo = crTable.LogOnInfo;
                                crTableLogOnInfo.ConnectionInfo = crConinfo;
                                crTableLogOnInfo.ConnectionInfo.IntegratedSecurity = true;
                                crTable.ApplyLogOnInfo(crTableLogOnInfo);

                            }

                        }
                    }



                    var cryTables = cryRep.Database.Tables;

                    foreach (Table cryTable in cryTables)
                    {
                        var tbloginfo = cryTable.LogOnInfo;
                        tbloginfo.ConnectionInfo = crConinfo;
                        tbloginfo.ConnectionInfo.IntegratedSecurity = true;
                        cryTable.ApplyLogOnInfo(tbloginfo);
                    }



                    CRViewerContract.ToolPanelView = ToolPanelViewType.None;

                   // ReportContract repcontract = new ReportContract();

                   conDetails = condetails.GetContractDetailsById(Convert.ToInt32(paramTransId)).ToList();

                   //Convert ViewModel List to DataTable
                   DataTable dtBookingDetailsTable = conDetails.ToDataTableList();


                    conBookMenus = bmv.ListofMenusBook(Convert.ToInt32(paramTransId)).Where(t=>t.menu_No!=null).ToList();
                    DataTable dtBookMenus = conBookMenus.ToDataTableList();

                    var transdetails = tdvm.GetTransactionStatementAccountById(Convert.ToInt32(paramTransId));

                    DataTable dtTransDetails = transdetails.ToDataTable();

                    //repcontract.SetDataSource(conDetails);



                    cryRep.Database.Tables[0].SetDataSource(dtBookingDetailsTable);
                    cryRep.Database.Tables[1].SetDataSource(dtBookMenus);
                    cryRep.Database.Tables[2].SetDataSource(dtTransDetails);


                    //cryRep.Subreports[1].Database.Tables[0].SetDataSource();


                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    try
                    {
                        cryRep.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, "ReportContract2Details");
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        throw;
                    }

                    //CRViewerContract.ReportSource = cryRep;
                    //CRViewerContract.RefreshReport();

                    //cryRep.PrintToPrinter(1,false,0,0);

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