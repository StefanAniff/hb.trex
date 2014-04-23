<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Finalize.aspx.cs" Inherits="RegistrationWebsite.Finalize" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        <asp:Label ID="TitleLabel" runat="server" /></h1>
    <p>
        &nbsp;</p>
    <div class="top">
    </div>
    <div class="fieldset">
        <br />
        <div id="messagebox">
            <asp:Label ID="StatusLabel" runat="server" Text=""></asp:Label>
        </div>
    </div>
</asp:Content>
