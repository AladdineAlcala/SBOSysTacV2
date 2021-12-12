using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Neodynamic.SDK.Web;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.ViewModel;

namespace SBOSysTacV2.Reports.ReportViewers
{
    public partial class ReportViewerContractPrint : System.Web.UI.Page
    {
        private PrintContractDetails condetails = new PrintContractDetails();
        private BookMenusViewModel bm = new BookMenusViewModel();
        private AddonsViewModel add = new AddonsViewModel();


        //protected void Page_Init(object sender, EventArgs e)
        //{

        //    //if (WebClientPrint.ProcessPrintJob(Request))
        //    //{

        //        try
        //        {
        //            var paramTransId = Request["transactionId"].Trim();


        //            List<PrintContractDetails> conDetails = new List<PrintContractDetails>();
        //            List<BookMenusViewModel> conBookMenus = new List<BookMenusViewModel>();
        //            List<AddonsViewModel> addons = new List<AddonsViewModel>();

        //            //var paramPrint_Option = Request["reportOption"].Trim();

        //            var cryRep = new ReportDocument();
        //            TableLogOnInfos tbloginfos = new TableLogOnInfos();
        //            ConnectionInfo crConinfo = new ConnectionInfo();

        //            string reportName = "ReportContract2Details";

        //            string report = Utilities.ReportPath(reportName);

        //            cryRep.Load(report);

        //            SqlConnectionStringBuilder cnstrbuilding = new SqlConnectionStringBuilder(Utilities.DBGateway());


        //            crConinfo.ServerName = cnstrbuilding.DataSource;
        //            crConinfo.DatabaseName = cnstrbuilding.InitialCatalog;
        //            crConinfo.UserID = cnstrbuilding.UserID;
        //            crConinfo.Password = cnstrbuilding.Password;

        //            var reportSections = cryRep.ReportDefinition.Sections;


        //            foreach (Section section in reportSections)
        //            {
        //                var crReportObjects = section.ReportObjects;

        //                foreach (ReportObject crreportObject in crReportObjects)
        //                {

        //                    if (crreportObject.Kind != ReportObjectKind.SubreportObject)
        //                        continue;

        //                    var crSubreportObject = (SubreportObject)crreportObject;
        //                    var crsubReportDocument = crSubreportObject.OpenSubreport(crSubreportObject.SubreportName);
        //                    var crDatabase = crsubReportDocument.Database;
        //                    var crTables = crDatabase.Tables;

        //                    //var tbloginfos = new TableLogOnInfos();


        //                    foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
        //                    {

        //                        var crTableLogOnInfo = crTable.LogOnInfo;
        //                        crTableLogOnInfo.ConnectionInfo = crConinfo;
        //                        crTableLogOnInfo.ConnectionInfo.IntegratedSecurity = true;
        //                        crTable.ApplyLogOnInfo(crTableLogOnInfo);

        //                    }

        //                }
        //            }



        //            var cryTables = cryRep.Database.Tables;

        //            foreach (CrystalDecisions.CrystalReports.Engine.Table cryTable in cryTables)
        //            {
        //                var tbloginfo = cryTable.LogOnInfo;
        //                tbloginfo.ConnectionInfo = crConinfo;
        //                tbloginfo.ConnectionInfo.IntegratedSecurity = true;
        //                cryTable.ApplyLogOnInfo(tbloginfo);
        //            }



        //            CRViewerContract.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

        //            // ReportContract repcontract = new ReportContract();

        //            conDetails = (from c in condetails.GetContractDetails() select c).ToList();

        //            conDetails = conDetails.Where(x => x.transId == Convert.ToInt32(paramTransId)).ToList();
        //            //where c.transId == Convert.ToInt32(paramTransId)
        //            //select c).ToList();


        //            conBookMenus = bm.LisofMenusBook().Where(x => x.transId == Convert.ToInt32(paramTransId)).ToList();

        //            addons = add.ListofAddons().Where(x => x.TransId == Convert.ToInt32(paramTransId)).ToList();



        //            //repcontract.SetDataSource(conDetails);



        //            cryRep.Database.Tables[0].SetDataSource(conDetails);
        //            cryRep.Database.Tables[1].SetDataSource(conBookMenus);
        //            cryRep.Database.Tables[2].SetDataSource(addons);


