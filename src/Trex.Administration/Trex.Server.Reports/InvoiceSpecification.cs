using System.Data;
using System.Data.SqlClient;
using Telerik.Reporting;
using Trex.Server.Core.Services;

namespace Trex.Server.Reports
{
    /// <summary>
    /// Summary description for InvoiceSpecification.
    /// </summary>
    public partial class InvoiceSpecification : Report
    {
        public InvoiceSpecification()
        {
            /// <summary>
            /// Required for telerik Reporting designer support
            /// </summary>
            InitializeComponent();
        }

        public InvoiceSpecification(int invoiceId, IAppSettings appSettings) : this()
        {
            var sqlSelect = "spAggregatedTimeEntriesPrTaskPrDayPrInvoice";

            var conn = new SqlConnection(appSettings.AppConnectionString);
            var cmd = new SqlCommand(sqlSelect, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("invoiceId", invoiceId));

            var adpt = new SqlDataAdapter(cmd);

            var dataSet = new DataSet();

            adpt.Fill(dataSet);

            DataSource = dataSet;
        }
    }
}