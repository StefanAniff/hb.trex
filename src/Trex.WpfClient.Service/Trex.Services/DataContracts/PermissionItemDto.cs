using System.Runtime.Serialization;

namespace TrexSL.Web.DataContracts
{
    [DataContract]
    public class PermissionItemDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string PermissionName { get; set; }

        [DataMember]
        public bool IsEnabled { get; set; }
    }
}