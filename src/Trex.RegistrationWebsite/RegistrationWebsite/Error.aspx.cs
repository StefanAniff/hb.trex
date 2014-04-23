namespace RegistrationWebsite
{
    public partial class Error : System.Web.UI.Page
    {
        protected void Page_Load()
        {
            var star = "<font color=\"red\"> *</font>";

            var cultureName = "en";

            if (Session["lng"] != null)
                cultureName = Session["lng"].ToString();


            Page.Culture = cultureName;
            Page.UICulture = cultureName;

            TitleLabel.Text = GetLocalResourceObject("TitleLabel").ToString();
            PageNotFoundErrorLabel.Text = GetLocalResourceObject("PageNotFoundErrorLabel").ToString();

        }
    }
}