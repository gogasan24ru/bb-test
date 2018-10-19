using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

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
        string ListUsers(string sessionKey, byte[] hash);

        [OperationContract]
        string Login(string login, string password, byte[] hash);

        [OperationContract]
        string Logout(string sessionKey, byte[] hash);


        [OperationContract]
        string Register(User data, byte[] hash);

        //[OperationContract]
        //return MethodName(args?);
        //..

    }
}
