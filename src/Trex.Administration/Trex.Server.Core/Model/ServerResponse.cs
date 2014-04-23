using System.Runtime.Serialization;

namespace Trex.Server.Core.Model
{
    [DataContract]
    public class ServerResponse
    {
        public ServerResponse(string responseMessage, bool success)
        {
            Response = responseMessage;
            Success = success;
        }

        [DataMember]
        public string Response { get; set; }

        [DataMember]
        public bool Success { get; set; }
    }
}