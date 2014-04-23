using System;
using System.ServiceModel;
using Telerik.ReportViewer.Silverlight;
using Telerik.Reporting.Service.SilverlightClient;

namespace Trex.Reports
{
    public class ReportService : IReportServiceClientFactory
    {
        #region IReportServiceClientFactory Members

        public ReportServiceClient Create(Uri remoteAddress)
        {
            var binding = new BasicHttpBinding
                              {
                                  MaxBufferSize = int.MaxValue,
                                  MaxReceivedMessageSize = int.MaxValue,
                                  ReceiveTimeout = new TimeSpan(0, 15, 0),
                                  SendTimeout = new TimeSpan(0, 15, 0),
                              };

            var endpointAddress = new EndpointAddress(remoteAddress);

            return new ReportServiceClient(binding, endpointAddress);
        }

        #endregion
    }
}