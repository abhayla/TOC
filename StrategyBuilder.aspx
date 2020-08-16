<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StrategyBuilder.aspx.cs" Inherits="TOC.StrategyBuilder" MasterPageFile="~/Site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%--<link href="App_Themes/DefaultTheme/earthyblue.css" rel="stylesheet" type="text/css" />--%>
    <style type="text/css">
        .hideGridColumn {
            display: none;
        }
    </style>
    <div>
        <asp:RadioButtonList ID="rblOCType" RepeatDirection="Horizontal" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblOCType_SelectedIndexChanged">
            <asp:ListItem Text="BANKNIFTY" Value="BANKNIFTY"></asp:ListItem>
            <asp:ListItem Text="NIFTY" Value="NIFTY" Selected="True"></asp:ListItem>
        </asp:RadioButtonList>

        <asp:Label runat="server" ID="lblLastFetchedTime" BorderWidth="0"></asp:Label>
        <asp:Label runat="server" Text="Last Price" BorderWidth="0"></asp:Label>
        <asp:Label runat="server" ID="lblLastPrice" BorderWidth="0"></asp:Label>
    </div>
    <div>
        <asp:Label runat="server" ID="lblFilterExpiryDate" Text="Expiry Dates"></asp:Label>
        <asp:DropDownList runat="server" ID="ddlFilterExpiryDates"></asp:DropDownList>

        <asp:Label runat="server" Text="Strategy List"></asp:Label>
        <asp:DropDownList runat="server" ID="ddlStrategyListFilter">
            <asp:ListItem Text="ALL" Value="ALL" Selected="True"></asp:ListItem>
            <asp:ListItem Text="Default" Value="Default"></asp:ListItem>
            <asp:ListItem Text="Strategy1" Value="Strategy1"></asp:ListItem>
            <asp:ListItem Text="Strategy2" Value="Strategy2"></asp:ListItem>
            <asp:ListItem Text="Strategy3" Value="Strategy3"></asp:ListItem>
            <asp:ListItem Text="Strategy4" Value="Strategy4"></asp:ListItem>
            <asp:ListItem Text="Strategy5" Value="Strategy5"></asp:ListItem>
            <asp:ListItem Text="Strategy6" Value="Strategy6"></asp:ListItem>
            <asp:ListItem Text="Strategy7" Value="Strategy7"></asp:ListItem>
            <asp:ListItem Text="Strategy8" Value="Strategy8"></asp:ListItem>
            <asp:ListItem Text="NiftyWatchlist" Value="NiftyWatchlist"></asp:ListItem>
            <asp:ListItem Text="Positions" Value="Positions"></asp:ListItem>
        </asp:DropDownList>

        <asp:Button runat="server" ID="btnFilter" Text="Filter" OnClick="btnFilter_Click" />
    </div>
    <br />
    <div>
        <asp:GridView HeaderStyle-BackColor="LightGray" HeaderStyle-Font-Size="Small" HeaderStyle-Font-Bold="true" RowStyle-HorizontalAlign="Right" RowStyle-Font-Size="Small" FooterStyle-Font-Size="Small" FooterStyle-HorizontalAlign="Right" ID="gvStrategy" runat="server" AutoGenerateColumns="true" OnRowDataBound="gvStrategy_RowDataBound" ShowHeader="true" ShowFooter="true">
            <Columns>
                <asp:TemplateField>
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
                <asp:TemplateField HeaderText="Lots" ItemStyle-Width="40">
                    <ItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlLots" Width="40">
                            <asp:ListItem Text="0" Value="0"></asp:ListItem>
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
                <asp:TemplateField HeaderText="Strategy" ItemStyle-Width="40">
                    <ItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlStrategyList">
                            <asp:ListItem Text="Default" Value="Default"></asp:ListItem>
                            <asp:ListItem Text="Strategy1" Value="Strategy1"></asp:ListItem>
                            <asp:ListItem Text="Strategy2" Value="Strategy2"></asp:ListItem>
                            <asp:ListItem Text="Strategy3" Value="Strategy3"></asp:ListItem>
                            <asp:ListItem Text="Strategy4" Value="Strategy4"></asp:ListItem>
                            <asp:ListItem Text="Strategy5" Value="Strategy5"></asp:ListItem>
                            <asp:ListItem Text="Strategy6" Value="Strategy6"></asp:ListItem>
                            <asp:ListItem Text="Strategy7" Value="Strategy7"></asp:ListItem>
                            <asp:ListItem Text="Strategy8" Value="Strategy8"></asp:ListItem>
                            <asp:ListItem Text="NiftyWatchlist" Value="NiftyWatchlist"></asp:ListItem>
                            <asp:ListItem Text="Positions" Value="Positions"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
        <asp:Button runat="server" Text="Delete" ID="btnDelete" OnClick="btnDelete_Click" />
        <asp:Button runat="server" Text="Add row" ID="btnAddRows" OnClick="btnAddRows_Click" />
        <asp:Button runat="server" Text="Update CMP" ID="btnUpdateCMP" OnClick="btnUpdateCMP_Click" />
        <asp:Button runat="server" Text="Save" ID="btnSave" OnClick="btnSave_Click" />
        <asp:Button runat="server" Text="Add to Order Basket" ID="btnAddToOrderBasket" OnClick="btnAddToOrderBasket_Click" />
    </div>
</asp:Content>
