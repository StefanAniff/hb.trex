using System;
using System.Resources;
using System.ServiceModel;
using System.Web.UI.WebControls;
using System.Xml;
using RegistrationWebsite.BaseClasses;
using RegistrationWebsite.Helpers;
using RegistrationWebsite.RegistrationService;

namespace RegistrationWebsite
{
    public partial class Default : PageBase
    {



        protected void Page_Load(object sender, EventArgs e)
        {
            #region language

           // const string star = "<font color=\"red\"> *</font>";


            //TitleLabel.Text = GetLocalResourceObject("TitleLabel").ToString();
            //RegistrationInfoLabel.Text = GetLocalResourceObject("RegistrationInfoLabel").ToString();
            //NameLabel.Text = GetLocalResourceObject("NameLabel").ToString() + star;
            //PhoneNumberLabel.Text = GetLocalResourceObject("PhoneNumberLabel").ToString() + star;
            //CompanyNameLabel.Text = GetLocalResourceObject("CompanyNameLabel").ToString() + star;
            ////   VatNumberLabel.Text = GetLocalResourceObject("VatNumberLabel").ToString() + star;
            //CountryLabel.Text = GetLocalResourceObject("CountryLabel").ToString() + star;
            //Address1Label.Text = GetLocalResourceObject("Address1Label").ToString() + star;
            //Address2Label.Text = GetLocalResourceObject("Address2Label").ToString();
            ////Address3Label.Text = GetLocalResourceObject("Address3Label").ToString();
            ////Address4Label.Text = GetLocalResourceObject("Address4Label").ToString();
            ////Address5Label.Text = GetLocalResourceObject("Address5Label").ToString();
            //CustomerIdLabel.Text = GetLocalResourceObject("CustomerIdLabel").ToString() + star;
            //CustomerIdInfoLabel.Text = GetLocalResourceObject("CustomerIdInfoLabel").ToString();
            ////VerificationInfoLabel.Text = GetLocalResourceObject("VerificationInfoLabel").ToString();
            ////VerificationLabel.Text = GetLocalResourceObject("VerificationLabel").ToString();
            //ContinueButton.Text = GetLocalResourceObject("ContinueButtonText").ToString();
            //ServiceErrorLabel.Text = GetLocalResourceObject("ServiceErrorlabel").ToString();
            ////RefreshButton.Text = GetLocalResourceObject("RefreshButton").ToString();
            ////validation resources
            //FullNameRequiredValidator.ErrorMessage = GetLocalResourceObject("FullNameRequiredValidator").ToString();
            //PhoneNumberRequiredValidator.ErrorMessage = GetLocalResourceObject("PhoneNumberRequiredValidator").ToString();
            //PhoneNumberExpressionValidator.ErrorMessage = GetLocalResourceObject("PhoneNumberExpressionValidator").ToString();
            //CompanyNameRequiredValidator.ErrorMessage = GetLocalResourceObject("CompanyNameRequiredValidator").ToString();
            ////VatNumberRequiredValidator.ErrorMessage = GetLocalResourceObject("VatNumberRequiredValidator").ToString();
            //CountryRequiredValidator.ErrorMessage = GetLocalResourceObject("CountryRequiredValidator").ToString();
            //Address1RequiredValidator.ErrorMessage = GetLocalResourceObject("Address1RequiredValidator").ToString();
            //ApplicationNameRequiredValidator.ErrorMessage = GetLocalResourceObject("ApplicationNameRequiredValidator").ToString();
            //ApplicationNameExpressionValidator.ErrorMessage = GetLocalResourceObject("ApplicationNameExpressionValidator").ToString();
            //ApplicationNameAvailableValidator.ErrorMessage = GetLocalResourceObject("ApplicationNameAvailableValidator").ToString();
            //CatchValidator.ErrorMessage = GetLocalResourceObject("CatchValidator").ToString();
            //CatchRequiredValidator.ErrorMessage = GetLocalResourceObject("CatchRequiredValidator").ToString();




            #endregion


            if (!IsPostBack)
            {
                BindCountries();
                //SetCaptcha();
            }

        }


        protected void IsNameTaken(object sender, ServerValidateEventArgs e)
        {
            Reset();

            if (CheckCustomerId())
                e.IsValid = false;
            else
                e.IsValid = true;
        }




        protected void ExecuteRegistration(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            CreateCustomer();

            Session["customerId"] = CustomerId;
            Session["fullName"] = FullName;
            Response.Redirect("User.aspx");
            //Server.Transfer("User.aspx");

        }


        private void CreateCustomer()
        {
            Reset();

            ServiceErrorLabel.Visible = false;
            CustomerServiceClient rs = null;


            try
            {
                rs = ServiceAgent.GetServiceClient();

                var trexCustomer = new TrexCustomer();
                trexCustomer.CompanyName = CompanyNameTextBox.Text;
                trexCustomer.Country = CountriesDropDownList.SelectedValue;
                trexCustomer.Address1 = Address1TextBox.Text;
                //trexCustomer.Address2 = Address2TextBox.Text;
                trexCustomer.Zipcode = ZipCodeTextBox.Text;
                trexCustomer.City = cityTextBox.Text;
                trexCustomer.CreatorFullName = FullNameTextBox.Text;
                trexCustomer.CreatorPhone = PhoneNumberTextBox.Text;
                trexCustomer.CustomerId = ApplicationNameTextBox.Text;
                trexCustomer.CreateDate = DateTime.Now;
                trexCustomer.VatNumber = string.Empty;


                rs.SaveCustomer(trexCustomer);

                Session["customerId"] = ApplicationNameTextBox.Text;
                Session["fullname"] = FullNameTextBox.Text;
                //Session["source"] = "stepone";
                //Session["guid"] = Guid.NewGuid().ToString();



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


        private void Reset()
        {
            ApplicationNameAvailableValidator.ErrorMessage = GetLocalResourceObject("ApplicationNameAvailableValidator").ToString();
            ServiceErrorLabel.Visible = false;
        }


        private bool CheckCustomerId()
        {
            var exists = false;

            CustomerServiceClient rs = null;

            try
            {
                rs = ServiceAgent.GetServiceClient();

                if (rs.ExistsApplicationName(ApplicationNameTextBox.Text))
                    exists = true;


            }
            catch (EndpointNotFoundException exception)
            {
                OnError(exception);
                ApplicationNameAvailableValidator.ErrorMessage = GetLocalResourceObject("ApplicationNameUnableToValidate").ToString();
                ServiceErrorLabel.Visible = true;
            }
            catch (TimeoutException exception)
            {
                OnError(exception);
                ApplicationNameAvailableValidator.ErrorMessage = GetLocalResourceObject("ApplicationNameUnableToValidate").ToString();
                ServiceErrorLabel.Visible = true;
            }

            finally
            {
                if (rs != null) rs.Close();
            }

            return exists;
        }


        public string CustomerId { get { return ApplicationNameTextBox.Text; } }
        public string FullName { get { return FullNameTextBox.Text; } }

        private void BindCountries()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Server.MapPath("countries.xml"));

            foreach (XmlNode node in doc.SelectNodes("//country"))
            {
                CountriesDropDownList.Items.Add(new ListItem(node.InnerText, node.Attributes["code"].InnerText));
            }

        }


     

    }
}