using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Trex.Infrastructure.CustomBehavior
{
    public class MessageInspector : IClientMessageInspector
    {
        private readonly string _customerId;
        private readonly string _userName;
        private readonly string _userPassword;

        public MessageInspector(string userName, string userPassword, string customerId)
        {
            _userName = userName;
            _userPassword = userPassword;
            _customerId = customerId;
        }

        #region IClientMessageInspector Members

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var nameHeader = MessageHeader.CreateHeader("UserName", "Authentication", _userName);
            var passwordHeader = MessageHeader.CreateHeader("UserPassword", "Authentication", _userPassword);
            var idHeader = MessageHeader.CreateHeader("CustomerId", "Authentication", _customerId);
            var clientApplicationTypeHeader = MessageHeader.CreateHeader("ClientApplicationType", "Authentication", 1);

            if (request.Headers.FindHeader("UserName", "Authentication") <= 0)
            {
                request.Headers.Add(nameHeader);
            }

            if (request.Headers.FindHeader("UserPassword", "Authentication") <= 0)
            {
                request.Headers.Add(passwordHeader);
            }

            if (request.Headers.FindHeader("CustomerId", "Authentication") <= 0)
            {
                request.Headers.Add(idHeader);
            }

            if (request.Headers.FindHeader("ClientApplicationType", "Authentication") <= 0)
            {
                request.Headers.Add(clientApplicationTypeHeader);
            }

            return null;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState) {}

        #endregion
    }
}