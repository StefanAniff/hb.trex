using System;
using System.Resources;
using System.ServiceModel;
using D60.Toolkit.LogUtils;
using RegistrationWebsite.BaseClasses;

namespace RegistrationWebsite
{
    public partial class User : PageBase
    {
        private string _customerId;
        private string _fullName;

        protected void Page_Load(object sender, EventArgs e)
        {

            //#region language

            //var star = "<font color=\"red\"> *</font>";



            //RegistrationInfoLabel.Text = GetLocalResourceObject("RegistrationInfoLabel").ToString();
            //TitleLabel.Text = GetLocalResourceObject("TitleLabel").ToString();
            //UserNameLabel.Text = GetLocalResourceObject("UserNameLabel").ToString() + star;
            //PasswordLabel.Text = GetLocalResourceObject("PasswordLabel").ToString() + star;
            //ConfirmPasswordLabel.Text = GetLocalResourceObject("ConfirmPasswordLabel").ToString() + star;
            //EmailLabel.Text = GetLocalResourceObject("EmailLabel").ToString() + star;
            //RepeatEmailLabel.Text = GetLocalResourceObject("RepeatEmailLabel").ToString() + star;
            //FinishButton.Text = GetLocalResourceObject("FinishButtonText").ToString();
            //ServiceErrorLabel.Text = GetLocalResourceObject("ServiceErrorLabel").ToString();

            //UsernameRequireddValidator.ErrorMessage = GetLocalResourceObject("UsernameRequireddValidator").ToString();
            //UserNameExpressionValidator.ErrorMessage = GetLocalResourceObject("UserNameExpressionValidator").ToString();
            //PasswordRequiredValidator.ErrorMessage = GetLocalResourceObject("PasswordRequiredValidator").ToString();
            //PasswordExpressionValidator.ErrorMessage = GetLocalResourceObject("PasswordExpressionValidator").ToString();
            //PasswordConfirmRequiredValidator.ErrorMessage = GetLocalResourceObject("PasswordConfirmRequiredValidator").ToString();
            //PasswordCompareValidator.ErrorMessage = GetLocalResourceObject("PasswordCompareValidator").ToString();
            //EmailRequiredValidator.ErrorMessage = GetLocalResourceObject("EmailRequiredValidator").ToString();
            //EmailExpressionValidator.ErrorMessage = GetLocalResourceObject("EmailExpressionValidator").ToString();
            //EmailConfirmRequiredValidator.ErrorMessage = GetLocalResourceObject("EmailConfirmRequiredValidator").ToString();
            //EmailCompareValidator.ErrorMessage = GetLocalResourceObject("EmailCompareValidator").ToString();



            //#endregion

            _customerId = (string)Session["customerId"];
            _fullName = (string)Session["fullName"];

        }

        private void Reset()
        {
            ServiceErrorLabel.Visible = false;
        }

        protected void FinishClick(object sender, EventArgs e)
        {

            Reset();

            if (!Page.IsValid)
                return;

            AppendCustomerDetails();
            CreateAdministrator();

        }

       

        private void AppendCustomerDetails()
        {
            RegistrationService.CustomerServiceClient rs = null;



            try
            {
                rs = ServiceAgent.GetServiceClient();
                var customer = rs.GetCustomerByCustomerId(_customerId);


                customer.CreatorUserName = UserNameTextBox.Text;
                customer.CreatorEmail = EmailConfirmTextBox.Text;
                
                rs.SaveCustomer(customer);

            }
            catch (EndpointNotFoundException exception)
            {
                OnError(exception);

                ServiceErrorLabel.Visible = true;
            }
            catch (TimeoutException exception)
            {
                OnError(exception);

                ServiceErrorLabel.Visible = true;
            }
          
            finally
            {
                if (rs != null) rs.Close();
            }


        }

        private void CreateAdministrator()
        {
            RegistrationService.CustomerServiceClient rs = null;


            try
            {
                rs = ServiceAgent.GetServiceClient();
                var response = rs.RegisterNewUser(_customerId, UserNameTextBox.Text, PasswordTextBox.Text, _fullName, EmailTextBox.Text,CurrentCulture.TwoLetterISOLanguageName);

                if (response.Success)
                {
                    registrationFormPlaceholder.Visible = false;
                    RecieptPlaceHolder.Visible = true;
                }
                else
                {
                    ServiceErrorLabel.Visible = true;
                    ServiceErrorLabel.Text = response.Response;
                }
            }

            catch (EndpointNotFoundException exception)
            {
                OnError(exception);
                ServiceErrorLabel.Text = (string)System.Web.HttpContext.GetLocalResourceObject("~/Activation.aspx", "User_CreateAdministrator_Could_not_contact_service_at_the_moment__Please_try_again_later");
                ServiceErrorLabel.Visible = true;
            }
            catch (TimeoutException exception)
            {
                OnError(exception);
                ServiceErrorLabel.Text = (string)System.Web.HttpContext.GetLocalResourceObject("~/Activation.aspx", "User_CreateAdministrator_Could_not_contact_service_at_the_moment__Please_try_again_later");
                ServiceErrorLabel.Visible = true;
            }
           
            finally
            {
                if (rs != null) rs.Close();
            }


        }

       

    }
}