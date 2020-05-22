<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ButterflySpread.aspx.cs" Inherits="TOC.ButterflySpread" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

</head>
<body>
    <form id="form1" runat="server" method="get">

        <div>
            <table>
                <tr>
                    <td>
                        <asp:RadioButtonList ID="rblOCType" RepeatDirection="Horizontal" runat="server" AutoPostBack="false" BorderWidth="0">
                            <asp:ListItem Text="BANKNIFTY" Value="BANKNIFTY"></asp:ListItem>
                            <asp:ListItem Text="NIFTY" Value="NIFTY" Selected="True"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:Label runat="server" Text="Last Price" BorderWidth="0"></asp:Label>
                        <asp:Label runat="server" ID="lblLastPrice" BorderWidth="0"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblLastFetchedTime" BorderWidth="0"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlExpiryDates">
                        </asp:DropDownList>
                        <asp:DropDownList runat="server" ID="ddlContractType">
                            <asp:ListItem Text="CE" Value="CE"></asp:ListItem>
                            <asp:ListItem Text="PE" Value="PE" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="ALL" Value="ALL"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:Button runat="server" Text="Fetch" ID="btnFetch" OnClick="btnFetch_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" Text="Filter Options" ID="lblFilterOptions"></asp:Label>
                        <asp:DropDownList runat="server" ID="ddlFilterOptions">
                        </asp:DropDownList>
                    </td>
                    <td></td>
                </tr>
            </table>
        </div>
        <br />
        <div runat="server" id="divMain">
        </div>
    </form>
<%--    <form method="post" id="basket-form" action="https://kite.trade/connect/basket">
        <input type="hidden" name="api_key" value="d805j3f0aeciwx8g" />
        <input type="hidden" id="basket" name="data" value="" />
    </form>--%>
    <script src="https://kite.trade/publisher.js?v=3"></script>
    <script src="Scripts/BasketOrders.js" type="text/javascript"></script>
    <script>
        var kite = null;
        KiteConnect.ready(function () {
            kite = new KiteConnect("d805j3f0aeciwx8g");
            kite.link('span:has(button)');
            kite.link("#custom-button");
            kite.renderButton("#custom-button");
            //alert("KiteConnect.ready loaded"); 
        });
    </script>
</body>
</html>
