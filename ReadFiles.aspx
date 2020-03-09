<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReadFiles.aspx.cs" Inherits="TOC.ReadFiles" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Read Files</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="gvFileData" runat="server"></asp:GridView>
        </div>
        <br />
        <asp:Button ID="btnReadOrders" runat="server" Text="Read Orders" OnClick="btnReadOrders_Click" />
    </form>
</body>
</html>
