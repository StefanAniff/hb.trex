using System;
using System.Web.Security;
using System.Web.UI;

namespace TrexSL.Web.MasterPages
{
    public partial class TimeRegMain : MasterPage
    {
        public bool DisplayPageTitle
        {
            get { return pageTitleDiv.Visible; }
            set { pageTitleDiv.Visible = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            pageTitle.Text = Page.Title;
        }

        public void ShowMessageBox(string title, string message)
        {
            //MessageBox1.Title = title;
            //MessageBox1.Text = message;
            //MessageBox1.Show();
        }

        protected void LogOutBtn_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("Default.aspx");
        }
    }
}