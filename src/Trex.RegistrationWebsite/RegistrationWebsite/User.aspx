<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="User.aspx.cs" Inherits="RegistrationWebsite.User" %>

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
                <asp:Label ID="TitleLabel" runat="server" /></h1>
            <asp:PlaceHolder runat="server" ID="registrationFormPlaceholder">
                <div class="contourField text name  mandatory">
                    <asp:Label ID="UserNameLabel" class="fieldLabel" runat="server" text="<%$ Resources:UserNameLabel %>" /><span class="mandatoryLabel"> *</span>
                    <div>
                        <asp:TextBox ID="UserNameTextBox" runat="server" class="text"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="UsernameRequireddValidator" runat="server" ControlToValidate="UserNameTextBox"
                        ErrorMessage="<%$ Resources:UsernameRequireddValidator %>" Display="None"></asp:RequiredFieldValidator>
                    <asp:ValidatorCalloutExtender ID="UsernameRequireddValidatorExtender" runat="server"
                        TargetControlID="UsernameRequireddValidator" HighlightCssClass="invalidfield">
                    </asp:ValidatorCalloutExtender>
                    
                </div>
                <div class="contourField text name  mandatory">
                    <asp:Label ID="PasswordLabel" class="fieldLabel" runat="server" Text="<%$ Resources:PasswordLabel %>" /><span class="mandatoryLabel"> *</span>
                    <div>
                        <asp:TextBox ID="PasswordTextBox" runat="server" class="text" TextMode="Password"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="PasswordRequiredValidator" runat="server" ErrorMessage="Error"
                        Display="None" ControlToValidate="PasswordTextBox"></asp:RequiredFieldValidator>
                    <asp:ValidatorCalloutExtender ID="PasswordValidatorExtender" runat="server" TargetControlID="PasswordRequiredValidator"
                        HighlightCssClass="invalidfield">
                    </asp:ValidatorCalloutExtender>
                    <asp:RegularExpressionValidator ID="PasswordExpressionValidator" runat="server" ErrorMessage="<%$ Resources:PasswordExpressionValidator %>"
                        ControlToValidate="PasswordTextBox" Display="None" ValidationExpression="[a-zA-z0-9]{6,}"></asp:RegularExpressionValidator>
                    <asp:ValidatorCalloutExtender ID="PasswordExpressionValidatorExtender" runat="server"
                        TargetControlID="PasswordExpressionValidator" HighlightCssClass="invalidfield">
                    </asp:ValidatorCalloutExtender>
                    
                </div>
                <div class="contourField text name  mandatory">
                    <asp:Label ID="ConfirmPasswordLabel" class="fieldLabel" runat="server" Text="<%$ Resources:ConfirmPasswordLabel %>" /><span class="mandatoryLabel"> *</span>
                    <div>
                        <asp:TextBox ID="PasswordConfirmTextBox" runat="server" class="text" TextMode="Password"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="PasswordConfirmRequiredValidator" runat="server"
                        ErrorMessage="<%$ Resources:PasswordConfirmRequiredValidator %>" Display="None" ControlToValidate="PasswordConfirmTextBox"></asp:RequiredFieldValidator>
                    <asp:ValidatorCalloutExtender ID="PasswordConfirmRequiredValidatorExtender" runat="server"
                        TargetControlID="PasswordConfirmRequiredValidator" HighlightCssClass="invalidfield">
                    </asp:ValidatorCalloutExtender>
                    <asp:CompareValidator ID="PasswordCompareValidator" runat="server" ErrorMessage="<%$ Resources:PasswordCompareValidator %>"
                        ControlToCompare="PasswordTextBox" ControlToValidate="PasswordConfirmTextBox"
                        Display="None"></asp:CompareValidator>
                    <asp:ValidatorCalloutExtender ID="PasswordCompareValidatorExtender" runat="server"
                        TargetControlID="PasswordCompareValidator" HighlightCssClass="invalidfield">
                    </asp:ValidatorCalloutExtender>
                    
                </div>
                <div class="contourField text name  mandatory">
                    <asp:Label ID="EmailLabel" class="fieldLabel" runat="server" Text="<%$ Resources:EmailLabel %>" /><span class="mandatoryLabel"> *</span>
                    <div>
                        <asp:TextBox ID="EmailTextBox" runat="server" class="text"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="EmailRequiredValidator" runat="server" ErrorMessage="<%$ Resources:EmailRequiredValidator %>"
                        Display="None" ControlToValidate="EmailTextBox"></asp:RequiredFieldValidator>
                    <asp:ValidatorCalloutExtender ID="EmailRequiredValidatorExtender" runat="server"
                        TargetControlID="EmailRequiredValidator" HighlightCssClass="invalidfield">
                    </asp:ValidatorCalloutExtender>
                    <asp:RegularExpressionValidator ID="EmailExpressionValidator" runat="server" ErrorMessage="<%$ Resources:EmailExpressionValidator %>"
                        ValidationExpression="^[\w-\.]{1,}\@([\da-zA-Z-]{1,}\.){1,}[\da-zA-Z-]{2,6}$"
                        Display="None" ControlToValidate="EmailTextBox"></asp:RegularExpressionValidator>
                    <asp:ValidatorCalloutExtender ID="EmailExpressionValidatorExtender" runat="server"
                        TargetControlID="EmailExpressionValidator" HighlightCssClass="invalidfield">
                    </asp:ValidatorCalloutExtender>
                    
                </div>
                <div class="contourField text name  mandatory">
                    <asp:Label ID="RepeatEmailLabel" class="fieldLabel" runat="server" Text="<%$ Resources:RepeatEmailLabel %>"/><span class="mandatoryLabel"> *</span>
                    <div>
                        <asp:TextBox ID="EmailConfirmTextBox" runat="server" class="text"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="EmailConfirmRequiredValidator" runat="server" ErrorMessage="<%$ Resources:EmailConfirmRequiredValidator %>"
                        Display="None" ControlToValidate="EmailConfirmTextBox"></asp:RequiredFieldValidator>
                    <asp:ValidatorCalloutExtender ID="EmailConfirmRequiredValidatoExtender" runat="server"
                        TargetControlID="EmailConfirmRequiredValidator" HighlightCssClass="invalidfield">
                    </asp:ValidatorCalloutExtender>
                    <asp:CompareValidator ID="EmailCompareValidator" runat="server" ErrorMessage="<%$ Resources:EmailCompareValidator %>"
                        ControlToCompare="EmailTextBox" Display="None" ControlToValidate="EmailConfirmTextBox"></asp:CompareValidator>
                    <asp:ValidatorCalloutExtender ID="EmailCompareValidatorExtender" runat="server" TargetControlID="EmailCompareValidator"
                        HighlightCssClass="invalidfield">
                    </asp:ValidatorCalloutExtender>
                  
                </div>
                <asp:Button ID="FinishButton" runat="server" OnClick="FinishClick" CssClass="button" text="<%$ Resources:FinishButtonText %>" />
                <br />
                <br />
                <asp:Label CssClass="errorbox" ID="ServiceErrorLabel" runat="server" Visible="False" Text="<%$ Resources:ServiceErrorLabel %>"></asp:Label>
                <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="UPanel"
                    DisplayAfter="100" DynamicLayout="true">
                    <ProgressTemplate>
                        <div class="wrapper">
                            <div class="wait">
                            </div>
                            <div class="wrapper2">
                                <div class="waitlabel">
                                    <img border="0" src="Images/loader.gif" />
                                    <asp:Label ID="PleaseWaitLabel" Text="Vent venligst / Please wait" runat="server" />
                                </div>
                            </div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                </div> </fieldset> </asp:PlaceHolder>
            <asp:PlaceHolder runat="server" ID="RecieptPlaceHolder" Visible="False">
                <div class="infoBox">
                      <asp:Label runat="server" ID="RecieptLabel" Text="<%$ Resources:UserRecieptLabel %>"></asp:Label>
                </div>
              
            </asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
