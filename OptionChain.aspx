<%@ Page Title="Option Chain" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OptionChain.aspx.cs" Inherits="TOC.OptionChain" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:RadioButtonList ID="rblOCType" RepeatDirection="Horizontal" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblOCType_SelectedIndexChanged">
            <asp:ListItem Text="BANKNIFTY" Value="BANKNIFTY" Selected="True"></asp:ListItem>
            <asp:ListItem Text="NIFTY" Value="NIFTY"></asp:ListItem>
        </asp:RadioButtonList>
        <asp:Button ID="btnRefresh" runat="server" Text="Refresh Data" OnClick="btnRefresh_Click" />
        <asp:Button ID="btnShowCalculation" runat="server" Text="Show Calculation" OnClick="btnShowCalculation_Click" />
        <asp:Button ID="btnGetButterflySpread" runat="server" Text="Get Butterfly Spread" OnClick="btnGetButterflySpread_Click" />
        <asp:DropDownList ID="ddlExpiryDates" runat="server"></asp:DropDownList>
    </div>
    <br />
    <div runat="server" id="divOptionChain">
        <asp:GridView AutoGenerateColumns="False" AllowPaging="false" runat="server" ID="gvOptionChain">
            <Columns>
                <asp:BoundField DataField="CEopenInterest" HeaderText="OI" />
                <asp:BoundField DataField="CEchangeinOpenInterest" HeaderText="Chng in OI" />
                <asp:BoundField DataField="CEtotalTradedVolume" HeaderText="Volume" />
                <asp:BoundField DataField="CEimpliedVolatility" HeaderText="IV" />
                <asp:BoundField DataField="CElastPrice" HeaderText="LTP" />
                <asp:BoundField DataField="CEchange" HeaderText="Net Chng" />
                <asp:BoundField DataField="CEbidQty" HeaderText="Bid Qty" />
                <asp:BoundField DataField="CEbidprice" HeaderText="Bid Price" />
                <asp:BoundField DataField="CEaskPrice" HeaderText="Ask Price" />
                <asp:BoundField DataField="CEaskQty" HeaderText="Ask Qty" />

                <asp:BoundField DataField="strikePrice" HeaderText="Strike Price" />

                <asp:BoundField DataField="PEbidQty" HeaderText="Bid Qty" />
                <asp:BoundField DataField="PEbidprice" HeaderText="Bid Price" />
                <asp:BoundField DataField="PEaskPrice" HeaderText="Ask Price" />
                <asp:BoundField DataField="PEaskQty" HeaderText="Ask Qty" />
                <asp:BoundField DataField="PEchange" HeaderText="Net Chng" />
                <asp:BoundField DataField="PElastPrice" HeaderText="LTP" />
                <asp:BoundField DataField="PEimpliedVolatility" HeaderText="IV" />
                <asp:BoundField DataField="PEtotalTradedVolume" HeaderText="Volume" />
                <asp:BoundField DataField="PEchangeinOpenInterest" HeaderText="Chng in OI" />
                <asp:BoundField DataField="PEopenInterest" HeaderText="OI" />
            </Columns>
        </asp:GridView>
    </div>
    <div runat="server" id="divButterflySpread">
        <table runat="server" id="tblButterflySpread">
            <tr>
                <td>
                    <asp:GridView></asp:GridView>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
