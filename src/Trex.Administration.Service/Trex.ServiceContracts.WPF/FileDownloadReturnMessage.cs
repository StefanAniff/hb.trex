using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Trex.ServiceContracts
{
    [MessageContract]
    public class FileDownloadReturnMessage
    {
        public FileDownloadReturnMessage( Stream stream) {this.FileByteStream = stream; }

        //[MessageHeader(MustUnderstand = true)]
        //public FileMetaData DownloadedFileMetadata;

        [MessageBodyMember(Order = 1)]
        public Stream FileByteStream;
    }

    [DataContract]
    public class FileMetaData
    {
        public FileMetaData(string localFileName, string remoteFileName)
        {
            this.LocalFileName = localFileName;
            this.RemoteFileName = remoteFileName;
            this.FileType = "Generic";
        }

        public FileMetaData(string localFileName, string remoteFileName, string fileType)
        {
            this.LocalFileName = localFileName;
            this.RemoteFileName = remoteFileName;
            this.FileType = fileType;
        }


        [DataMember(Name = "FileType", Order = 0, IsRequired = true)]
        public string FileType;

        [DataMember(Name = "localFilename", Order = 1, IsRequired = false)]
        public string LocalFileName;

        [DataMember(Name = "remoteFilename", Order = 2, IsRequired = false)]
        public string RemoteFileName;
    }
}
