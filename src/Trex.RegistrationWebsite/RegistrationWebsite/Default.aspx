<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="RegistrationWebsite.Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager EnablePageMethods="true" ID="MainSM" runat="server" ScriptMode="Release"
        LoadScriptsBeforeUI="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UPanel" runat="server">
        <ContentTemplate>
            <h1>
                <asp:Label ID="TitleLabel" runat="server" Text="<%$ Resources:TitleLabel %>" /></h1>
            <div>
                <asp:Label ID="NameLabel" CssClass="fieldLabel" runat="server" Text="<%$ Resources:NameLabel %>" /><span
                    class="mandatoryLabel"> *</span>
                <div>
                    <asp:TextBox ID="FullNameTextBox" runat="server" class="text"></asp:TextBox>
                </div>
                <asp:RequiredFieldValidator ID="FullNameRequiredValidator" runat="server" ControlToValidate="FullNameTextBox"
                    Display="None" ErrorMessage="<%$ Resources:FullNameRequiredValidator %>"><span class="mandatoryLabel"> *</span>
                </asp:RequiredFieldValidator>
                <asp:ValidatorCalloutExtender ID="FullNameRequiredValidatorExtender" runat="server"
                    TargetControlID="FullNameRequiredValidator" HighlightCssClass="invalidfield">
                </asp:ValidatorCalloutExtender>
            </div>
            <div class="contourField text name  mandatory">
                <asp:Label ID="PhoneNumberLabel" class="fieldLabel" runat="server" Text="<%$ Resources:PhoneNumberLabel %>" /><span
                    class="mandatoryLabel"> *</span>
                <div>
                    <asp:TextBox ID="PhoneNumberTextBox" runat="server" class="text"></asp:TextBox>
                </div>
                <asp:RequiredFieldValidator ID="PhoneNumberRequiredValidator" runat="server" ControlToValidate="PhoneNumberTextBox"
                    Display="None" ErrorMessage="<%$ Resources:PhoneNumberRequiredValidator %>">
                </asp:RequiredFieldValidator>
                <asp:ValidatorCalloutExtender ID="PhoneNumberRequiredValidatorExtender" runat="server"
                    TargetControlID="PhoneNumberRequiredValidator" HighlightCssClass="invalidfield">
                </asp:ValidatorCalloutExtender>
                <asp:RegularExpressionValidator ID="PhoneNumberExpressionValidator" runat="server"
                    ControlToValidate="PhoneNumberTextBox" Display="None" ErrorMessage="<%$ Resources:PhoneNumberExpressionValidator %>"
                    ValidationExpression="[0-9]{5,}">
                </asp:RegularExpressionValidator>
                <asp:ValidatorCalloutExtender ID="PhoneNumberExpressionValidatorExtender" runat="server"
                    HighlightCssClass="invalidfield" TargetControlID="PhoneNumberExpressionValidator">
                </asp:ValidatorCalloutExtender>
            </div>
            <div class="contourField text name  mandatory">
                <asp:Label ID="CompanyNameLabel" class="fieldLabel" runat="server" Text="<%$ Resources:CompanyNameLabel %>" /><span
                    class="mandatoryLabel"> *</span>
                <div>
                    <asp:TextBox ID="CompanyNameTextBox" runat="server" class="text"></asp:TextBox>
                </div>
                <asp:RequiredFieldValidator ID="CompanyNameRequiredValidator" runat="server" ControlToValidate="CompanyNameTextBox"
                    Display="None" ErrorMessage="<%$ Resources:CompanyNameRequiredValidator %>">
                </asp:RequiredFieldValidator>
                <asp:ValidatorCalloutExtender ID="CompanyNameRequiredValidatorExtender" runat="server"
                    TargetControlID="CompanyNameRequiredValidator" HighlightCssClass="invalidfield">
                </asp:ValidatorCalloutExtender>
            </div>
            <div class="contourField text name  mandatory">
                <asp:Label ID="CountryLabel" class="fieldLabel" runat="server" Text="<%$ Resources:CountryLabel %>" /><span
                    class="mandatoryLabel"> *</span>
                <div>
                    <asp:DropDownList ID="CountriesDropDownList" runat="server" class="dropdown">
                    </asp:DropDownList>
                </div>
                <asp:RequiredFieldValidator ID="CountryRequiredValidator" runat="server" ControlToValidate="CountriesDropDownList"
                    Display="None" ErrorMessage="<%$ Resources:CountryRequiredValidator %>">
                </asp:RequiredFieldValidator>
                <asp:ValidatorCalloutExtender ID="CountryRequiredValidatorExtender" runat="server"
                    HighlightCssClass="invalidfield" TargetControlID="CountryRequiredValidator">
                </asp:ValidatorCalloutExtender>
            </div>
            <div class="contourField text name  mandatory">
                <asp:Label ID="Address1Label" class="fieldLabel" runat="server" Text="<%$ Resources:Address1Label %>" /><span
                    class="mandatoryLabel"> *</span>
                <div>
                    <asp:TextBox ID="Address1TextBox" runat="server" class="text"></asp:TextBox>
                </div>
                <asp:RequiredFieldValidator ID="Address1RequiredValidator" runat="server" ControlToValidate="Address1TextBox"
                    Display="None" ErrorMessage="<%$ Resources:Address1RequiredValidator %>"></asp:RequiredFieldValidator>
                <asp:ValidatorCalloutExtender ID="Address1RequiredValidatorExtender" runat="server"
                    HighlightCssClass="invalidfield" TargetControlID="Address1RequiredValidator">
                </asp:ValidatorCalloutExtender>
            </div>
            <div class="contourField text name  mandatory">
                <div style="float: left">
                    <asp:Label ID="ZipcodeLabel" class="fieldLabel" runat="server" Text="<%$ Resources:ZipcodeLabel %>" /><span
                        class="mandatoryLabel"> *</span>
                    <div>
                        <asp:TextBox ID="ZipCodeTextBox" runat="server" class="ziptext"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="ZipcodeRequiredValidator" runat="server" ControlToValidate="ZipCodeTextBox"
                            Display="None" ErrorMessage="<%$ Resources:ZipcodeRequiredValidator %>"></asp:RequiredFieldValidator>
                        <asp:ValidatorCalloutExtender ID="ZipcodeRequiredExtender" runat="server" HighlightCssClass="invalidfield"
                                                      TargetControlID="ZipcodeRequiredValidator"/>
                    </div>
                </div>
                <div style="float: left; margin-left: 10px;">
                    <asp:Label ID="CityLabel" class="fieldLabel" runat="server" Text="<%$ Resources:CityLabel %>" /><span
                        class="mandatoryLabel"> *</span>
                    <div>
                        <asp:TextBox ID="cityTextBox" runat="server" class="citytext"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="CityRequiredValidator" runat="server" ControlToValidate="cityTextBox"
                            Display="None" ErrorMessage="<%$ Resources:CityRequiredValidator %>"></asp:RequiredFieldValidator>
                        <asp:ValidatorCalloutExtender ID="CityCalloutExtender" runat="server" HighlightCssClass="invalidfield"
                                                      TargetControlID="CityRequiredValidator"/>
                    </div>
                </div>
                <div style="clear: both">
                </div>
            </div>
            <div class="contourField text name  mandatory">
                <asp:Label ID="CustomerIdLabel" class="fieldLabel" runat="server" Text="<%$ Resources:CustomerIdLabel %>" /><span
                    class="mandatoryLabel"> *</span>
                <div>
                    <asp:TextBox ID="ApplicationNameTextBox" runat="server" class="text"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="ApplicationNameRequiredValidator" runat="server"
                        ControlToValidate="ApplicationNameTextBox" Display="None" ErrorMessage="<%$ Resources:ApplicationNameRequiredValidator %>">
                    </asp:RequiredFieldValidator>
                    <asp:ValidatorCalloutExtender ID="ApplicationNameRequiredValidatorExtender" runat="server"
                        HighlightCssClass="invalidfield" TargetControlID="ApplicationNameRequiredValidator">
                    </asp:ValidatorCalloutExtender>
                    <asp:CustomValidator ID="ApplicationNameAvailableValidator" runat="server" ControlToValidate="ApplicationNameTextBox"
                        Display="None" OnServerValidate="IsNameTaken" ErrorMessage="<%$ Resources:ApplicationNameAvailableValidator %>">
                    </asp:CustomValidator>
                    <asp:ValidatorCalloutExtender ID="ApplicationNameAvailableValidatorExtender" runat="server"
                        TargetControlID="ApplicationNameAvailableValidator" HighlightCssClass="invalidfield">
                    </asp:ValidatorCalloutExtender>
                </div>
                <div style="padding-top: 5px; padding-bottom: 5px;">
                    <asp:Label ID="CustomerIdInfoLabel" runat="server" Text="<%$ Resources:CustomerIdInfoLabel %>">></asp:Label>
                </div>
            </div>
            <div class="contourField text name  mandatory">
                <asp:Label CssClass="errorbox" ID="ServiceErrorLabel" runat="server" Text="<%$ Resources:ServiceErrorLabel %>"
                    Visible="False"></asp:Label>
                <asp:Button ID="ContinueButton" runat="server" OnClick="ExecuteRegistration" Class="button"
                    Text="<%$ Resources:ContinueButtonText %>" />
                <div style="height: 35px; padding-top: 5px; padding-bottom: 5px">
                    <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="UPanel"
                        DisplayAfter="100" DynamicLayout="true">
                        <ProgressTemplate>
                            <div class="wrapper">
                                <div class="wait">
                                </div>
                                <div class="wrapper2">
                                    <div class="waitlabel">
                                        <img border="0" src="Images/loader.gif" />
                                        <asp:Label ID="PleaseWaitLabel" runat="server" Text="<%$ Resources:PleaseWait %>" />
                                    </div>
                                </div>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
