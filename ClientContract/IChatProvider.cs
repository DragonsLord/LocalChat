using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Contracts
{
    [DataContract]
    public enum ExceptionType 
    {
        [EnumMember]
        Unknown = 0,
        [EnumMember]
        UserAlreadyRegisterd = 1,
        [EnumMember]
        EmptyOrUnknownRecieverList = 2,
        [EnumMember]
        FileNotFound = 3,
        [EnumMember]
        ErrorDuringDataTransfer = 4
    }

    [ServiceContract(CallbackContract = typeof(IContractClient))]
    [ServiceKnownType(typeof(System.IO.FileStream))]
    public interface IChatProvider
    {
        [OperationContract]
        [FaultContract(typeof(ExceptionType))]
        void RegisterNewUser(string user_name);

        [OperationContract]
        void UnregisterUser(string user_name);

        [OperationContract]
        IEnumerable<string> GetUsersList();

        [OperationContract(IsOneWay = true)]
        void SendMessage(Message message);

        [OperationContract]
        [FaultContract(typeof(ExceptionType))]
        byte[] DownloadData(string file_adress, int buffer_size);
    }
}
