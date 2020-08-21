<%@ Page Title="Position Tracker" Language="C#" AutoEventWireup="true" CodeBehind="Positions.aspx.cs" Inherits="TOC.PositionsTracker" MasterPageFile="~/Site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .hideGridColumn {
            display: none;
        }
    </style>
    <div>
        <asp:Label runat="server" ID="lblFilterExpiryDate" Text="Expiry Dates"></asp:Label>
        <asp:DropDownList runat="server" ID="ddlFilterExpiryDates"></asp:DropDownList>

        <asp:Label runat="server" ID="lblContractType" Text="Contract Type"></asp:Label>
        <asp:DropDownList runat="server" ID="ddlContractType">
            <asp:ListItem Text="All" Value="All"></asp:ListItem>
            <asp:ListItem Text="CE" Value="CE"></asp:ListItem>
            <asp:ListItem Text="PE" Value="PE"></asp:ListItem>
            <asp:ListItem Text="FUT" Value="FUT"></asp:ListItem>
            <asp:ListItem Text="EQ" Value="EQ"></asp:ListItem>
        </asp:DropDownList>

        <asp:Label runat="server" ID="lblFilterProfiles" Text="Profiles"></asp:Label>
        <asp:DropDownList runat="server" ID="ddlFilterProfiles">
            <asp:ListItem Text="All" Value="All"></asp:ListItem>
            <asp:ListItem Text="Abhay" Value="Abhay"></asp:ListItem>
            <asp:ListItem Text="Saurabh" Value="Saurabh"></asp:ListItem>
        </asp:DropDownList>

        <asp:Label runat="server" ID="lblFilterStrategy" Text="Strategy"></asp:Label>
        <asp:DropDownList runat="server" ID="ddlFilterStrategy">
            <asp:ListItem Text="All" Value="All"></asp:ListItem>
            <asp:ListItem Text="Butterfly" Value="Butterfly"></asp:ListItem>
            <asp:ListItem Text="Iron Condor" Value="Iron Condor"></asp:ListItem>
            <asp:ListItem Text="Naked Call" Value="Naked Call"></asp:ListItem>
            <asp:ListItem Text="Naked Put" Value="Naked Put"></asp:ListItem>
            <asp:ListItem Text="Straddle" Value="Straddle"></asp:ListItem>
            <asp:ListItem Text="Strangle" Value="Strangle"></asp:ListItem>
            <asp:ListItem Text="Spreads" Value="Spreads"></asp:ListItem>
            <asp:ListItem Text="Ratio Spreads" Value="Ratio Spreads"></asp:ListItem>
        </asp:DropDownList>

        <asp:Label runat="server" ID="lblFilterPostionStatus" Text="Postion Status"></asp:Label>
        <asp:DropDownList runat="server" ID="ddlFilterPostionStatus">
            <asp:ListItem Text="All" Value="All"></asp:ListItem>
            <asp:ListItem Text="Open" Value="Open"></asp:ListItem>
            <asp:ListItem Text="Close" Value="Close"></asp:ListItem>
        </asp:DropDownList>

        <asp:Button runat="server" ID="btnFilter" Text="Filter" OnClick="btnFilter_Click" />
    </div>
    <br />
    <div>
        <asp:GridView HeaderStyle-BackColor="LightGray" HeaderStyle-Font-Size="Small" HeaderStyle-Font-Bold="true" RowStyle-HorizontalAlign="Right" RowStyle-Font-Size="Small" FooterStyle-Font-Size="Small" FooterStyle-HorizontalAlign="Right" ID="gvPosTracker" runat="server" AutoGenerateColumns="false" ShowHeader="true" ShowFooter="true" OnRowDataBound="gvPosTracker_RowDataBound">
            <Columns>
                <asp:TemplateField ItemStyle-Width="10">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chkSelect" />
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
                <asp:TemplateField HeaderText="Entry Date">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txtEntryDate" Width="130" TextMode="Date" ToolTip="dd-MMM-yyyy"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Entry Price" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txtEntryPrice" Width="50"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Exit Price" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txtExitPrice" Width="50"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="CMP" HeaderText="CMP" ItemStyle-Width="50" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="UnRealisedPL" ItemStyle-Width="50" HeaderText="Unrealised P/L" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="ChgPer" ItemStyle-Width="50" HeaderText="Chg %" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="RealisedPL" ItemStyle-Width="50" HeaderText="Realised P/L" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="MaxProfit" HeaderText="Max Profit" ItemStyle-HorizontalAlign="Right" />
                <asp:TemplateField HeaderText="Strategy" ItemStyle-Width="100">
                    <ItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlStrategy" Width="100">
                            <asp:ListItem Text="Butterfly" Value="Butterfly"></asp:ListItem>
                            <asp:ListItem Text="Iron Condor" Value="Iron Condor"></asp:ListItem>
                            <asp:ListItem Text="Naked Call" Value="Naked Call"></asp:ListItem>
                            <asp:ListItem Text="Naked Put" Value="Naked Put"></asp:ListItem>
                            <asp:ListItem Text="Straddle" Value="Straddle"></asp:ListItem>
                            <asp:ListItem Text="Strangle" Value="Strangle"></asp:ListItem>
                            <asp:ListItem Text="Spreads" Value="Spreads"></asp:ListItem>
                            <asp:ListItem Text="Synthetic Future" Value="Synthetic Future"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Profile" ItemStyle-Width="80">
                    <ItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlProfile" Width="80">
                            <asp:ListItem Text="Abhay" Value="Abhay"></asp:ListItem>
                            <asp:ListItem Text="Saurabh" Value="Saurabh"></asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Position" HeaderText="Position" />
                <asp:BoundField DataField="Id" HeaderText="Id" />

                <asp:BoundField DataField="DaysToExpiry" HeaderText="Days To Expiry" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="DaysHeld" HeaderText="Days Held" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="Recommendation" HeaderText="Recommendation" />
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <asp:Button runat="server" Text="Delete" ID="btnDelete" OnClick="btnDelete_Click" />
    <asp:Button runat="server" Text="Add row" ID="btnAddRows" OnClick="btnAddRows_Click" />
    <asp:Button runat="server" Text="Refresh" ID="btnUpdateCMP" OnClick="btnUpdateCMP_Click" />
    <asp:Button runat="server" Text="Save" ID="btnSave" OnClick="btnSave_Click" />
    <asp:Button runat="server" Text="Add to Strategy Builder" ID="btnAddToStrategyBuilder" OnClick="btnAddToStrategyBuilder_Click" />
</asp:Content>
