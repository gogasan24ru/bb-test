using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.ServiceModel;

namespace backend2
{
    [DataContract]
    public class Returnable
    {
        private DateTime timestamp;
        private byte[] CheckSum;
        private Type DataTypeLocal;
        private object Data;

        public Returnable(Type DT, object D)
        {
            timestamp=DateTime.Now;
            Data = D;
            DataTypeLocal = DT;
            CheckSum = MD5.Create(timestamp.ToString()+GlobalVar.ServerSecret).Hash;
        }

        private bool CheckSumOk => MD5.Create(timestamp.ToString() + GlobalVar.ServerSecret).Hash.Equals(CheckSum);

        public DataTypeLocal ExtractData<DataTypeLocal>()//will it work??
        {
            return (DataTypeLocal)Data;
        }
    }
    /// <summary>
    /// Dummy global vars
    /// </summary>
    static class GlobalVar
    {
        /// <summary>
        /// Used in data validating. Because no https :(
        /// </summary>
        public const string ClientSecret = "ImMegaClientSecret";
        public const string ServerSecret = "ImMegaServerSecret";
    }
    [ServiceContract]
    interface IUsersManagementService
    {

        [OperationContract]
        bool Test(string input);

        [OperationContract]
        Returnable ListUsers(string sessionKey, byte[] hash);

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
