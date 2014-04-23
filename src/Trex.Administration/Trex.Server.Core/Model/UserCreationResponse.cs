using System.Runtime.Serialization;

namespace Trex.Server.Core.Model
{
    [DataContract]
    public class UserCreationResponse : ServerResponse
    {
        public UserCreationResponse(string responseMessage, bool success, User user) : base(responseMessage, success)
        {
            User = user;
        }

        [DataMember]
        public User User { get; set; }
    }
}