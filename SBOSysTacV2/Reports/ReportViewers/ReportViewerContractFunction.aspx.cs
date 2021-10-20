using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.Reports;
using SBOSysTacV2.Models;
using SBOSysTacV2.ViewModel;
using System.Data.SqlClient;

namespace SBOSysTacV2.Reports.ReportViewers
{
    public partial class ReportViewerContractFunction : System.Web.UI.Page
    {
        private PrintContractDetails condetails = new PrintContractDetails();
        private BookMenusViewModel bm = new BookMenusViewModel();
        private AddonsViewModel add = new AddonsViewModel();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                try
                {
                    var paramTransId = Request["transactionId"].Trim();

                    //var paramPrint_Option = Request["reportOption"].Trim();

                    ReportDocument cryRep = new ReportDocument();
                    TableLogOnInfos tbloginfos = new TableLogOnInfos();
                    ConnectionInfo crConinfo = new ConnectionInfo();

                    string reportName = "ReportContractFunction";

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


                    List<PrintContractDetails> conDetails = new List<PrintContractDetails>();
                    List<BookMenusViewModel> conBookMenus = new List<BookMenusViewModel>();
                    List<AddonsViewModel> addons = new List<AddonsViewModel>();
                    CRViewerContractFunction.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

                   // ReportContractFunction repcontractfunction = new ReportContractFunction();

                    conDetails = (from c in condetails.GetContractDetails()
                        where c.transId == Convert.ToInt32(paramTransId)
                        select c).ToList();


                    conBookMenus = bm.LisofMenusBook(Convert.ToInt32(paramTransId)).ToList();

                    addons = add.ListofAddons().Where(x => x.TransId == Convert.ToInt32(paramTransId)).ToList();



                    //repcontract.SetDataSource(conDetails);

                    cryRep.Database.Tables[0].SetDataSource(conDetails);
                    cryRep.Database.Tables[1].SetDataSource(conBookMenus);
                    cryRep.Database.Tables[2].SetDataSource(addons);

                    //cryRep.Database.Tables["SBOSysTacV2_ViewModel_BookMenusViewModel"].SetDataSource(conDetails);
                    //cryRep.Database.Tables["SBOSysTacV2_ViewModel_BookMenusViewModel"].SetDataSource(conBookMenus);
                    //cryRep.Database.Tables["SBOSysTacV2_Models_BookingAddon"].SetDataSource(addons);


                    CRViewerContractFunction.ReportSource = cryRep;
                    CRViewerContractFunction.RefreshReport();

                  //  ScriptManager.RegisterStartupScript(this,this.GetType(),"script", "removeloader();",true);

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