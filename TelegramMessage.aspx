<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TelegramMessage.aspx.cs" Inherits="TOC.TelegramMessage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="txtSendMsg" runat="server"></asp:TextBox>
            <br />
            <asp:Button ID="btnSendTelegramMessage" runat="server" Text="Send Telegram Message" OnClick="btnSendTelegramMessage_Click" />
            <br />
            <asp:TextBox ID="txtReceiveMsg" runat="server" Width="1047px"></asp:TextBox>
        </div>
    </form>
</body>
</html>
