<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CrystalReport.aspx.cs" Inherits="WebReportHightJump.CrystalReport" %>

<%--<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>--%>

<%--<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>--%>
<%--<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>--%>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lbError" runat="server" Text="Label" ForeColor="Red"></asp:Label>
    <hr />
    <asp:Panel ID="Panel1" runat="server" Width="98%" ScrollBars="auto">
        <asp:Label ID="LabelPrinterName" runat="server" Text="Label" Visible="False"></asp:Label><br />
        <asp:Button ID="ButtonPrint" runat="server" Text="Print" 
            onclick="ButtonPrint_Click" />
        <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True"
            GroupTreeImagesFolderUrl="" Height="940px" ToolbarImagesFolderUrl="" ToolPanelWidth="200px"
            Width="1210px" ToolPanelView="None" EnableDatabaseLogonPrompt="False" 
          />
     
        <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
            <Report FileName="CrystalReports\Report13_.rpt">
            </Report>
        </CR:CrystalReportSource>
     
    </asp:Panel>
 
</asp:Content>
