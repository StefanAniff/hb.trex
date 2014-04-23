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
    public partial class InvoiceSpecification : Telerik.Reporting.Report
    {
        public InvoiceSpecification()
        {
            /// <summary>
            /// Required for telerik Reporting designer support
            /// </summary>
            InitializeComponent();

           
        }

        public InvoiceSpecification(int invoiceId, IAppSettings appSettings):this()
        {
            string sqlSelect = "spAggregatedTimeEntriesPrTaskPrDayPrInvoice";

            SqlConnection conn = new SqlConnection(appSettings.AppConnectionString);
            SqlCommand cmd = new SqlCommand(sqlSelect, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("invoiceId", invoiceId));

            SqlDataAdapter adpt = new SqlDataAdapter(cmd);

            DataSet dataSet = new DataSet();

            adpt.Fill(dataSet);

            this.DataSource = dataSet;
        
        }
    }
}