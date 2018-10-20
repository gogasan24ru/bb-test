using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using ClassLibrary1;

namespace backend2
{
    [ServiceKnownType(typeof(Returnable))]//useless
    [ServiceKnownType(typeof(object))]//useless
    [ServiceKnownType(typeof(DateTime))]//useless
    [ServiceKnownType(typeof(Type))]//useless
    [ServiceKnownType(typeof(byte[]))]//useless
    [ServiceContract]
    interface IUsersManagementService
    {

        [OperationContract]
        bool Test(string input);

        [OperationContract]
        Returnable ListUsers(UInt32 timestamp, string sessionKey, byte[] hash, string filterSet);

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
