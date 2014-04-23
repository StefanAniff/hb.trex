using System;
using System.Resources;
using System.ServiceModel;
using D60.Toolkit.LogUtils;

namespace RegistrationWebsite
{
    public partial class Finalize : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            #region language

            var cultureName = "en";

            if (Session["lng"] != null)
                cultureName = Session["lng"].ToString();


            try
            {
                StatusLabel.Text = GetLocalResourceObject("FailureMessage").ToString();
            }
            catch (NullReferenceException exception)
            {
                OnError(exception);
                Response.Redirect("Critical.aspx");
            }
            catch (MissingManifestResourceException exception)
            {
                OnError(exception);
                Response.Redirect("Critical.aspx");
            }
            catch (Exception exception)
            {
                OnError(exception);
                throw;
            }



            Page.Culture = cultureName;
            Page.UICulture = cultureName;

            #endregion

            if (!IsPostBack)
                finalize();

        }

        private void finalize()
        {
            RegistrationService.CustomerServiceClient rs = null;
            var hasErrors = true;

            try
            {
                rs = ServiceAgent.GetServiceClient();

                var activationId = Guid.NewGuid().ToString();

                if (rs.A(activationId, (string)Session["app"]))
                    hasErrors = false;
            }
            catch (EndpointNotFoundException exception)
            {
                OnError(exception);
                hasErrors = true;
            }
            catch (TimeoutException exception)
            {
                OnError(exception);
                hasErrors = true;
            }
            catch (Exception exception)
            {
                OnError(exception);
                throw;
            }
            finally
            {
                if (rs != null) rs.Close();

                if (hasErrors)
                {
                    StatusLabel.Text = GetLocalResourceObject("FailureMessage").ToString();
                    TitleLabel.Text = GetLocalResourceObject("FailureTitleLabel").ToString();
                }
                else
                {
                    StatusLabel.Text = GetLocalResourceObject("SuccessMessage").ToString();
                    TitleLabel.Text = GetLocalResourceObject("SuccessTitleLabel").ToString();
                }
                Session.Clear();
            }
        }

        private void OnError(Exception ex)
        {
            var logger = new Logger();
            logger.LogError(ex);
        }

    }
}