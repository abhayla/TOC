<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NiftyWatchlist.aspx.cs" Inherits="TOC.NiftyWatchlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:DropDownList ID="ddlExpiryDates" runat="server"></asp:DropDownList>
        <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" />
    </div>
    <br />
    <div runat="server" id="divNiftyWatchlist">
        <asp:GridView AutoGenerateColumns="False" AllowPaging="false" runat="server" ID="gvNiftyWatchlist" HorizontalAlign="Center">
            <Columns>
                <asp:TemplateField ItemStyle-Width="10">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chkCE" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CETradingSymbol" HeaderText="CE Trading Symbol" />
                <asp:BoundField DataField="CEopenInterest" HeaderText="OI" />
                <asp:BoundField DataField="CEchangeinOpenInterest" HeaderText="Chng in OI" />
                <asp:BoundField DataField="CEtotalTradedVolume" HeaderText="Volume" />
                <asp:BoundField DataField="CEimpliedVolatility" HeaderText="IV" />
                <asp:BoundField DataField="CElastPrice" HeaderText="LTP" />
                <asp:BoundField DataField="CEchange" HeaderText="Net Chng" />
                <asp:BoundField DataField="strikePrice" HeaderText="Strike Price" />
                <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" />
                <asp:BoundField DataField="PEchange" HeaderText="Net Chng" />
                <asp:BoundField DataField="PElastPrice" HeaderText="LTP" />
                <asp:BoundField DataField="PEimpliedVolatility" HeaderText="IV" />
                <asp:BoundField DataField="PEtotalTradedVolume" HeaderText="Volume" />
                <asp:BoundField DataField="PEchangeinOpenInterest" HeaderText="Chng in OI" />
                <asp:BoundField DataField="PEopenInterest" HeaderText="OI" />
                <asp:BoundField DataField="PETradingSymbol" HeaderText="PE Trading Symbol" />
                <asp:TemplateField ItemStyle-Width="10">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chkPE" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <span>
        <button id="btnBuyBasketOrder" runat="server">Buy Basket Order</button></span>
    <asp:Button runat="server" ID="btnAddToPositions" Text="Add To Positions" />
    <asp:Button runat="server" ID="btnAddToMyWatchlist" Text="Add To My Watchlist" />
    <asp:Button runat="server" ID="btnAddToStrategyBuilder" Text="Add To Strategy Builder" OnClick="btnAddToStrategyBuilder_Click" />
    <%--<span>
        <input type="button" class="btn btn-success" onclick="CreateBasketOfOrders(gvNiftyWatchlist)" value="Value" title="Title" /></span>--%>
    <script src="https://kite.trade/publisher.js?v=3"></script>
    <script src="../Scripts/BasketOrders.js" type="text/javascript"></script>
    <script>
        var kite = null;
        KiteConnect.ready(function () {
            kite = new KiteConnect("drt13a4n12vorpac");
            kite.link('span:has(button)');
        });
    </script>
</asp:Content>
