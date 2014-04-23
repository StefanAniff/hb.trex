using System.ServiceModel;

namespace Trex.ServiceContracts
{
    [MessageContract]
    public class FileDownloadRequestMessage
    {
       [MessageBodyMember(Order = 1)]
        public FileMetaData FileMetaData;

    }
}
