using System.Windows.Controls;
using Telerik.ReportViewer.Silverlight;
using Trex.Core.Interfaces;

namespace Trex.Reports.StatusReportScreen.StatusReportView
{
    public partial class StatusReportView : UserControl, IView
    {
        public StatusReportView()
        {
            InitializeComponent();

            ReportViewer.ReportServiceClientFactory = new ReportService();

            ReportViewer.RenderBegin += ReportViewer_RenderBegin;
        }

        #region IView Members

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        #endregion

        private void ReportViewer_RenderBegin(object sender, RenderBeginEventArgs args)
        {
            var loginSettings = ((StatusReportViewModel) DataContext).LoginSettings;

            args.ParameterValues["CustomerId"] = loginSettings.CustomerId;
        }
    }
}