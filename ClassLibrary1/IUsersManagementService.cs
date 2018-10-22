using System;
using System.ServiceModel;

namespace ClassLibrary1
{
    [ServiceKnownType(typeof(Returnable))]//useless
    [ServiceKnownType(typeof(object))]//useless
    [ServiceKnownType(typeof(DateTime))]//useless
    [ServiceKnownType(typeof(Type))]//useless
    [ServiceKnownType(typeof(byte[]))]//useless
    [ServiceContract]
    public interface IUsersManagementService
    {

        [OperationContract]
        bool Test(string input);

        [OperationContract]
        Returnable ListUsers(UInt32 timestamp, string sessionKey, byte[] hash, int page, string filterSet);
        [OperationContract]
        Returnable GetUsersCount(UInt32 timestamp, string sessionKey, byte[] hash);

        [OperationContract]
        Returnable Login(UInt32 timestamp, string login, string password, byte[] hash);

        [OperationContract]
        Returnable Logout(UInt32 timestamp, string sessionKey, byte[] hash);


        [OperationContract]
        Returnable Register(UInt32 timestamp, User data, byte[] hash);

        //[OperationContract]
        //return MethodName(args?);
        //..

    }
}
