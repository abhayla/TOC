<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NiftyOptionChain.aspx.cs" Inherits="TOC.NiftyOptionChain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--    <link href="App_Themes/DefaultTheme/earthyblue.css" rel="stylesheet" type="text/css" />--%>
    <div runat="server" id="divNiftyWatchlist">
        <table style="width: 100%">
            <tr>
                <td>
                    <asp:DropDownList ID="ddlExpiryDates" runat="server"></asp:DropDownList>

                    <asp:Label runat="server" ID="lblContractType" Text="Contract Type"></asp:Label>
                    <asp:DropDownList runat="server" ID="ddlContractType">
                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                        <asp:ListItem Text="CE" Value="CE"></asp:ListItem>
                        <asp:ListItem Text="PE" Value="PE"></asp:ListItem>
                    </asp:DropDownList>

                    <asp:Label runat="server" Text="ImpliedVolatility % >= " BorderWidth="0"></asp:Label>
                    <asp:DropDownList ID="ddlImpliedVolatility" runat="server">
                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                        <asp:ListItem Text="40" Value="40"></asp:ListItem>
                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                        <asp:ListItem Text="60" Value="60"></asp:ListItem>
                        <asp:ListItem Text="70" Value="70"></asp:ListItem>
                        <asp:ListItem Text="80" Value="80"></asp:ListItem>
                        <asp:ListItem Text="90" Value="90"></asp:ListItem>
                        <asp:ListItem Text="100" Value="100"></asp:ListItem>
                    </asp:DropDownList>

                    <asp:Label runat="server" Text="LTP >= " BorderWidth="0"></asp:Label>
                    <asp:DropDownList ID="ddlLTP" runat="server">
                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                        <asp:ListItem Text="40" Value="40"></asp:ListItem>
                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                        <asp:ListItem Text="60" Value="60"></asp:ListItem>
                        <asp:ListItem Text="70" Value="70"></asp:ListItem>
                        <asp:ListItem Text="80" Value="80"></asp:ListItem>
                        <asp:ListItem Text="90" Value="90"></asp:ListItem>
                        <asp:ListItem Text="100" Value="100"></asp:ListItem>
                    </asp:DropDownList>

                    <asp:Label runat="server" Text="Extrinsic Value % >= " BorderWidth="0"></asp:Label>
                    <asp:DropDownList ID="ddlExtrinsicValuePer" runat="server">
                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
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

                    <asp:Label runat="server" Text="Ext Val/Day >= " BorderWidth="0"></asp:Label>
                    <asp:DropDownList ID="ddlExtValPerDay" runat="server">
                        <asp:ListItem Text="All" Value="All"></asp:ListItem>
                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                        <asp:ListItem Text="40" Value="40"></asp:ListItem>
                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                        <asp:ListItem Text="60" Value="60"></asp:ListItem>
                        <asp:ListItem Text="70" Value="70"></asp:ListItem>
                        <asp:ListItem Text="80" Value="80"></asp:ListItem>
                        <asp:ListItem Text="90" Value="90"></asp:ListItem>
                        <asp:ListItem Text="100" Value="100"></asp:ListItem>
                    </asp:DropDownList>

                    <asp:Button ID="btnFilter" runat="server" Text="Filter" OnClick="btnFilter_Click" />
                </td>
                <td style="text-align: right">
                    <asp:Label runat="server" Text="Last Price" BorderWidth="0"></asp:Label>
                    <asp:Label runat="server" ID="lblLastPrice" BorderWidth="0"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <asp:GridView EditRowStyle-Font-Size="Small" HeaderStyle-BackColor="LightGray" HeaderStyle-Font-Size="Small" HeaderStyle-Font-Bold="true" RowStyle-HorizontalAlign="Right" RowStyle-Font-Size="Small" FooterStyle-Font-Size="Small" FooterStyle-HorizontalAlign="Right" AutoGenerateColumns="False" AllowPaging="false" runat="server" ID="gvNiftyWatchlist" HorizontalAlign="Center" OnRowDataBound="gvNiftyWatchlist_RowDataBound">
            <Columns>
                <asp:TemplateField ItemStyle-Width="10">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chkCE" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CETradingSymbol" HeaderText="CE Trading Symbol" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="CEopenInterest" HeaderText="OI" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="CEchangeinOpenInterest" HeaderText="Chng in OI" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="CEtotalTradedVolume" HeaderText="Volume" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="CEimpliedVolatility" HeaderText="IV" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="CElastPrice" HeaderText="LTP" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="CEchange" HeaderText="Net Chng" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="CEDelta" HeaderText="CE Delta" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="CEExtrinsicValue" HeaderText="Ext Val" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="CEExtrinsicValuePer" HeaderText="Ext Val %" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="CEExtrinsicValuePerDay" HeaderText="Ext Val/Day" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="StrikePrice" HeaderText="Strike Price" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry Date" ItemStyle-HorizontalAlign="Right" ItemStyle-Wrap="false" />
                <asp:BoundField DataField="DaysToExpiry" HeaderText="D.T.E." ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="SD" HeaderText="SD" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="PEExtrinsicValuePerDay" HeaderText="Ext Val/Day" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="PEExtrinsicValuePer" HeaderText="Ext Val %" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="PEExtrinsicValue" HeaderText="Ext Val" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="PEDelta" HeaderText="PE Delta" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="PEchange" HeaderText="Net Chng" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="PElastPrice" HeaderText="LTP" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="PEimpliedVolatility" HeaderText="IV" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="PEtotalTradedVolume" HeaderText="Volume" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="PEchangeinOpenInterest" HeaderText="Chng in OI" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="PEopenInterest" HeaderText="OI" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="PETradingSymbol" HeaderText="PE Trading Symbol" ItemStyle-HorizontalAlign="Right" />
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
    <asp:Button runat="server" ID="btnAddToPositions" Text="Add To Positions" OnClick="btnAddToPositions_Click" />
    <asp:Button runat="server" ID="btnSuggestNoLossStrategy" Text="Suggest No Loss Strategy" />
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
