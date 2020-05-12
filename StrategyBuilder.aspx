<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StrategyBuilder.aspx.cs" Inherits="TOC.StrategyBuilder" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="Scripts/myscript.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblOCType" runat="server" Text="OC Type"></asp:Label>
                        <asp:RadioButtonList ID="rblOCType" RepeatDirection="Horizontal" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rblOCType_SelectedIndexChanged">
                            <asp:ListItem Text="BANKNIFTY" Value="BANKNIFTY"></asp:ListItem>
                            <asp:ListItem Text="NIFTY" Value="NIFTY" Selected="True"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>
                        <asp:Label ID="lblLotSize" runat="server" Text="Lot Size"></asp:Label>
                        <asp:Label ID="lblLotSizeValue" runat="server" Text="75"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <asp:GridView ID="gvStrategy" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvStrategy_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Delete">
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
                    <asp:TemplateField HeaderText="Strike Price" ItemStyle-Width="50">
                        <ItemTemplate>
                            <asp:DropDownList runat="server" ID="ddlStrikePrice" Width="50">
                                <asp:ListItem Text="8500" Value="8500"></asp:ListItem>
                                <asp:ListItem Text="8600" Value="8600"></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Premium" ItemStyle-Width="50">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtPremium" Width="50"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Lots" ItemStyle-Width="50">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="txtLots" Width="50"></asp:TextBox>
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
            <asp:Button runat="server" Text="Add more rows" ID="btnAddRows" OnClick="btnAddRows_Click" />
        </div>
    </form>
</body>
</html>
