﻿using System;
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
        Returnable ListUsers(string sessionKey, byte[] hash, string filterSet);

        [OperationContract]
        Returnable Login(string login, string password, byte[] hash);

        [OperationContract]
        Returnable Logout(string sessionKey, byte[] hash);


        [OperationContract]
        Returnable Register(User data, byte[] hash);

        //[OperationContract]
        //return MethodName(args?);
        //..

    }
}
