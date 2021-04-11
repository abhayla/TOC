<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TOC.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>Login using Google</h1>
            <p>
                <asp:TextBox ID="txtGmail" runat="server"></asp:TextBox>
            </p>
            <p>
                <asp:Button ID="btnAuthorize" runat="server" Text="Google Authorize" OnClick="btnAuthorize_Click" />
            </p>
        </div>
    </form>
</body>
</html>
