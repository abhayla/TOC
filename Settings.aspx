<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="TOC.Settings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:CheckBox ID="chkHide50MulSP" runat="server" Text="Hide strike price of 50 multiples" />
    </div>
    <div>
        <asp:Label ID="Label1" runat="server" Text="Settings for Watchlist"></asp:Label>
        <asp:CheckBox ID="chkHide1SDSP" runat="server" Text="Hide strike price within one standard deviation range, increases profit probability to 66%" />
        <asp:CheckBox ID="chkHide2SDSP" runat="server" Text="Hide strike price within two standard deviation range, increases profit probability to 95%" />
    </div>
    <div>
        <asp:Button runat="server" ID="btnSaveChanges" Text="Save Changes" OnClick="btnSaveChanges_Click" />
    </div>
</asp:Content>
