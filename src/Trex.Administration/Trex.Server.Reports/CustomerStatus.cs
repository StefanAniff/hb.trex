using System.Configuration;

namespace Trex.Server.Reports
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;

    /// <summary>
    /// Summary description for CustomerStatus.
    /// </summary>
    public partial class CustomerStatus : Telerik.Reporting.Report
    {
        public CustomerStatus()
        {
   
            InitializeComponent();

           
            this.CustomerStatusDB.ConnectionString = ConfigurationManager.ConnectionStrings["TrexConnectionString"].ConnectionString;
            this.UserDataSource.ConnectionString = ConfigurationManager.ConnectionStrings["TrexConnectionString"].ConnectionString;
            this.ProjectDataSource.ConnectionString = ConfigurationManager.ConnectionStrings["TrexConnectionString"].ConnectionString;
            this.CustomerDB.ConnectionString = ConfigurationManager.ConnectionStrings["TrexConnectionString"].ConnectionString;
            
        }
    }
}