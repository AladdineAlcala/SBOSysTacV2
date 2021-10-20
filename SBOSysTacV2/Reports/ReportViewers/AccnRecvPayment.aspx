<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccnRecvPayment.aspx.cs" Inherits="SBOSysTacV2.Reports.ReportViewers.AccnRecvPayment" %>
<%@ Register TagPrefix="CR" Namespace="CrystalDecisions.Web" Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<form id="form1" runat="server">
    <div>
        <div>
            <CR:CrystalReportViewer ID="CRViewerAccnRecvPayment" runat="server"  Width="100%" Height="100%"  AutoDataBind="True" ToolPanelView="None" EnableParameterPrompt="false"  />
        </div>
    </div>
</form>
</body>
</html>
