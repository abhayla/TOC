<%@ Page Title="Send Orders" Language="C#" AutoEventWireup="true" CodeBehind="ReadFiles.aspx.cs" Inherits="TOC.ReadFiles" EnableViewState="true" ViewStateMode="Enabled" MasterPageFile="~/Site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:GridView ID="gvFileData" runat="server"></asp:GridView>
    </div>
    <br />
    <asp:Button ID="btnReadOrders" runat="server" Text="Read Orders" OnClick="btnReadOrders_Click" />
    <br />
    <br />
    <asp:Button ID="btnSyncOrders" runat="server" Text="Sync Orders" OnClick="btnSyncOrders_Click" />
    <br />
    <br />
    <asp:Label ID="lblSendTelegramMessages" runat="server" Text="Send Telegram Messages"></asp:Label>
    <br />
    <asp:CheckBox ID="chkSendTelegramMessages" runat="server" Checked="false" />
    <br />
    <br />
    <div>
        <asp:Label ID="Label1" runat="server" Text="Group and Channels: "></asp:Label><asp:Label ID="lblGroupsChannels" runat="server"></asp:Label>
    </div>
    <br />
    <asp:Button ID="btnEquityReport" runat="server" Text="Equity Report" OnClick="btnEquityReport_Click" />
    <br />
    <br />
    <asp:TextBox ID="txtMessage" runat="server" Height="29px" Width="361px"></asp:TextBox>
    <br />
    <br />
    <asp:Button ID="btnShowLogs" runat="server" Text="Show Logs" OnClick="btnShowLogs_Click" />
    <asp:TextBox ID="txtResult" runat="server"></asp:TextBox>
    <br />
    <div>
        <asp:GridView ID="gvLog" runat="server"></asp:GridView>
    </div>
</asp:Content>
