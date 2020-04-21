<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReadFiles.aspx.cs" Inherits="TOC.ReadFiles" EnableViewState="true" ViewStateMode="Enabled" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Read Files</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="gvFileData" runat="server"></asp:GridView>
        </div>
        <br />
        <asp:Button ID="btnReadOrders" runat="server" Text="Read Orders" OnClick="btnReadOrders_Click" />
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
    </form>
</body>
</html>
