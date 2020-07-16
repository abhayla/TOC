<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FilterOptions.aspx.cs" Inherits="TOC.FilterOptions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:RadioButtonList ID="rblOCType" RepeatDirection="Horizontal" runat="server" AutoPostBack="True">
            <asp:ListItem Text="BANKNIFTY" Value="BANKNIFTY"></asp:ListItem>
            <asp:ListItem Text="NIFTY" Value="NIFTY" Selected="True"></asp:ListItem>
        </asp:RadioButtonList>
        <asp:Label runat="server" ID="lblExpiryDates" Text="Expiry Dates"></asp:Label>
        <asp:DropDownList ID="ddlExpiryDates" runat="server"></asp:DropDownList>
        <asp:DropDownList runat="server" ID="ddlContractType">
            <asp:ListItem Text="ALL" Value="ALL" Selected="True"></asp:ListItem>
            <asp:ListItem Text="CE" Value="CE"></asp:ListItem>
            <asp:ListItem Text="PE" Value="PE"></asp:ListItem>
        </asp:DropDownList>
        <asp:Label runat="server" ID="lblWeeksToExpiry" Text="Weeks To Expiry"></asp:Label>
        <asp:DropDownList runat="server" ID="ddlWeeksToExpiry">
            <asp:ListItem Value="1" Text="1"></asp:ListItem>
            <asp:ListItem Value="2" Text="2"></asp:ListItem>
            <asp:ListItem Value="3" Text="3"></asp:ListItem>
            <asp:ListItem Value="4" Text="4"></asp:ListItem>
            <asp:ListItem Value="5" Text="5"></asp:ListItem>
            <asp:ListItem Value="6" Text="6"></asp:ListItem>
            <asp:ListItem Value="7" Text="7"></asp:ListItem>
            <asp:ListItem Value="8" Text="8" Selected="True"></asp:ListItem>
            <asp:ListItem Value="9" Text="9"></asp:ListItem>
        </asp:DropDownList>

        <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" />
    </div>
    <br />
    <div runat="server" id="divFilterOptionChain">
        <asp:GridView AutoGenerateColumns="False" AllowPaging="false" runat="server" ID="gvFilterOptionChain">
            <Columns>
                <asp:BoundField DataField="CEopenInterest" HeaderText="OI" />
                <asp:BoundField DataField="CEchangeinOpenInterest" HeaderText="Chng in OI" />
                <asp:BoundField DataField="CEtotalTradedVolume" HeaderText="Volume" />
                <asp:BoundField DataField="CEimpliedVolatility" HeaderText="IV" />
                <asp:BoundField DataField="CElastPrice" HeaderText="LTP" />
                <asp:BoundField DataField="CEchange" HeaderText="Net Chng" />
                <asp:BoundField DataField="strikePrice" HeaderText="Strike Price" />
                <asp:BoundField DataField="PEchange" HeaderText="Net Chng" />
                <asp:BoundField DataField="PElastPrice" HeaderText="LTP" />
                <asp:BoundField DataField="PEimpliedVolatility" HeaderText="IV" />
                <asp:BoundField DataField="PEtotalTradedVolume" HeaderText="Volume" />
                <asp:BoundField DataField="PEchangeinOpenInterest" HeaderText="Chng in OI" />
                <asp:BoundField DataField="PEopenInterest" HeaderText="OI" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
