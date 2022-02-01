using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using SBOSys.HtmlHelperClass;
using SBOSys.Reports;
using SBOSys.ViewModel;


namespace SBOSys
{
    public partial class ReportViewer : System.Web.UI.Page
    {

        private PrintContractDetails condetails=new PrintContractDetails();
        private BookMenusViewModel bm=new BookMenusViewModel();
        private AddonsViewModel add=new AddonsViewModel();
        protected void Page_Init(object sender, EventArgs e)
        {
            


        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    var paramTransId = Request["transactionId"].Trim();

                    //var paramPrint_Option = Request["reportOption"].Trim();

                    ReportDocument cryRep=new ReportDocument();
                    TableLogOnInfos tbloginfos = new TableLogOnInfos();
                    ConnectionInfo crConinfo = new ConnectionInfo();

                   const string reportName = "ReportContract";

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
                    List<AddonsViewModel> addons=new List<AddonsViewModel>();
                    CRViewerContract.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

                    ReportContract repcontract = new ReportContract();

                    conDetails = (from c in condetails.GetContractDetails()
                        where c.transId == Convert.ToInt32(paramTransId)
                        select c).ToList();


                    conBookMenus = bm.LisofMenusBook().Where(x => x.transId == Convert.ToInt32(paramTransId)).ToList();

                    addons = add.ListofAddons().Where(x => x.TransId == Convert.ToInt32(paramTransId)).ToList();



                    //repcontract.SetDataSource(conDetails);

                    repcontract.Database.Tables[0].SetDataSource(conDetails);
                    repcontract.Database.Tables[1].SetDataSource(conBookMenus);
                    repcontract.Database.Tables[2].SetDataSource(addons);

                    CRViewerContract.ReportSource = repcontract;
                    CRViewerContract.RefreshReport();


                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
           

        }

   
    }
}