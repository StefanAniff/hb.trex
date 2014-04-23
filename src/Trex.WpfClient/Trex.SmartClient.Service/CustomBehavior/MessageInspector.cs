using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;

namespace Trex.SmartClient.Service.CustomBehavior
{
    public class MessageInspector : IClientMessageInspector
    {
        private readonly string _userName;
        private readonly string _userPassword;
        private readonly string _customerId;

        public MessageInspector(string userName, string userPassword, string customerId)
        {
            _userName = userName;
            _userPassword = userPassword;
            _customerId = customerId;
        }

        public object BeforeSendRequest(ref Message request, System.ServiceModel.IClientChannel channel)
        {
            if (_userName != null)
            {
                var nameHeader = MessageHeader.CreateHeader("UserName", "Authentication", _userName);
                if (request.Headers.FindHeader("UserName", "Authentication") <= 0)
                {
                    request.Headers.Add(nameHeader);
                }

            }
            if (_userPassword != null)
            {
                var passwordHeader = MessageHeader.CreateHeader("UserPassword", "Authentication", _userPassword);
                if (request.Headers.FindHeader("UserPassword", "Authentication") <= 0)
                {
                    request.Headers.Add(passwordHeader);
                }
            }
            if (_customerId != null)
            {
                var idHeader = MessageHeader.CreateHeader("CustomerId", "Authentication", _customerId);
                if (request.Headers.FindHeader("CustomerId", "Authentication") <= 0)
                {
                    request.Headers.Add(idHeader);
                }
            }
            var clientApplicationTypeHeader = MessageHeader.CreateHeader("ClientApplicationType", "Authentication", 3);
            if (request.Headers.FindHeader("ClientApplicationType", "Authentication") <= 0)
            {
                request.Headers.Add(clientApplicationTypeHeader);
            }

            return null;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {

        }
    }
}
