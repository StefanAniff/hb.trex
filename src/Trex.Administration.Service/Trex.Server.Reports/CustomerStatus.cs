using System.Configuration;
using StructureMap;
using Trex.Server.Core.Services;
using Trex.Server.Infrastructure;

namespace Trex.Server.Reports
{
    using System;

    /// <summary>
    /// Summary description for CustomerStatus.
    /// </summary>
    public partial class CustomerStatus : Telerik.Reporting.Report
    {
        public CustomerStatus()
        {
            InitializeComponent();
            
            NeedDataSource += new EventHandler(CustomerStatus_NeedDataSource);
            DataSource = null;
        }

        void CustomerStatus_NeedDataSource(object sender, EventArgs e)
        {
            var customerId = this.ReportParameters["CustomerId"].Value.ToString();

            DynamicConnectionProvider.DynamicString = ConfigurationManager.ConnectionStrings["TrexBase"].ConnectionString;
           
            var clientRepository = ObjectFactory.GetInstance<IClientRepository>();
            var client = clientRepository.FindClientByCustomerId(customerId);
            
            this.CustomerStatusDB.ConnectionString = client.ConnectionString;
            this.UserDataSource.ConnectionString = client.ConnectionString;
            this.ProjectDataSource.ConnectionString = client.ConnectionString;
            this.CustomerDB.ConnectionString = client.ConnectionString;

            DataSource = CustomerStatusDB;
        }
    }
}