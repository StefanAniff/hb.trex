using System.Runtime.Serialization;

namespace Trex.ServiceContracts
{
    [DataContract]
    public class ExceptionInfo
    {
        public ExceptionInfo()
        {

        }

        public ExceptionInfo(string message, string exceptionDetails)
        {
            Message = message;
            ExceptionDetails = exceptionDetails;
        }


        [DataMember]
        public string ExceptionDetails { get; set; }

        [DataMember]
        public string Message { get; set; }
    }

}