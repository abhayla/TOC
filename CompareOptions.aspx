<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompareOptions.aspx.cs" Inherits="TOC.CompareOptions" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="gvCompareOptions" runat="server" AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField HeaderText="OC Type" ItemStyle-Width="50">
                        <ItemTemplate>
                            <asp:DropDownList runat="server" ID="ddlOCType">
                                <asp:ListItem Text="NIFTY" Value="NIFTY"></asp:ListItem>
                                <asp:ListItem Text="BANKNIFTY" Value="BANKNIFTY"></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Expiry" ItemStyle-Width="80">
                        <ItemTemplate>
                            <asp:DropDownList runat="server" ID="ddlExpiry" Width="80">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Strike Price" ItemStyle-Width="80">
                        <ItemTemplate>
                            <asp:DropDownList runat="server" ID="ddlStrikePrice" Width="80">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity" ItemStyle-Width="80">
                        <ItemTemplate>
                            <asp:DropDownList runat="server" ID="ddlQuantity" Width="80">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Margin Required" ItemStyle-Width="50" />
                    <asp:BoundField HeaderText="Max Loss" ItemStyle-Width="50" />
                    <asp:BoundField HeaderText="Max Profit" ItemStyle-Width="50" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
