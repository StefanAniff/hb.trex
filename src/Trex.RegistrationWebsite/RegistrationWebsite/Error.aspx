<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Error.aspx.cs" Inherits="RegistrationWebsite.Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        <asp:Label ID="TitleLabel" runat="server" /></h1>
    <p>
        &nbsp;</p>
    <div class="top">
    </div>
    <div id="messagebox">
        <asp:Label ID="PageNotFoundErrorLabel" runat="server" Text="Page does not exist!"></asp:Label>
    </div>
</asp:Content>
