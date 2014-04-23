using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.ServiceModel;

using RegistrationWebsite.BaseClasses;

namespace RegistrationWebsite
{
    public partial class Activation : PageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            var id = Request.QueryString["ID"];
            if (id == null)
            {
                StatusLabel.Text = GetLocalResourceObject("FailureMessageMissingId").ToString();
                OnError(new MissingFieldException("Missing id in the query string"));
            }

            else
                Activate(id);


        }


        private void Activate(string activationId)
        {

            RegistrationService.CustomerServiceClient rs = null;

            try
            {
                rs = new RegistrationService.CustomerServiceClient();

                rs.ActivateCustomer(activationId, CurrentCulture.TwoLetterISOLanguageName);

                if (CurrentCulture.TwoLetterISOLanguageName == "da")
                    recieptLabel.Text = GetContentFromUrl(ConfigurationManager.AppSettings["recieptTextUrl_da"]);
                else
                {
                    recieptLabel.Text = GetContentFromUrl(ConfigurationManager.AppSettings["recieptTextUrl_en"]);
                }

            }
            catch (EndpointNotFoundException exception)
            {
                OnError(exception);
                StatusLabel.Text = GetLocalResourceObject("FailureMessage").ToString();

            }
            catch (TimeoutException exception)
            {
                OnError(exception);
                StatusLabel.Text = GetLocalResourceObject("FailureMessage").ToString();
            }
            finally
            {
                if (rs != null) rs.Close();
            }


        }


        protected string GetContentFromUrl(string url)
        {

            var request = (HttpWebRequest)WebRequest.Create(url);
            var response = (HttpWebResponse)request.GetResponse();

            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream == null)
                    throw new ArgumentException("Could not obtain the download email template. Url: " + url, "url");


                var resultString = string.Empty;

                using (var streamReader = new StreamReader(responseStream))
                {
                    resultString = streamReader.ReadToEnd();


                }
                return resultString;

            }
        }




    }
}