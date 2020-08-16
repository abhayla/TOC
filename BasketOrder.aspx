<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BasketOrder.aspx.cs" Inherits="TOC.BasketOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div runat="server" id="divNiftyWatchlist">
        <asp:GridView HeaderStyle-Font-Size="Small" HeaderStyle-Font-Bold="true" RowStyle-Font-Size="Small" AutoGenerateColumns="False" AllowPaging="false" runat="server" ID="gvBasketOrder" HorizontalAlign="Center" OnRowDataBound="gvBasketOrder_RowDataBound">
            <Columns>
                <asp:TemplateField ItemStyle-Width="10">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chkSelect" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Expiry Date" ItemStyle-Width="105">
                    <ItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlExpiryDates" Width="105">
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Contract Type" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlContractType">
                            <asp:ListItem Text="CE" Value="CE"></asp:ListItem>
                            <asp:ListItem Text="PE" Value="PE"></asp:ListItem>
                            <asp:ListItem Text="FUT" Value="FUT"></asp:ListItem>
                            <asp:ListItem Text="EQ" Value="EQ"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Transaction Type" ItemStyle-Width="70">
                    <ItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlTransactionType" Width="70">
                            <asp:ListItem Text="BUY" Value="BUY"></asp:ListItem>
                            <asp:ListItem Text="SELL" Value="SELL"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Strike Price" ItemStyle-Width="80">
                    <ItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlStrikePrice" Width="80">
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="OC Type" ItemStyle-Width="105">
                    <ItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlOCType" Width="105">
                            <asp:ListItem Text="BANKNIFTY" Value="BANKNIFTY"></asp:ListItem>
                            <asp:ListItem Text="NIFTY" Value="NIFTY" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Order Type" ItemStyle-Width="105">
                    <ItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlOrderType" Width="105">
                            <asp:ListItem Text="LIMIT" Value="LIMIT" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="MARKET" Value="MARKET"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Limit Price" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txtLimitPrice" Width="50"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Lots" ItemStyle-Width="40">
                    <ItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlLots" Width="40">
                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                            <asp:ListItem Text="2" Value="2"></asp:ListItem>
                            <asp:ListItem Text="3" Value="3"></asp:ListItem>
                            <asp:ListItem Text="4" Value="4"></asp:ListItem>
                            <asp:ListItem Text="5" Value="5"></asp:ListItem>
                            <asp:ListItem Text="6" Value="6"></asp:ListItem>
                            <asp:ListItem Text="7" Value="7"></asp:ListItem>
                            <asp:ListItem Text="8" Value="8"></asp:ListItem>
                            <asp:ListItem Text="9" Value="9"></asp:ListItem>
                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CMP" HeaderText="CMP" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="TradingSymbol" HeaderText="Trading Symbol" ItemStyle-HorizontalAlign="Right" />
            </Columns>
        </asp:GridView>
        <br />
        <asp:Button runat="server" Text="Delete" ID="btnDelete" OnClick="btnDelete_Click" />
        <asp:Button runat="server" Text="Add row" ID="btnAddRows" OnClick="btnAddRows_Click" />
        <asp:Button runat="server" Text="Save" ID="btnSave" OnClick="btnSave_Click" />
        <asp:Button runat="server" Text="Place Order" ID="btnPlaceOrder" OnClick="btnPlaceOrder_Click" />
        <span>
            <button id="btnBuyBasketOrder" runat="server">Buy Basket Order</button></span>
        <script src="https://kite.trade/publisher.js?v=3"></script>
        <script src="../Scripts/BasketOrders.js" type="text/javascript"></script>
        <script>
            var kite = null;
            KiteConnect.ready(function () {
                kite = new KiteConnect("drt13a4n12vorpac");
                kite.link('span:has(button)');
            });
        </script>
    </div>
</asp:Content>
