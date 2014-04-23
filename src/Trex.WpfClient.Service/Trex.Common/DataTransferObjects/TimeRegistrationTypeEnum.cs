using System.Runtime.Serialization;

namespace Trex.Common.DataTransferObjects
{
    [DataContract]
    public enum TimeRegistrationTypeEnum
    {
        [EnumMember]
        Standard = 0,
        [EnumMember]
        Projection = 1,
        [EnumMember]
        Vacation,
    }
}