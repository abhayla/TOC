<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StrategyBuilder.aspx.cs" Inherits="TOC.StrategyBuilder" MasterPageFile="~/Site.Master" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:RadioButtonList ID="rblOCType" RepeatDirection="Horizontal" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblOCType_SelectedIndexChanged">
            <asp:ListItem Text="BANKNIFTY" Value="BANKNIFTY"></asp:ListItem>
            <asp:ListItem Text="NIFTY" Value="NIFTY" Selected="True"></asp:ListItem>
        </asp:RadioButtonList>
    </div>
    <br />
    <div>
        <asp:Label ID="lblLotSize" runat="server" Text="Lot Size"></asp:Label>
        <asp:Label ID="lblLotSizeValue" runat="server" Text="75"></asp:Label>
        <asp:DropDownList runat="server" ID="ddlExpiryDates">
        </asp:DropDownList>
    </div>
    <br />
    <div>
        <asp:GridView ID="gvStrategy" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvStrategy_RowDataBound" ShowHeader="true" ShowFooter="true">
            <Columns>
                <asp:TemplateField HeaderText="Select">
                    <ItemTemplate>
                        <asp:CheckBox runat="server" ID="chkDelete" />
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
                <asp:TemplateField HeaderText="Transaction Type" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:DropDownList runat="server" ID="ddlTransactionType" Width="50">
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
                <asp:BoundField HeaderText="CMP" ItemStyle-Width="50" />
                <asp:TemplateField HeaderText="Premium Paid" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txtPremium" Width="50"></asp:TextBox>
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
                <asp:BoundField ItemStyle-Width="50" />
                <asp:BoundField ItemStyle-Width="50" />
                <asp:BoundField ItemStyle-Width="50" />
                <asp:BoundField ItemStyle-Width="50" />
                <asp:BoundField ItemStyle-Width="50" />
                <asp:BoundField ItemStyle-Width="50" />
                <asp:BoundField ItemStyle-Width="50" />
                <asp:BoundField ItemStyle-Width="50" />
                <asp:BoundField ItemStyle-Width="50" />
                <asp:BoundField ItemStyle-Width="50" />
                <asp:BoundField ItemStyle-Width="50" />
                <asp:BoundField ItemStyle-Width="50" />
                <asp:BoundField ItemStyle-Width="50" />
                <asp:BoundField ItemStyle-Width="50" />
                <asp:BoundField ItemStyle-Width="50" />
                <asp:BoundField ItemStyle-Width="50" />
                <asp:BoundField ItemStyle-Width="50" />
                <asp:BoundField ItemStyle-Width="50" />
                <asp:BoundField ItemStyle-Width="50" />
                <asp:BoundField ItemStyle-Width="50" />
                <asp:BoundField ItemStyle-Width="50" />
            </Columns>
        </asp:GridView>
        <br />
        <asp:Button runat="server" Text="Add row" ID="btnAddRows" OnClick="btnAddRows_Click" />
        <asp:Button runat="server" Text="Update CMP" ID="btnUpdateCMP" OnClick="btnUpdateCMP_Click" />
    </div>
</asp:Content>
