using System.Runtime.Serialization;
using Trex.Server.Core.Model;

namespace TrexSL.Web.DataContracts
{
    [DataContract]
    public class UserCreationResponse : ServerResponse
    {
        public UserCreationResponse(string responseMessage, bool success)
            : base(responseMessage, success) {}

        [DataMember]
        public User User { get; set; }
    }
}