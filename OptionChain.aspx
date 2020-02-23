<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OptionChain.aspx.cs" Inherits="TOC.OptionChain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button ID="btnRefresh" runat="server" Text="Refresh Data" OnClick="btnRefresh_Click" />
            <asp:Button ID="btnGetButterflySpread" runat="server" Text="Get Butterfly Spread" OnClick="btnGetButterflySpread_Click" />
            <asp:DropDownList ID="ddlExpiryDates" runat="server"></asp:DropDownList>
        </div>
        <div runat="server" id="divOptionChain">
        </div>
        <div runat="server" id="divButterflySpread">
        </div>
    </form>
</body>
</html>
