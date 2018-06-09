using System.Runtime.Serialization;
using System.ServiceModel;

namespace Manage.WcfService.Services
{
    [ServiceContract]
    public interface IServiceFile
    {
        [OperationContract]
        CustomFileInfo UpLoadFileInfo(CustomFileInfo fileInfo);

        [OperationContract]
        CustomFileInfo DeleteFile(CustomFileInfo fileInfo);
    }

    [DataContract]
    public class CustomFileInfo
    {
        [DataMember]
        public string Extension { get; set; }

        [DataMember]
        public string OldName { get; set; }

        [DataMember]
        public string NewName { get; set; }

        [DataMember]
        public string Path { get; set; }

        [DataMember]
        public string FileSize { get; set; }

        [DataMember]
        public byte[] SendByte { get; set; }

        [DataMember]
        public string SendByteStr { get; set; }

        [DataMember]
        public int State { get; set; }
    }
}
