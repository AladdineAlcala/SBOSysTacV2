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
        private PrintContractDetails condetails;
        private BookMenusViewModel bmv;
        private PackageBookingViewModel package_book_vm;
        private BookMenusViewModel book_menus_vm;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                condetails = new PrintContractDetails();
                bmv = new BookMenusViewModel();
                package_book_vm = new PackageBookingViewModel();
                book_menus_vm = new BookMenusViewModel();



                try
                {
                    var paramTransId = Request["transactionId"].Trim();


                    PrintContractDetails conDetails =new PrintContractDetails();
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

                    conDetails = condetails.GetContractDetailsById(Convert.ToInt32(paramTransId));

                   //Convert ViewModel List to DataTable
                   DataTable dtBookingDetailsTable = conDetails.ToDataTable();

                   var packageBooking = package_book_vm.GetBookingDetailById(Convert.ToInt32(paramTransId));

                   conBookMenus = book_menus_vm.ListOfMenusBook(packageBooking).ToList();


                    //conBookMenus = bmv.ListOfMenusBook(Convert.ToInt32(paramTransId)).Where(t=>t.menu_No!=null).ToList();

                    DataTable dtBookMenus = conBookMenus.ToDataTableList();

                    var transdetails = tdvm.GetTransactionStatementAccountById(conDetails);
                    var addonsmiscListRep = TransactionDetailsViewModel.GetListReport(Convert.ToInt32(paramTransId));

                    DataTable dtaddmiscLlist = addonsmiscListRep.ToDataTableList();

                    //DataTable dtTransDetails = transdetails.ToDataTable();

                    //repcontract.SetDataSource(conDetails);

                    var netPackageAmount =
                        (transdetails.PackageAmount + transdetails.TotaAddons + transdetails.TotaMiscCharge + transdetails.extLocAmount) -
                        (transdetails.cateringdiscount);

                    var discountAmount = transdetails.cateringdiscount;

                    cryRep.Database.Tables[0].SetDataSource(dtBookingDetailsTable);
                    cryRep.Database.Tables[1].SetDataSource(dtBookMenus);
                    cryRep.Database.Tables[2].SetDataSource(dtaddmiscLlist);
                    //cryRep.Database.Tables[3].SetDataSource(dtTransDetails);

                    cryRep.SetParameterValue("PackageTotaAmount", netPackageAmount, "SOA");
                    cryRep.SetParameterValue("Discount", discountAmount, "SOA");

                    //cryRep.Subreports[1].Database.Tables[0].SetDataSource();



                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();


                    // uncomment to display as pdf
                    try
                    {
                        cryRep.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "ContractDetails");

                        //cryRep.ExportToDisk(ExportFormatType.PortableDocFormat, HostingEnvironment.MapPath(string.Format("~/Reports/{0}.pdf", reportName)));

                        //ClientScript.RegisterStartupScript(this.Page.GetType(),Guid.NewGuid().ToString(), "var popup=window.open('/Reports/"+ reportName +".pdf');popup.focus();",true);

                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        throw;
                    }



                    // uncomment to display in report viewer
                    //CRViewerContract.ReportSource = cryRep;
                    //CRViewerContract.RefreshReport();


                    //uncomment to directly print to printer
                    //cryRep.PrintToPrinter(1,false,0,0);
                    Response.End();
                    cryRep.Dispose();

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