<%@ Page Title="Filter All" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FilterAllStrategies.aspx.cs" Inherits="TOC.FilterAllStrategies" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:Label runat="server" ID="lblStart"></asp:Label>
        <asp:Label runat="server" ID="lblEnd"></asp:Label>
        <asp:Label runat="server" ID="lblDifference"></asp:Label>
        <asp:RadioButtonList ID="rblOCType" RepeatDirection="Horizontal" runat="server" AutoPostBack="false" BorderWidth="0">
            <asp:ListItem Text="BANKNIFTY" Value="BANKNIFTY"></asp:ListItem>
            <asp:ListItem Text="NIFTY" Value="NIFTY" Selected="True"></asp:ListItem>
        </asp:RadioButtonList>
        <asp:Label runat="server" Text="Last Price" BorderWidth="0"></asp:Label>
        <asp:Label runat="server" ID="lblLastPrice" BorderWidth="0"></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblLastFetchedTime" BorderWidth="0"></asp:Label>
        <asp:DropDownList runat="server" ID="ddlExpiryDates">
        </asp:DropDownList>
        <asp:DropDownList runat="server" ID="ddlContractType">
            <asp:ListItem Text="ALL" Value="ALL" Selected="True"></asp:ListItem>
            <asp:ListItem Text="CE" Value="CE"></asp:ListItem>
            <asp:ListItem Text="PE" Value="PE"></asp:ListItem>
        </asp:DropDownList>
        <%--<asp:Button runat="server" Text="Fetch" ID="btnFetch" OnClick="btnFetch_Click" />--%>
    </div>
    <div>
        <asp:Label runat="server" Text="Filter Options" ID="lblFilterOptions"></asp:Label>
        <asp:DropDownList runat="server" ID="ddlFilterOptions">
            <asp:ListItem Text="RANGE" Value="RANGE" Selected="True"></asp:ListItem>
            <asp:ListItem Text="EXPIRY" Value="EXPIRY"></asp:ListItem>
        </asp:DropDownList>
        <asp:Label runat="server" ID="lblSPLowerRange" Text="Lower Range"></asp:Label>
        <asp:DropDownList runat="server" ID="ddlSPLowerRange">
        </asp:DropDownList>
        <asp:Label runat="server" ID="lblSPExpiry" Text="Expected Expiry"></asp:Label>
        <asp:DropDownList runat="server" ID="ddlSPExpiry">
            <asp:ListItem Text="NONE" Value="NONE" Selected="True"></asp:ListItem>
        </asp:DropDownList>
        <asp:Label runat="server" ID="lblSPHigherRange" Text="Higher Range"></asp:Label>
        <asp:DropDownList runat="server" ID="ddlSPHigherRange">
        </asp:DropDownList>
        <asp:Button runat="server" Text="Filter Results" ID="btnFilterResults" OnClick="btnFilterResults_Click" />
    </div>
    <br />
    <div runat="server" id="divMain">
    </div>
    <script src="https://kite.trade/publisher.js?v=3"></script>
    <script src="../Scripts/BasketOrders.js" type="text/javascript"></script>
    <script>
        var kite = null;
        KiteConnect.ready(function () {
            kite = new KiteConnect("d805j3f0aeciwx8g");
            kite.link('span:has(button)');
            //kite.link("#custom-button");
            //kite.renderButton("#custom-button");
            //kite.renderButton("#default-button");
            //kite.renderButton('span:has(default-button)');
        });
    </script>
</asp:Content>