        //            //Export rpt to a temp PDF and get binary content
        //            byte[] pdfContent = null;
        //            using (MemoryStream ms = (MemoryStream)cryRep.ExportToStream(ExportFormatType.PortableDocFormat))
        //            {
        //                pdfContent = ms.ToArray();
        //            }


        //            //get selected printer           
        //            string printerName = Server.UrlDecode(Request["printerName"]);

        //            //create a temp file name for our PDF report...
        //            string fileName = Guid.NewGuid().ToString("N") + ".pdf";

        //            //Create a PrintFile object with the pdf report
        //            PrintFile file = new PrintFile(pdfContent, fileName);


        //            //Create a ClientPrintJob and send it back to the client!
        //            ClientPrintJob cpj = new ClientPrintJob();

        //            //set file to print...
        //            cpj.PrintFile = file;
        //            //set client printer...
        //            if (printerName == "Default Printer")
        //                cpj.ClientPrinter = new DefaultPrinter();
        //            else
        //                cpj.ClientPrinter = new InstalledPrinter(System.Web.HttpUtility.UrlDecode(printerName));

        //            //send it...
        //           cpj.SendToClient(HttpContext.Current.Response);

                                  
                   
        //            //CRViewerContract.ReportSource = cryRep;
        //            //CRViewerContract.RefreshReport();

        //            ////cryRep.PrintToPrinter(1,false,0,0);


        //        }
        //        catch (Exception exception)
        //        {
        //            Console.WriteLine(exception);
        //            throw;
        //        }


        //    //}
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                try
                {
                    var paramTransId = Request["transactionId"].Trim();


                    List<PrintContractDetails> conDetails = new List<PrintContractDetails>();
                    List<BookMenusViewModel> conBookMenus = new List<BookMenusViewModel>();
                    List<AddonsViewModel> addons = new List<AddonsViewModel>();

                    //var paramPrint_Option = Request["reportOption"].Trim();

                    var cryRep = new ReportDocument();
                    TableLogOnInfos tbloginfos = new TableLogOnInfos();
                    ConnectionInfo crConinfo = new ConnectionInfo();

                    string reportName = "ReportContract2Details";

                    string report = Utilities.ReportPath(reportName);

                    cryRep.Load(report);

                    SqlConnectionStringBuilder cnstrbuilding = new SqlConnectionStringBuilder(Utilities.DBGateway());


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


                            foreach (CrystalDecisions.CrystalReports.Engine.Table crTable in crTables)
                            {

                                var crTableLogOnInfo = crTable.LogOnInfo;
                                crTableLogOnInfo.ConnectionInfo = crConinfo;
                                crTableLogOnInfo.ConnectionInfo.IntegratedSecurity = true;
                                crTable.ApplyLogOnInfo(crTableLogOnInfo);

                            }

                        }
                    }



                    var cryTables = cryRep.Database.Tables;

                    foreach (CrystalDecisions.CrystalReports.Engine.Table cryTable in cryTables)
                    {
                        var tbloginfo = cryTable.LogOnInfo;
                        tbloginfo.ConnectionInfo = crConinfo;
                        tbloginfo.ConnectionInfo.IntegratedSecurity = true;
                        cryTable.ApplyLogOnInfo(tbloginfo);
                    }


                 
                    CRViewerContract.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

                   // ReportContract repcontract = new ReportContract();

                    conDetails = (from c in condetails.GetContractDetails() select c).ToList();

                    conDetails = conDetails.Where(x => x.transId == Convert.ToInt32(paramTransId)).ToList();
                    //where c.transId == Convert.ToInt32(paramTransId)
                    //select c).ToList();


                    conBookMenus = bm.ListofMenusBook(Convert.ToInt32(paramTransId)).ToList();

                    addons = add.ListofAddons().Where(x => x.TransId == Convert.ToInt32(paramTransId)).ToList();



                    //repcontract.SetDataSource(conDetails);



                    cryRep.Database.Tables[0].SetDataSource(conDetails);
                    cryRep.Database.Tables[1].SetDataSource(conBookMenus);
                    cryRep.Database.Tables[2].SetDataSource(addons);


                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();

                    try
                    {
                        cryRep.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response,true,"ContractReciept");
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