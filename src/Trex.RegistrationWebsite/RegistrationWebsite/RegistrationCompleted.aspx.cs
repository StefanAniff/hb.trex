using System;
using System.Resources;
using RegistrationWebsite.BaseClasses;

namespace RegistrationWebsite
{
    public partial class RegistrationCompleted : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           

           

            try
            {
                TitleLabel.Text = GetLocalResourceObject("TitleLabel").ToString();
                Content.Text = GetLocalResourceObject("Content").ToString();
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

            
        }

     
    }
}