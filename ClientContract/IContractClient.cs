using System;
using System.ServiceModel;

namespace Contracts
{
    public interface IContractClient
    {
        [OperationContract(IsOneWay = true)]
        void Write(Message message);

        [OperationContract(IsOneWay=true)]
        void Disconnect();
        
        [OperationContract(IsOneWay=true)]
        void NewUserRegistered(string user_name);

        [OperationContract(IsOneWay = true)]
        void UserUnregistered(string user_name);

        [OperationContract]
        byte[] Upload(int buffer_size, int file_index);
    }
}
