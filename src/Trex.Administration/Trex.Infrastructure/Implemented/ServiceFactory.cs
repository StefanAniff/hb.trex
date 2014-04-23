using System.ServiceModel;
using System.ServiceModel.Channels;
using Trex.Core.Exceptions;
using Trex.Core.Services;
using Trex.Infrastructure.CustomBehavior;
using IFileDownloadService = Trex.ServiceModel.FileDownloadService.IFileDownloadService;
using ITrexService = Trex.ServiceModel.Model.ITrexService;

namespace Trex.Infrastructure.Implemented
{
    internal static class ServiceFactory
    {
        private static string _environment;

        /// <summary>
        /// Indicates the environment app is running in.
        /// Initially passed from app arguments
        /// </summary>
        public static string Environment
        {
            get { return _environment; }
            set
            {
                if (_environment != null)
                    throw new ApplicationBaseException("Environment should only be set once");
                _environment = value;
            }
        }

        public static ITrexService GetServiceClient(ILoginSettings loginSettings)
        {
            var binding = new CustomBinding();
            binding.Elements.Add(new BinaryMessageEncodingBindingElement());
            var httpTransport = new HttpTransportBindingElement
                {
                    MaxBufferSize = int.MaxValue,
                    MaxReceivedMessageSize = int.MaxValue
                };
            binding.Elements.Add(httpTransport);

            var channel = GetDomainChannel();

            if (channel.Endpoint.Behaviors.Find<UserCredentialsEndpointBehavior>() == null && loginSettings != null)
            {
                var customBehavior = new UserCredentialsEndpointBehavior(loginSettings.UserName, loginSettings.Password, loginSettings.CustomerId);
                channel.Endpoint.Behaviors.Add(customBehavior);
            }

            return channel.CreateChannel();

        }

        private static ChannelFactory<ITrexService> GetDomainChannel()
        {
            var channel = new ChannelFactory<ITrexService>(string.Format("{0}{1}", Environment, "_DomainServiceEndpoint"));
            return channel;
        }

        public static string DomainChannelUrl
        {
            get { return GetDomainChannel().Endpoint.Address.Uri.AbsoluteUri; }
        }

        public static IFileDownloadService GetFileDownloadClient(ILoginSettings loginSettings)
        {
            var channel = GetFileDownloadChannel();

            if (channel.Endpoint.Behaviors.Find<UserCredentialsEndpointBehavior>() == null && loginSettings != null)
            {
                var customBehavior = new UserCredentialsEndpointBehavior(loginSettings.UserName, loginSettings.Password, loginSettings.CustomerId);
                channel.Endpoint.Behaviors.Add(customBehavior);
            }

            return channel.CreateChannel();
        }

        public static ChannelFactory<IFileDownloadService> GetFileDownloadChannel()
        {
            var channel = new ChannelFactory<IFileDownloadService>(string.Format("{0}{1}", Environment, "_FileServiceEndpoint"));
            return channel;
        }        

        public static string FileDownloadChannelUrl
        {
            get { return GetFileDownloadChannel().Endpoint.Address.Uri.AbsoluteUri; }
        }
    }
}