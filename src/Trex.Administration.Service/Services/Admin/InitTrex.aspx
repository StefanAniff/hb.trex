<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InitTrex.aspx.cs" Inherits="TrexSL.Web.Admin.InitTrex" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:CheckBoxList runat="server" ID="roleList">
        </asp:CheckBoxList>
        <div>
            User name<asp:TextBox runat="server" ID="txtUserName" /></div>
            <div>
            Full name<asp:TextBox runat="server" ID="txtFullName"></asp:TextBox>
            </div>
        <div>
            Email
            <asp:TextBox runat="server" ID="txtEmail" /></div>
        <div>
            Password
            <asp:TextBox runat="server" ID="txtPassword" /></div>
        <div>
            <asp:Button Text="Opret" runat="server" OnClick="CreateUserClick" />
        </div>
        <div>
            <asp:Literal  runat="server" ID="status" /></div>
    </div>
    </form>
</body>
</html>
