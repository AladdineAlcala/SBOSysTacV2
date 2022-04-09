using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using SBOSysTacV2.HtmlHelperClass;
using SBOSysTacV2.Models;
using SBOSysTacV2.ServiceLayer;
using SBOSysTacV2.ViewModel;

namespace SBOSysTacV2.Reports.ReportViewers
{
    public partial class AccnRecvPayment : System.Web.UI.Page
    {

        private PrintContractDetails condetails = new PrintContractDetails();
        private PrintRcvPaymentDetails pmtRcvDetails=new PrintRcvPaymentDetails();
        private BookingsService bookingsService = new BookingsService();
        private TransactionDetailsViewModel transDetailsvm = new TransactionDetailsViewModel();
        static Func<int, decimal> _getBookingAmount = BookingsService.Get_TotalAmountBook;
        private Func<Booking, List<ICollection<BookAddonsDetail>>> getAddonDetails = BookingAddonDetailsViewModel.GetAddonDetails;
        protected void Page_Load(object sender, EventArgs e)
        {



            if (!IsPostBack)
            {
               
                   var _paymentId = Request["_paymntNo"].Trim();

                   PrintContractDetails conDetails =new PrintContractDetails();
                   List<PrintRcvPaymentDetails> payablelist = new List<PrintRcvPaymentDetails>();

                try
                   {

                       var transId = Payment_Service.GetTransctionIdByPayment(_paymentId);


                        ReportDocument cryRep = new ReportDocument();
        

                        string reportName = "PrintPaymentVouch";

                        string report = Utilities.ReportPath(reportName);

                        cryRep.Load(report);

                        SqlConnectionStringBuilder cnstrbuilding = new SqlConnectionStringBuilder(Utilities.DBGateway());

                        ConnectionInfo crConinfo = new ConnectionInfo();

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

                        CRViewerAccnRecvPayment.ToolPanelView = ToolPanelViewType.None;


                        conDetails = condetails.GetContractDetailsById(transId);

                        payablelist = Payment_Service.GetPaymentsListById(_paymentId).ToList();

                        //var paymentlog = payablelist.FirstOrDefault(t => t.PayNo == _paymentId);

                        var paymentdate = Payment_Service.GetPaymentDate(_paymentId);
                        //decimal totalPackageAmountDue = _getBookingAmount(transId);
                        decimal totalPackageAmountDue = conDetails.packageamount * conDetails.noofPax;
                         var bookings = bookingsService.GetBookingByTransaction(transId);
                         decimal addons = AddonsViewModel.AddonsTotal(getAddonDetails(bookings));


                        decimal locextcharge = BookingsService.Get_extendedAmountLoc(transId);

                         var packageType = conDetails.packageType.TrimEnd();

                        decimal catering_discount = transDetailsvm.GetCateringdiscountByPax(packageType, conDetails.noofPax);


                        decimal totalPayment = Payment_Service.GetTotalPaymentByTransId(transId);

                    //var discount = rcvDetails.Get_bookingDiscountbyTrans(transId, totalPackageAmountDue);


                    cryRep.Database.Tables[0].SetDataSource(conDetails.ToDataTable());
                    cryRep.Database.Tables[1].SetDataSource(payablelist.ToDataTableList());

                    cryRep.SetParameterValue("pmtNo", _paymentId);
                    cryRep.SetParameterValue("paymdate", paymentdate);
                    cryRep.SetParameterValue("addons", addons);
                    cryRep.SetParameterValue("extlocation", locextcharge > 0 ? Convert.ToDecimal(locextcharge * conDetails.noofPax):0);
                    cryRep.SetParameterValue("catdesc", catering_discount> 0 ? Convert.ToDecimal(catering_discount * conDetails.noofPax) : 0);
                    cryRep.SetParameterValue("PrevPayment", totalPayment);
                    cryRep.SetParameterValue("totalPackageAmt", totalPackageAmountDue);
                    cryRep.SetParameterValue("Discounted", 0);


                    Response.Buffer = false;
                    Response.ClearContent();
                    Response.ClearHeaders();


                    cryRep.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "PrintPaymentVouch");

                    //Response.End();
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