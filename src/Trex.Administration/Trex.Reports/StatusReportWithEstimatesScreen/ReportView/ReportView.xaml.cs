using System;
using System.Windows.Controls;
using Trex.Core.Interfaces;

namespace Trex.Reports.StatusReportWithEstimatesScreen.ReportView
{
    public partial class ReportView : UserControl, IView
    {
        public ReportView()
        {
            InitializeComponent();

            ReportViewer.ReportServiceClientFactory = new ReportService();
        }

        #region IView Members

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        #endregion
    }
}