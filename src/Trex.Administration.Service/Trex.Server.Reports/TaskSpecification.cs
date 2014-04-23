using System;
using Trex.Server.Core.Model;
using Trex.Server.Core.Services;

namespace Trex.Server.Reports
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Data.SqlClient;
    using System.Data;
    using Server.Core.Services;

    /// <summary>
    /// Summary description for InvoiceSpecification.
    /// </summary>
    public partial class TaskSpecification : Telerik.Reporting.Report
    {
        public TaskSpecification()
        {
            /// <summary>
            /// Required for telerik Reporting designer support
            /// </summary>
            InitializeComponent();


        }

        public TaskSpecification(int customerId,DateTime startDate, DateTime endDate, bool billable, IAppSettings appSettings)
            : this()
        {
            var sqlSelect = string.Empty;

            sqlSelect = "spAggregatedTimeEntriesPrTaskPrDay";

            var conn = new SqlConnection(appSettings.AppConnectionString);
            var cmd = new SqlCommand(sqlSelect, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("customerId", customerId));
            cmd.Parameters.Add(new SqlParameter("startDate", startDate));
            cmd.Parameters.Add(new SqlParameter("endDate", endDate));
            cmd.Parameters.Add(new SqlParameter("billable", billable));

            SqlDataAdapter adpt = new SqlDataAdapter(cmd);

            DataSet dataSet = new DataSet();

            adpt.Fill(dataSet);

            this.DataSource = dataSet;
            this.ReportParameters["BillableTime"].Value = billable;
            this.ReportParameters["PeriodStart"].Value = startDate;
            this.ReportParameters["PeriodEnd"].Value = endDate;
        
        }
    }
}