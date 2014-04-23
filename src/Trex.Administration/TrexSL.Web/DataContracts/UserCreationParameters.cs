using System.Runtime.Serialization;

namespace TrexSL.Web.DataContracts
{
    [DataContract]
    public class UserCreationParameters
    {
        [DataMember]
        public User User { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string PasswordQuestion { get; set; }

        [DataMember]
        public string PasswordAnswer { get; set; }
    }
}